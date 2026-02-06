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
        private readonly ISkillsEnglandService _skillsEnglandService;
        private readonly IStandardImportRepository _standardImportRepository;
        private readonly IStandardRepository _standardRepository;
        private readonly IImportAuditRepository _auditRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly IRouteImportRepository _routeImportRepository;

        private readonly ISlackNotificationService _slackNotificationService;
        private readonly ILogger<StandardsImportService> _logger;

        private static readonly SemaphoreSlim _semaphoreImport = new SemaphoreSlim(1, 1);

        public StandardsImportService(
            ISkillsEnglandService skillsEnglandService,
            IStandardImportRepository standardImportRepository,
            IStandardRepository standardRepository,
            IImportAuditRepository auditRepository,
            IRouteRepository routeRepository,
            IRouteImportRepository routeImportRepository,
            ISlackNotificationService slackNotificationService,
            ILogger<StandardsImportService> logger)
        {
            _skillsEnglandService = skillsEnglandService;
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

                    var standards = await _skillsEnglandService.GetStandards();
                    var importedStandards = RemoveIndevelopmentVersions(standards);

                    _logger.LogInformation("{MethodName} - Retrieved Apprenticeships: {ApprenticeshipsCount} Foundation: {FoundationCount}", nameof(ImportDataIntoStaging), importedStandards.Count(c => c.ApprenticeshipType == ApprenticeshipType.Apprenticeship), importedStandards.Count(c => c.ApprenticeshipType == ApprenticeshipType.FoundationApprenticeship));

                    // if there are any missing fields in any standard or 
                    var validationFailures = new Dictionary<string, ValidationFailures>();
                    ValidateStandards(new Dictionary<string, List<Domain.ImportTypes.SkillsEngland.Standard>> { { "All", importedStandards } },
                        validationFailures, [new RequiredFieldsPresentValidator(), new ReferenceNumberFormatValidator(), new VersionFormatValidator()]);

                    var hasValidationErrors = validationFailures.Any(p => p.Value.Errors.Count > 0);
                    if (!hasValidationErrors)
                    {
                        var currentRoutes = await _routeRepository.GetAll();
                        var currentStandards = await _standardRepository.GetStandards(null);

                        var routeImports = await PrepareRouteImports(GetDistinctRoutes(importedStandards));
                        var groupedImportedStandards = GroupImportedStandards(importedStandards, routeImports);

                        // first the standard groups are validated between versions, invalid standard groups are replaced by retaining those previously imported
                        Dictionary<string, List<StandardImport>> validStandardImports = IndividuallyValidateStandardGroups(groupedImportedStandards, currentStandards, currentRoutes, validationFailures);
                        validStandardImports = ConcatRetainedStandards(validStandardImports, currentStandards);

                        // then the standard groups are cross validated with eachother, invalid standard groups are again replaced by retaining those previously imported
                        validStandardImports = CrossValidateStandardGroups(validStandardImports, validationFailures);
                        validStandardImports = ConcatRetainedStandards(validStandardImports, currentStandards);

                        // finally the standard groups are assessed to determine which version is the latest
                        validStandardImports = PopulateIsLatestVersions(validStandardImports);

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
                            $"{_slackNotificationService.FormattedTag()} The standard import from IfATE failed validation, the last successful run was {(DateTime.UtcNow - lastSuccessfulImport).Days} days ago.");
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
            foreach (var newRouteName in importedRoutes.ExceptBy(updatedRoutes.Select(x => x.Name), x => x.Name).Select(x => x.Name))
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

        private static List<Domain.ImportTypes.SkillsEngland.Standard> RemoveIndevelopmentVersions(IEnumerable<Domain.ImportTypes.SkillsEngland.Standard> standards)
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

        private static Dictionary<string, List<StandardImport>> PopulateIsLatestVersions(Dictionary<string, List<StandardImport>> validStandardImports)
        {
            if (validStandardImports == null || validStandardImports.Count == 0)
                return validStandardImports ?? [];

            foreach (var standards in validStandardImports.Values)
            {
                if (standards == null || standards.Count == 0)
                    continue;

                var ordered = standards
                    .OrderByDescending(s => s.Version.ParseVersion())
                    .ToList();

                var latestVersion = ordered[0].Version.ParseVersion();
                foreach (var s in ordered)
                {
                    s.IsLatestVersion = s.Version.ParseVersion().Equals(latestVersion);
                }
            }

            return validStandardImports;
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

        private static List<Domain.ImportTypes.Route> GetDistinctRoutes(List<Domain.ImportTypes.SkillsEngland.Standard> standards)
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

        private static Dictionary<string, List<Domain.ImportTypes.SkillsEngland.Standard>> GroupImportedStandards(List<Domain.ImportTypes.SkillsEngland.Standard> importedStandards, List<RouteImport> routes)
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

        private static void UpdateStandardsSectorId(IEnumerable<Domain.ImportTypes.SkillsEngland.Standard> standards,
             List<RouteImport> routes)
        {
            foreach (var standard in standards)
            {
                standard.RouteCode = routes.SingleOrDefault(c => c.Name.Equals(standard.Route))?.Id ?? 0;
            }
        }

        private static void UpdateStandardsEqaProviderName(List<Domain.ImportTypes.SkillsEngland.Standard> standards)
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
            (Dictionary<string, List<Domain.ImportTypes.SkillsEngland.Standard>> importedStandards,
            IEnumerable<Standard> currentStandards,
            IEnumerable<Route> currentRoutes,
            Dictionary<string, ValidationFailures> validationFailures)
        {
            var fatalValidators = new List<ValidatorBase<List<Domain.ImportTypes.SkillsEngland.Standard>>>
            {
                new LarsCodeIsNumberValidator(),
                new VersionsHaveNoGapsValidator()
            };

            var validStandards = ValidateStandards(importedStandards, validationFailures, fatalValidators);

            var otherValidators = new List<ValidatorBase<List<Domain.ImportTypes.SkillsEngland.Standard>>>
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
            var audit = await _auditRepository.GetLastImportByType(ImportType.SkillsEnglandImport);
            return audit?.TimeFinished;
        }
    }
}
