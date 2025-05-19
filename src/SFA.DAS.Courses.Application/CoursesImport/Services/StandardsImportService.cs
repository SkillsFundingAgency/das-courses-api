using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Application.CoursesImport.Extensions.StringExtensions;
using SFA.DAS.Courses.Application.CoursesImport.Validators;
using SFA.DAS.Courses.Application.Exceptions;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using Standard = SFA.DAS.Courses.Domain.Entities.Standard;

namespace SFA.DAS.Courses.Application.CoursesImport.Services
{
    public class StandardsImportService : IStandardsImportService
    {
        private readonly IInstituteOfApprenticeshipService _instituteOfApprenticeshipService;
        private readonly IStandardImportRepository _standardImportRepository;
        private readonly IStandardRepository _standardRepository;
        private readonly IImportAuditRepository _auditRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly IRouteImportRepository _routeImportRepository;

        private readonly ISlackNotificationService _slackNotificationService;
        private readonly ILogger<StandardsImportService> _logger;

        private static readonly SemaphoreSlim _semaphoreImport = new SemaphoreSlim(1, 1);

        public StandardsImportService(
            IInstituteOfApprenticeshipService instituteOfApprenticeshipService,
            IStandardImportRepository standardImportRepository,
            IStandardRepository standardRepository,
            IImportAuditRepository auditRepository,
            IRouteRepository routeRepository,
            IRouteImportRepository routeImportRepository,
            ISlackNotificationService slackNotificationService,
            ILogger<StandardsImportService> logger)
        {
            _instituteOfApprenticeshipService = instituteOfApprenticeshipService;
            _standardImportRepository = standardImportRepository;
            _standardRepository = standardRepository;
            _auditRepository = auditRepository;
            _routeRepository = routeRepository;
            _routeImportRepository = routeImportRepository;
            _slackNotificationService = slackNotificationService;
            _logger = logger;
        }

        public async Task<bool> ImportDataIntoStaging()
        {
            var dataSuccessfullyStaged = false;

            if (await _semaphoreImport.WaitAsync(0))
            {
                try
                {
                    _logger.LogInformation("{MethodName} - starting", nameof(ImportDataIntoStaging));

                    var importedStandards = RemoveIndevelopmentVersions(await _instituteOfApprenticeshipService.GetStandards());

                    _logger.LogInformation("{MethodName} - Retrieved {StandardsCount} standards from API", nameof(ImportDataIntoStaging), importedStandards.Count);

                    // if there are any missing fields in any standard or 
                    var validationFailures = new Dictionary<string, ValidationFailures>();
                    ValidateStandards(new Dictionary<string, List<Domain.ImportTypes.Standard>> { { "All", importedStandards } },
                        validationFailures, [new RequiredFieldsPresentValidator(), new ReferenceNumberFormatValidator(), new VersionFormatValidator()]);

                    var hasValidationErrors = validationFailures.Any(p => p.Value.Errors.Count > 0);
                    if (!hasValidationErrors)
                    {
                        var currentRoutes = await _routeRepository.GetAll();
                        var currentStandards = await _standardRepository.GetStandards();

                        var routeImports = await PrepareRouteImports(GetDistinctRoutes(importedStandards));
                        var groupedImportedStandards = GroupImportedStandards(importedStandards, routeImports);

                        var validStandardImports = IndividuallyValidateStandardGroups(groupedImportedStandards, currentStandards, currentRoutes, validationFailures);
                        validStandardImports = ConcatRetainedStandards(validStandardImports, currentStandards);

                        // cross validation must include the retained standards after individual validation
                        validStandardImports = CrossValidateStandardGroups(validStandardImports, validationFailures);
                        validStandardImports = ConcatRetainedStandards(validStandardImports, currentStandards);

                        hasValidationErrors = validationFailures.Any(p => p.Value.Errors.Count > 0);
                        if (!hasValidationErrors)
                        {
                            await ImportRouteDataIntoStaging(routeImports, validStandardImports);
                            await ImportStandardDataIntoStaging(validStandardImports);

                            dataSuccessfullyStaged = true;
                        }
                    }

                    // if there were any validation issues then report them via the slack notification
                    var sortedKeys = validationFailures.Keys.OrderBy(k => k).ToList();
                    var allMessages = sortedKeys
                        .SelectMany(key => validationFailures[key].Errors.Select(error => error))
                        .Concat(sortedKeys.SelectMany(key => validationFailures[key].StandardErrors.Select(standardError => standardError)))
                        .Concat(sortedKeys.SelectMany(key => validationFailures[key].Warnings.Select(warning => warning)))
                        .ToList();

                    if (allMessages.Any())
                    {
                        var lastSuccessfulImport = await LastSuccessfullImport() ?? DateTime.UtcNow;
                        await _slackNotificationService.UploadFile(
                            allMessages,
                            $"IfATE_Validation_Results_{DateTime.Now.ToFileTimeUtc()}.txt",
                            $"{_slackNotificationService.FormattedTag()} The standard import from IfATE failed validation, the last successfull run was {(DateTime.UtcNow - lastSuccessfulImport).Days} days ago.");
                    }

                    _logger.LogInformation("{MethodName} - finished", nameof(ImportDataIntoStaging));
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "{MethodName} - error whilst importing data into staging", nameof(ImportDataIntoStaging));
                    dataSuccessfullyStaged = false;
                }
                finally
                {
                    _semaphoreImport.Release();
                }
            }
            else
            {
                _logger.LogInformation("{MethodName} - importing data into staging is already running", nameof(ImportDataIntoStaging));
            }

