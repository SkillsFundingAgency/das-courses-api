using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

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
        private readonly ILogger<StandardsImportService> _logger;

        public StandardsImportService (
            IInstituteOfApprenticeshipService instituteOfApprenticeshipService, 
            IStandardImportRepository standardImportRepository, 
            IStandardRepository standardRepository,
            IImportAuditRepository auditRepository,
            IRouteRepository routeRepository,
            IRouteImportRepository routeImportRepository,
            ILogger<StandardsImportService> logger)
        {
            _instituteOfApprenticeshipService = instituteOfApprenticeshipService;
            _standardImportRepository = standardImportRepository;
            _standardRepository = standardRepository;
            _auditRepository = auditRepository;
            _routeRepository = routeRepository;
            _routeImportRepository = routeImportRepository;
            _logger = logger;
        }

        public async Task ImportDataIntoStaging()
        {
            try
            {
                _logger.LogInformation("Standards import - starting");

                var standards = (await _instituteOfApprenticeshipService.GetStandards()).ToList();

                _logger.LogInformation($"Standards import - Retrieved {standards.Count} standards from API");

                var routes = GetDistinctRoutesFromStandards(standards);

                await LoadRoutesInStaging(routes);

                UpdateStandardsWithRespectiveSectorId(standards, routes);

                var standardsImport = standards
                    .Select(c => (StandardImport)c)
                    .ToList();

                _standardImportRepository.DeleteAll();
                await _standardImportRepository.InsertMany(standardsImport);

                _logger.LogInformation("Standards import - starting");
            }
            catch (Exception e)
            {
                _logger.LogError("Standards import - an error occurred when trying to import data into staging.", e);
                throw;
            }
        }

        public async Task LoadDataFromStaging(DateTime timeStarted)
        {
            try
            {
                var routesToImport = await _routeImportRepository.GetAll();
                var standardsToInsert = (await _standardImportRepository.GetAll()).ToList();

                if (!standardsToInsert.Any())
                {
                    await AuditImport(timeStarted, 0);
                    _logger.LogWarning("Standards import - No standards loaded. No standards retrieved from API");
                    return;
                }

                _standardRepository.DeleteAll();
                _routeRepository.DeleteAll();
                
                _logger.LogInformation($"Standards import - Adding {standardsToInsert.Count} to Standards table.");

                await _routeRepository.InsertMany(routesToImport.Select(c => (Route) c).ToList());

                var standards = standardsToInsert.Select(c => (Standard)c).ToList();
                await _standardRepository.InsertMany(standards);

                await AuditImport(timeStarted, standards.Count);
                _logger.LogInformation("Standards import - complete");
            }
            catch (Exception e)
            {
                _logger.LogError("Standards import - an error occurred when trying to load data from staging.", e);
                throw;
            }
        }

        private static void UpdateStandardsWithRespectiveSectorId( IEnumerable<Domain.ImportTypes.Standard> standards,
             List<RouteImport> routes)
        {
            foreach (var standard in standards)
            {
                standard.RouteCode = routes.Single(c => c.Name.Equals(standard.Route)).Id;
            }
        }

        private async Task LoadRoutesInStaging(List<RouteImport> routes)
        {
            _routeImportRepository.DeleteAll();
            await _routeImportRepository.InsertMany(routes);
        }

        private static List<RouteImport> GetDistinctRoutesFromStandards(List<Domain.ImportTypes.Standard> standards)
        {
            var routeId = 1;
            return standards
                .Select(c => c.Route)
                .Distinct()
                .OrderBy(c=>c)
                .Select(c => new RouteImport
                {
                    Id = routeId ++,
                    Name = c
                })
                .ToList();
        }

        private async Task AuditImport(DateTime timeStarted, int rowsImported)
        {
            var auditRecord = new ImportAudit(timeStarted, rowsImported);
            await _auditRepository.Insert(auditRecord);
        }
    }
}
