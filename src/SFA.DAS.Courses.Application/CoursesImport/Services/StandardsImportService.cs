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
        private readonly ISectorRepository _sectorRepository;
        private readonly ISectorImportRepository _sectorImportRepository;
        private readonly ILogger<StandardsImportService> _logger;

        public StandardsImportService (
            IInstituteOfApprenticeshipService instituteOfApprenticeshipService, 
            IStandardImportRepository standardImportRepository, 
            IStandardRepository standardRepository,
            IImportAuditRepository auditRepository,
            ISectorRepository sectorRepository,
            ISectorImportRepository sectorImportRepository,
            ILogger<StandardsImportService> logger)
        {
            _instituteOfApprenticeshipService = instituteOfApprenticeshipService;
            _standardImportRepository = standardImportRepository;
            _standardRepository = standardRepository;
            _auditRepository = auditRepository;
            _sectorRepository = sectorRepository;
            _sectorImportRepository = sectorImportRepository;
            _logger = logger;
        }

        public async Task ImportDataIntoStaging()
        {
            try
            {
                _logger.LogInformation("Standards import - starting");

                var standards = (await _instituteOfApprenticeshipService.GetStandards()).ToList();

                _logger.LogInformation($"Standards import - Retrieved {standards.Count} standards from API");

                var sectors = GetDistinctSectorsFromStandards(standards);

                await LoadSectorsInStaging(sectors);

                UpdateStandardsWithRespectiveSectorId(standards, sectors);

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
                var sectorsToImport = await _sectorImportRepository.GetAll();
                var standardsToInsert = (await _standardImportRepository.GetAll()).ToList();

                if (!standardsToInsert.Any())
                {
                    await AuditImport(timeStarted, 0);
                    _logger.LogWarning("Standards import - No standards loaded. No standards retrieved from API");
                    return;
                }

                _standardRepository.DeleteAll();
                _sectorRepository.DeleteAll();

                _logger.LogInformation($"Standards import - Adding {standardsToInsert.Count} to Standards table.");

                await _sectorRepository.InsertMany(sectorsToImport.Select(c => (Sector)c).ToList());

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

        private static void UpdateStandardsWithRespectiveSectorId(List<Domain.ImportTypes.Standard> standards, IEnumerable<SectorImport> sectors)
        {
            foreach (var standard in standards)
            {
                standard.RouteId = sectors.Single(c => c.Route.Equals(standard.Route)).Id;
            }
        }

        private async Task LoadSectorsInStaging(IEnumerable<SectorImport> sectors)
        {
            _sectorImportRepository.DeleteAll();
            await _sectorImportRepository.InsertMany(sectors);
        }

        private static List<SectorImport> GetDistinctSectorsFromStandards(List<Domain.ImportTypes.Standard> standards)
        {
            return standards
                .Select(c => c.Route)
                .Distinct()
                .Select(c => new SectorImport
                {
                    Id = Guid.NewGuid(),
                    Route = c
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
