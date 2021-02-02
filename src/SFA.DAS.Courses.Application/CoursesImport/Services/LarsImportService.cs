using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Services
{
    public class LarsImportService  : ILarsImportService
    {
        private readonly ILarsPageParser _larsPageParser;
        private readonly IImportAuditRepository _importAuditRepository;
        private readonly IApprenticeshipFundingImportRepository _apprenticeshipFundingImportRepository;
        private readonly ILarsStandardImportRepository _larsStandardImportRepository;
        private readonly IApprenticeshipFundingRepository _apprenticeshipFundingRepository;
        private readonly ILarsStandardRepository _larsStandardRepository;
        private readonly ISectorSubjectAreaTier2ImportRepository _sectorSubjectAreaTier2ImportRepository;
        private readonly ISectorSubjectAreaTier2Repository _sectorSubjectAreaTier2Repository;
        private readonly IStandardImportRepository _standardImportRepository;
        private readonly ILogger<LarsImportService> _logger;
        private readonly LarsImportStaging _larsImportStaging;

        public LarsImportService (ILarsPageParser larsPageParser, 
            IDataDownloadService dataDownloadService, 
            IImportAuditRepository importAuditRepository,
            IApprenticeshipFundingImportRepository apprenticeshipFundingImportRepository,
            ILarsStandardImportRepository larsStandardImportRepository, 
            IApprenticeshipFundingRepository apprenticeshipFundingRepository,
            ILarsStandardRepository larsStandardRepository,
            ISectorSubjectAreaTier2ImportRepository sectorSubjectAreaTier2ImportRepository,
            ISectorSubjectAreaTier2Repository sectorSubjectAreaTier2Repository,
            IStandardImportRepository standardImportRepository,
            IZipArchiveHelper zipArchiveHelper,
            ILogger<LarsImportService> logger)
        {
            _larsPageParser = larsPageParser;
            _importAuditRepository = importAuditRepository;
            _apprenticeshipFundingImportRepository = apprenticeshipFundingImportRepository;
            _larsStandardImportRepository = larsStandardImportRepository;
            _apprenticeshipFundingRepository = apprenticeshipFundingRepository;
            _larsStandardRepository = larsStandardRepository;
            _sectorSubjectAreaTier2ImportRepository = sectorSubjectAreaTier2ImportRepository;
            _sectorSubjectAreaTier2Repository = sectorSubjectAreaTier2Repository;
            _standardImportRepository = standardImportRepository;
            _logger = logger;
            _larsImportStaging = new LarsImportStaging(
                dataDownloadService,
                zipArchiveHelper,
                _apprenticeshipFundingImportRepository,
                _larsStandardImportRepository,
                _sectorSubjectAreaTier2ImportRepository,
                _logger);
        }

        public async Task<(bool Success, string FileName)> ImportDataIntoStaging()
        {
            try
            {
                _logger.LogInformation("LARS Import - data into staging - started");

                var lastFilePath = _importAuditRepository.GetLastImportByType(ImportType.LarsImport);
                var filePath = _larsPageParser.GetCurrentLarsDataDownloadFilePath();

                await Task.WhenAll(lastFilePath, filePath);

                if (lastFilePath.Result != null && filePath.Result.Equals(
                    lastFilePath.Result.FileName, StringComparison.CurrentCultureIgnoreCase))
                {
                    _logger.LogInformation("LARS Import - no new data to import");
                    return (false, null);
                }

                await _larsImportStaging.Import(filePath.Result);

                _logger.LogInformation("LARS Import - data into staging - finished");

                return (true, filePath.Result);
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

                await Task.WhenAll(larsStandardImports, sectorSubjectAreaTier2Imports);

                _larsStandardRepository.DeleteAll();
                
                _sectorSubjectAreaTier2Repository.DeleteAll();

                var importLarsStandardTask =
                    _larsStandardRepository.InsertMany(larsStandardImports.Result
                        .Select(c => (LarsStandard)c).ToList());
                var importSectorSubjectAreaTier2Task =
                    _sectorSubjectAreaTier2Repository.InsertMany(sectorSubjectAreaTier2Imports.Result
                        .Select(c => (SectorSubjectAreaTier2)c).ToList());

                var importApprenticeshipFundingTask = LoadApprenticeshipFunding();

                await Task.WhenAll(importLarsStandardTask, importApprenticeshipFundingTask, importSectorSubjectAreaTier2Task);

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
            var standardsTask = _standardImportRepository.GetAll();
            var apprenticeshipFundingImportsTask = _apprenticeshipFundingImportRepository.GetAll();

            await Task.WhenAll(standardsTask, apprenticeshipFundingImportsTask);

            var fundings = standardsTask.Result.Join(
                apprenticeshipFundingImportsTask.Result,
                standard => standard.LarsCode,
                funding => funding.LarsCode,
                (standard, apprenticeshipFundingImport) => ApprenticeshipFunding.ConvertFrom(apprenticeshipFundingImport, standard.StandardUId))
                .ToList();

            _apprenticeshipFundingRepository.DeleteAll();

            var importApprenticeshipFundingResult = _apprenticeshipFundingRepository.InsertMany(fundings);

            return fundings.Count();
        }
    }
}
