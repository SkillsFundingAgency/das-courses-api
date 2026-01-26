using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Services
{
    public class LarsImportService : ILarsImportService
    {
        private readonly ILarsPageParser _larsPageParser;
        private readonly IImportAuditRepository _importAuditRepository;
        private readonly IApprenticeshipFundingImportRepository _apprenticeshipFundingImportRepository;
        private readonly IFundingImportRepository _fundingImportRepository;
        private readonly ILarsStandardImportRepository _larsStandardImportRepository;
        private readonly IApprenticeshipFundingRepository _apprenticeshipFundingRepository;
        private readonly ILarsStandardRepository _larsStandardRepository;
        private readonly ISectorSubjectAreaTier2ImportRepository _sectorSubjectAreaTier2ImportRepository;
        private readonly ISectorSubjectAreaTier2Repository _sectorSubjectAreaTier2Repository;
        private readonly ISectorSubjectAreaTier1ImportRepository _sectorSubjectAreaTier1ImportRepository;
        private readonly ISectorSubjectAreaTier1Repository _sectorSubjectAreaTier1Repository;
        private readonly ILarsImportStagingService _larsImportStagingService;
        private readonly ILogger<LarsImportService> _logger;
        

        public LarsImportService(ILarsPageParser larsPageParser,
            IImportAuditRepository importAuditRepository,
            IApprenticeshipFundingImportRepository apprenticeshipFundingImportRepository,
            IFundingImportRepository fundingImportRepository,
            ILarsStandardImportRepository larsStandardImportRepository,
            IApprenticeshipFundingRepository apprenticeshipFundingRepository,
            ILarsStandardRepository larsStandardRepository,
            ISectorSubjectAreaTier2ImportRepository sectorSubjectAreaTier2ImportRepository,
            ISectorSubjectAreaTier2Repository sectorSubjectAreaTier2Repository,
            ISectorSubjectAreaTier1ImportRepository sectorSubjectAreaTier1ImportRepository,
            ISectorSubjectAreaTier1Repository sectorSubjectAreaTier1Repository,
            ILarsImportStagingService larsImportStaging,
            ILogger<LarsImportService> logger)
        {
            _larsPageParser = larsPageParser;
            _importAuditRepository = importAuditRepository;
            _apprenticeshipFundingImportRepository = apprenticeshipFundingImportRepository;
            _fundingImportRepository = fundingImportRepository; 
            _larsStandardImportRepository = larsStandardImportRepository;
            _apprenticeshipFundingRepository = apprenticeshipFundingRepository;
            _larsStandardRepository = larsStandardRepository;
            _sectorSubjectAreaTier2ImportRepository = sectorSubjectAreaTier2ImportRepository;
            _sectorSubjectAreaTier2Repository = sectorSubjectAreaTier2Repository;
            _sectorSubjectAreaTier1ImportRepository = sectorSubjectAreaTier1ImportRepository;
            _sectorSubjectAreaTier1Repository = sectorSubjectAreaTier1Repository;
            _larsImportStagingService = larsImportStaging;
            _logger = logger;
        }

        public async Task<(bool Success, string FileName)> ImportDataIntoStaging()
        {
            try
            {
                _logger.LogInformation("LARS Import - data into staging - started");

                var filePath = await _larsPageParser.GetCurrentLarsDataDownloadFilePath();

                await _larsImportStagingService.Import(filePath);

                _logger.LogInformation("LARS Import - data into staging - finished");

                return (true, filePath);
            }
            catch (Exception e)
            {
                _logger.LogError("LARS Import - an error occurred while trying to import data from LARS file.", e);
                throw;
            }
        }

        public async Task LoadDataFromStaging(DateTime importAuditStartTime, string filePath)
        {
            try
            {
                _logger.LogInformation("LARS Import - data load from staging - started");

                var larsStandardImports = _larsStandardImportRepository.GetAll();

                var sectorSubjectAreaTier2Imports = _sectorSubjectAreaTier2ImportRepository.GetAll();

                var sectorSubjectAreaTier1Imports = _sectorSubjectAreaTier1ImportRepository.GetAll();

                await Task.WhenAll(larsStandardImports, sectorSubjectAreaTier2Imports, sectorSubjectAreaTier1Imports);

                await _larsStandardRepository.DeleteAll();
                await _sectorSubjectAreaTier2Repository.DeleteAll();
                await _sectorSubjectAreaTier1Repository.DeleteAll();

                var importLarsStandardTask =
                    _larsStandardRepository.InsertMany(larsStandardImports.Result
                        .Select(c => (LarsStandard)c).ToList());
                var importSectorSubjectAreaTier2Task =
                    _sectorSubjectAreaTier2Repository.InsertMany(sectorSubjectAreaTier2Imports.Result
                        .Select(c => (SectorSubjectAreaTier2)c).ToList());
                var importSectorSubjectAreaTier1Task =
                    _sectorSubjectAreaTier1Repository.InsertMany(sectorSubjectAreaTier1Imports.Result
                        .Select(c => (SectorSubjectAreaTier1)c).ToList());
                var importApprenticeshipFundingTask = LoadApprenticeshipFunding();

                await Task.WhenAll(importLarsStandardTask, importApprenticeshipFundingTask, importSectorSubjectAreaTier2Task, importSectorSubjectAreaTier1Task);

                var rowsImported = larsStandardImports.Result.Count() +
                                   importApprenticeshipFundingTask.Result +
                                   sectorSubjectAreaTier2Imports.Result.Count();

                await _importAuditRepository.Insert(new ImportAudit(importAuditStartTime,
                    rowsImported, ImportType.LarsImport, filePath));

                _logger.LogInformation("LARS Import - data load from staging - finished");
            }
            catch (Exception e)
            {
                _logger.LogError("LARS Import - an error occurred while trying to load data from staging tables.", e);
                throw;
            }
        }

        private async Task<int> LoadApprenticeshipFunding()
        {
            var fundingImportTask = _fundingImportRepository.GetAll();
            var apprenticeshipFundingImportTask = _apprenticeshipFundingImportRepository.GetAll();

            await Task.WhenAll(fundingImportTask, apprenticeshipFundingImportTask);

            await _apprenticeshipFundingRepository.DeleteAll();

            var fundings = fundingImportTask.Result
                .Select(c => (ApprenticeshipFunding)c).ToList();
            await _apprenticeshipFundingRepository.InsertMany(fundings);

            var apprenticeshipFundings = apprenticeshipFundingImportTask.Result
                .Select(c => (ApprenticeshipFunding)c).ToList();
            await _apprenticeshipFundingRepository.InsertMany(apprenticeshipFundings);

            return fundings.Count + apprenticeshipFundings.Count;
        }
    }
}