            return dataSuccessfullyStaged;
        }

        public async Task ImportStandardDataIntoStaging(Dictionary<string, List<StandardImport>> standardImports)
        {
            await _standardImportRepository.DeleteAll();
            if (standardImports.Any())
            {
                await _standardImportRepository.InsertMany(standardImports.SelectMany(s => s.Value).ToList());
            }
        }

        public async Task<List<RouteImport>> PrepareRouteImports(List<Domain.ImportTypes.Route> importedRoutes)
        {
            var currentRoutes = await _routeRepository.GetAll();
            var lastRouteId = currentRoutes.OrderBy(c => c.Id).LastOrDefault()?.Id ?? 0;

            var updatedRoutes = currentRoutes.Select(c => (RouteImport)c).ToList();
            foreach(var newRouteName in importedRoutes.ExceptBy(updatedRoutes.Select(x => x.Name), x => x.Name).Select(x => x.Name))
            {
                updatedRoutes.Add(new RouteImport { Id = ++lastRouteId, Name = newRouteName, Active = true });
            }

            return updatedRoutes;
        }

        public async Task ImportRouteDataIntoStaging(List<RouteImport> routeImports, Dictionary<string, List<StandardImport>> standardImports)
        {
            await _routeImportRepository.DeleteAll();

            if (routeImports.Any())
            {
                foreach (var removedRoute in routeImports.Where(r => !standardImports.SelectMany(s => s.Value).Any(s => s.RouteCode == r.Id)))
                {
                    removedRoute.Active = false;
                }

                await _routeImportRepository.InsertMany(routeImports);
            }
        }

        public async Task LoadDataFromStaging(DateTime timeStarted)
        {
            try
            {
                int standardsTransfered = await LoadStandardDataFromStaging();
                if (standardsTransfered == 0)
                {
                    await AuditImport(timeStarted, standardsTransfered);
                    _logger.LogInformation("{MethodName} - No standards transfered. No standards retrieved from API", nameof(LoadDataFromStaging));
                    return;
                }

                await LoadRouteDataFromStaging();
                
                await AuditImport(timeStarted, standardsTransfered);
                
                _logger.LogInformation("{MethodName} - finished", nameof(LoadDataFromStaging));
            }
            catch (Exception e)
            {
                throw new ImportStandardsException($"{nameof(LoadDataFromStaging)} - error whilst loading data from staging.", e);
            }
        }

