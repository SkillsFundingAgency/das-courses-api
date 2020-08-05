using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Services
{
    public class LarsImportService  : ILarsImportService
    {
        private readonly ILarsPageParser _larsPageParser;
        private readonly ILarsDataDownloadService _larsDataDownloadService;
        private readonly IImportAuditRepository _importAuditRepository;
        private readonly IApprenticeshipFundingImportRepository _apprenticeshipFundingImportRepository;
        private readonly ILarsStandardImportRepository _larsStandardImportRepository;
        private readonly IApprenticeshipFundingRepository _apprenticeshipFundingRepository;
        private readonly ILarsStandardRepository _larsStandardRepository;
        private readonly ISectorSubjectAreaTier2ImportRepository _sectorSubjectAreaTier2ImportRepository;
        private readonly ISectorSubjectAreaTier2Repository _sectorSubjectAreaTier2Repository;
        private readonly IZipArchiveHelper _zipArchiveHelper;
        private readonly ILogger<LarsImportService> _logger;

        public LarsImportService (ILarsPageParser larsPageParser, 
            ILarsDataDownloadService larsDataDownloadService, 
            IImportAuditRepository importAuditRepository,
            IApprenticeshipFundingImportRepository apprenticeshipFundingImportRepository,
            ILarsStandardImportRepository larsStandardImportRepository, 
            IApprenticeshipFundingRepository apprenticeshipFundingRepository,
            ILarsStandardRepository larsStandardRepository,
            ISectorSubjectAreaTier2ImportRepository sectorSubjectAreaTier2ImportRepository,
            ISectorSubjectAreaTier2Repository sectorSubjectAreaTier2Repository,
            IZipArchiveHelper zipArchiveHelper,
            ILogger<LarsImportService> logger)
        {
            _larsPageParser = larsPageParser;
            _larsDataDownloadService = larsDataDownloadService;
            _importAuditRepository = importAuditRepository;
            _apprenticeshipFundingImportRepository = apprenticeshipFundingImportRepository;
            _larsStandardImportRepository = larsStandardImportRepository;
            _apprenticeshipFundingRepository = apprenticeshipFundingRepository;
            _larsStandardRepository = larsStandardRepository;
            _sectorSubjectAreaTier2ImportRepository = sectorSubjectAreaTier2ImportRepository;
            _sectorSubjectAreaTier2Repository = sectorSubjectAreaTier2Repository;
            _zipArchiveHelper = zipArchiveHelper;
            _logger = logger;
        }
        public async Task ImportData()
        {
            try
            {
                _logger.LogInformation("LARS Import - commencing");
                var lastFilePath =
                    _importAuditRepository.GetLastImportByType(ImportType.LarsImport);
                var filePath = _larsPageParser.GetCurrentLarsDataDownloadFilePath();

                await Task.WhenAll(lastFilePath, filePath);

                if (lastFilePath.Result != null && filePath.Result.Equals(
                    lastFilePath.Result.FileName, StringComparison.CurrentCultureIgnoreCase))
                {
                    _logger.LogInformation("LARS Import - no new data to import");
                    return;
                }

                _logger.LogInformation($"LARS import - starting import of {filePath.Result}");
                var importAuditStartTime = DateTime.UtcNow;
                var content = await _larsDataDownloadService.GetFileStream(filePath.Result);

                await InsertDataFromZipStreamToImportTables(content);

                _logger.LogInformation("LARS Import - starting data transfer from import tables");
                _larsStandardRepository.DeleteAll();
                _apprenticeshipFundingRepository.DeleteAll();
                _sectorSubjectAreaTier2Repository.DeleteAll();

                var larsStandardImports = _larsStandardImportRepository.GetAll();
                var apprenticeshipFundingImports = _apprenticeshipFundingImportRepository.GetAll();
                var sectorSubjectAreaTier2Imports = _sectorSubjectAreaTier2ImportRepository.GetAll();

                await Task.WhenAll(larsStandardImports, apprenticeshipFundingImports, sectorSubjectAreaTier2Imports);

                var importLarsStandardResult =
                    _larsStandardRepository.InsertMany(larsStandardImports.Result
                        .Select(c => (LarsStandard)c).ToList());
                var importApprenticeshipFundingResult =
                    _apprenticeshipFundingRepository.InsertMany(apprenticeshipFundingImports.Result
                        .Select(c => (ApprenticeshipFunding)c).ToList());
                var importSectorSubjectAreaTier2Result =
                    _sectorSubjectAreaTier2Repository.InsertMany(sectorSubjectAreaTier2Imports.Result
                        .Select(c => (SectorSubjectAreaTier2) c).ToList());

                await Task.WhenAll(importLarsStandardResult, importApprenticeshipFundingResult, importSectorSubjectAreaTier2Result);

                var rowsImported = larsStandardImports.Result.Count() +
                                   apprenticeshipFundingImports.Result.Count() +
                                   sectorSubjectAreaTier2Imports.Result.Count();

                await _importAuditRepository.Insert(new ImportAudit(importAuditStartTime,
                    rowsImported, ImportType.LarsImport, filePath.Result));
                _logger.LogInformation("LARS Import - finished data transfer from import tables");
            }
            catch (Exception e)
            {
                _logger.LogError("LARS Import - failed", e);
                throw;
            }
        }

        private async Task InsertDataFromZipStreamToImportTables(Stream content)
        {
            _logger.LogInformation("LARS Import - starting extract from ZIP");
            var standardsCsv = _zipArchiveHelper
                .ExtractModelFromCsvFileZipStream<StandardCsv>(content, Constants.LarsStandardsFileName);
            var apprenticeshipFundingCsv = _zipArchiveHelper
                .ExtractModelFromCsvFileZipStream<ApprenticeshipFundingCsv>(content,
                    Constants.LarsApprenticeshipFundingFileName);
            var sectorSubjectAreaTier2Csv = _zipArchiveHelper
                .ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier2Csv>(content,
                    Constants.LarsSectorSubjectAreaTier2FileName);

            _larsStandardImportRepository.DeleteAll();
            _apprenticeshipFundingImportRepository.DeleteAll();
            _sectorSubjectAreaTier2ImportRepository.DeleteAll();

            var larsImportResult = _larsStandardImportRepository
                .InsertMany(standardsCsv.Select(c => (LarsStandardImport) c).ToList());

            var filterRecords = apprenticeshipFundingCsv
                .Where(c => c.ApprenticeshipType.StartsWith("STD", StringComparison.CurrentCultureIgnoreCase))
                .Select(c => (ApprenticeshipFundingImport) c).ToList();
            
            var apprenticeFundingImportResult =
                _apprenticeshipFundingImportRepository.InsertMany(filterRecords);

            var sectorSubjectAreaTier2ImportResult =
                _sectorSubjectAreaTier2ImportRepository.InsertMany(sectorSubjectAreaTier2Csv
                    .Select(x => (SectorSubjectAreaTier2Import) x).ToList());

            await Task.WhenAll(larsImportResult, apprenticeFundingImportResult, sectorSubjectAreaTier2ImportResult);
            
            _logger.LogInformation("LARS Import - finished load into Import tables");
        }
    }
}
