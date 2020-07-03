using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.StandardsImport.Services
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
        private readonly IZipArchiveHelper _zipArchiveHelper;
        private readonly ILogger<LarsImportService> _logger;

        public LarsImportService (ILarsPageParser larsPageParser, 
            ILarsDataDownloadService larsDataDownloadService, 
            IImportAuditRepository importAuditRepository,
            IApprenticeshipFundingImportRepository apprenticeshipFundingImportRepository,
            ILarsStandardImportRepository larsStandardImportRepository, 
            IApprenticeshipFundingRepository apprenticeshipFundingRepository,
            ILarsStandardRepository larsStandardRepository,
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
            _zipArchiveHelper = zipArchiveHelper;
            _logger = logger;
        }
        public async Task ImportData()
        {
            var lastFilePath = _importAuditRepository.GetLastImportByType(ImportType.LarsImport);
            var filePath = _larsPageParser.GetCurrentLarsDataDownloadFilePath();

            await Task.WhenAll(lastFilePath, filePath);

            if (lastFilePath.Result != null && filePath.Result.Equals(lastFilePath.Result.FileName, StringComparison.CurrentCultureIgnoreCase))
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

            var larsStandardImports = _larsStandardImportRepository.GetAll();
            var apprenticeshipFundingImports = _apprenticeshipFundingImportRepository.GetAll();

            await Task.WhenAll(larsStandardImports, apprenticeshipFundingImports);

            var importLarsStandardResult =
                _larsStandardRepository.InsertMany(larsStandardImports.Result.Select(c => (LarsStandard) c).ToList());
            var importApprenticeshipFundingResult =
                _apprenticeshipFundingRepository.InsertMany(apprenticeshipFundingImports.Result.Select(c => (ApprenticeshipFunding) c).ToList());
            
            await Task.WhenAll(importLarsStandardResult, importApprenticeshipFundingResult);

            var rowsImported = larsStandardImports.Result.Count() + apprenticeshipFundingImports.Result.Count();
            
            await _importAuditRepository.Insert(new ImportAudit(importAuditStartTime, rowsImported, ImportType.LarsImport, filePath.Result));
            _logger.LogInformation("LARS Import - finished data transfer from import tables");
        }

        private async Task InsertDataFromZipStreamToImportTables(Stream content)
        {
            _logger.LogInformation("LARS Import - starting extract from ZIP");
            var standardsCsv = _zipArchiveHelper
                .ExtractModelFromCsvFileZipStream<StandardCsv>(content, Constants.LarsStandardsFileName);
            var apprenticeshipFundingCsv = _zipArchiveHelper
                .ExtractModelFromCsvFileZipStream<ApprenticeshipFundingCsv>(content,
                    Constants.LarsApprenticeshipFundingFileName);

            _larsStandardImportRepository.DeleteAll();
            _apprenticeshipFundingImportRepository.DeleteAll();

            var larsImportResult = _larsStandardImportRepository
                .InsertMany(standardsCsv.Select(c => (LarsStandardImport) c).ToList());

            var filterRecords = apprenticeshipFundingCsv
                .Where(c => c.ApprenticeshipType.StartsWith("STD", StringComparison.CurrentCultureIgnoreCase))
                .Select(c => (ApprenticeshipFundingImport) c).ToList();
            
            var apprenticeFundingImportResult =
                _apprenticeshipFundingImportRepository.InsertMany(filterRecords);

            await Task.WhenAll(larsImportResult, apprenticeFundingImportResult);
            
            _logger.LogInformation("LARS Import - finished load into Import tables");
        }
    }
}