        private static List<Domain.ImportTypes.Standard> RemoveIndevelopmentVersions(IEnumerable<Domain.ImportTypes.Standard> standards)
        {
            var inDevelopmentStatuses = new List<string> { Domain.Courses.Status.InDevelopment, Domain.Courses.Status.ProposalInDevelopment };

            return standards
                .Where(p =>
                {
                    var (Major, Minor) = p.Version.Value.ParseVersion();
                    return 
                        Major < 1 ||
                        (Major == 1 && Minor == 0) ||
                        !inDevelopmentStatuses.Contains(p.Status.Value, StringComparer.OrdinalIgnoreCase);
                })
                .ToList();
        }

        private static Dictionary<string, List<StandardImport>> ConcatRetainedStandards(Dictionary<string, List<StandardImport>> validStandardImports, IEnumerable<Standard> currentStandards)
        {
            var currentStandardsToRetain = currentStandards
                .Where(x => !validStandardImports.Keys.Contains(x.IfateReferenceNumber));

            var groupedCurrentStandardsToRetain = currentStandardsToRetain
                .Select(s => (StandardImport)s)
                .GroupBy(s => s.IfateReferenceNumber)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(s => s.Version.ParseVersion())
                        .ToList()
                );

            return validStandardImports.Concat(groupedCurrentStandardsToRetain).ToDictionary();
        }

        private async Task<int> LoadStandardDataFromStaging()
        {
            var standardImports = (await _standardImportRepository.GetAll())
                .GroupBy(s => s.IfateReferenceNumber)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList()
                );

            if (!standardImports.Any())
            {
                return 0;
            }

            var standards = standardImports.SelectMany(s => s.Value).Select(s => (Standard)s).ToList();
            await _standardRepository.DeleteAll();
            return await _standardRepository.InsertMany(standards);
        }

        private async Task<int> LoadRouteDataFromStaging()
        {
            await _routeRepository.DeleteAll();
            
            var routesToImport = await _routeImportRepository.GetAll();
            return await _routeRepository.InsertMany(routesToImport.Select(c => (Route)c).ToList());
        }

        private static List<Domain.ImportTypes.Route> GetDistinctRoutes(List<Domain.ImportTypes.Standard> standards)
        {
            return standards
                .Where(c => (c.Status.Value?.Equals(Domain.Courses.Status.ApprovedForDelivery, StringComparison.CurrentCultureIgnoreCase) ?? false) && 
                            !string.IsNullOrEmpty(c.Route.Value))
                .Select(c => c.Route.Value)
                .Distinct()
                .OrderBy(c => c)
                .Select(c => (Domain.ImportTypes.Route)c)
                .ToList();
        }

        private static Dictionary<string, List<Domain.ImportTypes.Standard>> GroupImportedStandards(List<Domain.ImportTypes.Standard> importedStandards, List<RouteImport> routes)
        {
            UpdateStandardsSectorId(importedStandards, routes);
            UpdateStandardsEqaProviderName(importedStandards);

            var groupedStandards = importedStandards
                .GroupBy(s => s.ReferenceNumber.Value)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(s => s.Version.Value.ParseVersion())
                          .ToList()
                );

            foreach (var group in groupedStandards)
            {
                var referenceNumber = group.Key;
                var standardsList = group.Value;

                var duplicates = standardsList
                    .GroupBy(s => new { ReferenceNumber = s.ReferenceNumber.Value, Version = s.Version.Value })
                    .Where(g => g.Count() > 1)
                    .Select(g => new
                    {
                        g.Key.ReferenceNumber,
                        g.Key.Version,
                        Standards = g.ToList()
                    });

                foreach (var duplicate in duplicates)
                {
                    var latestStandard = duplicate.Standards
                        .OrderByDescending(s => s.CreatedDate.Value)
                        .FirstOrDefault();

                    standardsList.RemoveAll(s => s.ReferenceNumber.Value == duplicate.ReferenceNumber && s.Version.Value == duplicate.Version);
                    standardsList.Add(latestStandard);
                }

                groupedStandards[referenceNumber] = standardsList;
            }

            return groupedStandards;
        }

        private static void UpdateStandardsSectorId(IEnumerable<Domain.ImportTypes.Standard> standards,
             List<RouteImport> routes)
        {
            foreach (var standard in standards)
            {
                standard.RouteCode = routes.SingleOrDefault(c => c.Name.Equals(standard.Route))?.Id ?? 0;
            }
        }

        private static void UpdateStandardsEqaProviderName(List<Domain.ImportTypes.Standard> standards)
        {
            foreach (var standard in standards)
            {
                if (standard.EqaProvider.HasValue && standard.EqaProvider.Value.ProviderName.Value?.ToLower() == "ofqual is the intended eqa provider")
                {
                    standard.EqaProvider.Value.ProviderName = "Ofqual";
                }
            }
        }

        public static Dictionary<string, List<StandardImport>> IndividuallyValidateStandardGroups
            (Dictionary<string, List<Domain.ImportTypes.Standard>> importedStandards, 
            IEnumerable<Standard> currentStandards,
            IEnumerable<Route> currentRoutes,
            Dictionary<string, ValidationFailures> validationFailures)
        {
            var fatalValidators = new List<ValidatorBase<List<Domain.ImportTypes.Standard>>>
            {
                new LarsCodeIsNumberValidator(),
                new VersionsHaveNoGapsValidator()
            };

            var validStandards = ValidateStandards(importedStandards, validationFailures, fatalValidators);
           
            var otherValidators = new List<ValidatorBase<List<Domain.ImportTypes.Standard>>>
            {
                new LarsCodeNotZeroTwoWeeksAfterPublishValidator(),
                new LarsCodeNotZeroForNewVersionValidator(),
                new LarsCodeNotZeroForRetiredVersionValidator(),
                new StatusValidValidator(),
                new StatusRecommendedValidator(),
                new VersionsSingleApprovedValidator(),
                new VersionMustMatchVersionNumberValidator(),
                new StartDatesValidator(),
                new CourseOptionsPresentValidator(),
                new CourseOptionsPreservedValidator(currentStandards.ToList()),
                new PreviouslyDefinedRoutesValidator(currentRoutes.ToList()),
                new TitleValidator(),
                new CreatedDateValidator()
            };

            // further validations are done for standards without a fatal error, as a fatal error
            // may indicate that the data structure is corrupted or incomplete and would cause false
            // positives or negatives in other validation rules
            validStandards = ValidateStandards(validStandards, validationFailures, otherValidators);

            return validStandards
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Select(s => (StandardImport)s).ToList());
        }

        public static Dictionary<string, List<StandardImport>> CrossValidateStandardGroups(Dictionary<string, List<StandardImport>> standardImports,
            Dictionary<string, ValidationFailures> validationFailures)
        {
            var validators = new List<ValidatorBase<List<StandardImport>>>
            {
                new LarsCodeNotDuplicatedValidator(standardImports),
            };

            var validStandards = ValidateStandards(standardImports, validationFailures, validators);

            return validStandards;
        }

        public static Dictionary<string, List<T>> ValidateStandards<T>(Dictionary<string, List<T>> standardImports,
            Dictionary<string, ValidationFailures> validationFailures, List<ValidatorBase<List<T>>> validators)
        {
            foreach (var validator in validators)
            {
                foreach (var entry in standardImports)
                {
                    var result = validator.Validate(entry.Value);
                    if (!result.IsValid)
                    {
                        if (!validationFailures.ContainsKey(entry.Key))
                        {
                            validationFailures.Add(entry.Key, new ValidationFailures());
                        }

                        validationFailures[entry.Key].AddValidationFailure(validator.ValidationFailureType, result.Errors[0].ErrorMessage);
                    }
                }
            }

            var validStandards = new Dictionary<string, List<T>>();
            if (validationFailures.Values.All(p => !p.Errors.Any()))
            {
                validStandards = standardImports.Where(p => !validationFailures.ContainsKey(p.Key) || !validationFailures[p.Key].StandardErrors.Any()).ToDictionary();
            }

            return validStandards;
        }

        private async Task AuditImport(DateTime timeStarted, int rowsImported)
        {
            var auditRecord = new ImportAudit(timeStarted, rowsImported);
            await _auditRepository.Insert(auditRecord);
        }

        private async Task<DateTime?> LastSuccessfullImport()
        {
            var audit = await _auditRepository.GetLastImportByType(ImportType.IFATEImport);
            return audit?.TimeFinished;
        }
    }
}
