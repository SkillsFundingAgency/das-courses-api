using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public LarsImportService (ILarsPageParser larsPageParser, 
            ILarsDataDownloadService larsDataDownloadService, 
            IImportAuditRepository importAuditRepository,
            IApprenticeshipFundingImportRepository apprenticeshipFundingImportRepository,
            ILarsStandardImportRepository larsStandardImportRepository, 
            IApprenticeshipFundingRepository apprenticeshipFundingRepository,
            ILarsStandardRepository larsStandardRepository,
            IZipArchiveHelper zipArchiveHelper)
        {
            _larsPageParser = larsPageParser;
            _larsDataDownloadService = larsDataDownloadService;
            _importAuditRepository = importAuditRepository;
            _apprenticeshipFundingImportRepository = apprenticeshipFundingImportRepository;
            _larsStandardImportRepository = larsStandardImportRepository;
            _apprenticeshipFundingRepository = apprenticeshipFundingRepository;
            _larsStandardRepository = larsStandardRepository;
            _zipArchiveHelper = zipArchiveHelper;
        }
        public async Task ImportData()
        {
            var lastFilePath = _importAuditRepository.GetLastImportByType(ImportType.LarsImport);
            var filePath = _larsPageParser.GetCurrentLarsDataDownloadFilePath();

            await Task.WhenAll(lastFilePath, filePath);

            if (lastFilePath.Result != null && filePath.Result.Equals(lastFilePath.Result.FileName, StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }
            var importAuditStartTime = DateTime.UtcNow;
            var content = await _larsDataDownloadService.GetFileStream(filePath.Result);

            await InsertDataFromZipStreamToImportTables(content);

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
        }

        private async Task InsertDataFromZipStreamToImportTables(Stream content)
        {
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
        }
    }
}