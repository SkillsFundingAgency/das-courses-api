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
    internal class LarsImportStaging
    {
        private readonly IDataDownloadService _dataDownloadService;
        private readonly IZipArchiveHelper _zipArchiveHelper;
        private readonly IApprenticeshipFundingImportRepository _apprenticeshipFundingImportRepository;
        private readonly ILarsStandardImportRepository _larsStandardImportRepository;
        private readonly ILogger<LarsImportService> _logger;
        private readonly ISectorSubjectAreaTier2ImportRepository _sectorSubjectAreaTier2ImportRepository;
        private readonly ISectorSubjectAreaTier1ImportRepository _sectorSubjectAreaTier1ImportRepository;

        public LarsImportStaging(
            IDataDownloadService dataDownloadService,
            IZipArchiveHelper zipArchiveHelper,
            IApprenticeshipFundingImportRepository apprenticeshipFundingImportRepository,
            ILarsStandardImportRepository larsStandardImportRepository,
            ISectorSubjectAreaTier2ImportRepository sectorSubjectAreaTier2ImportRepository,
            ISectorSubjectAreaTier1ImportRepository sectorSubjectAreaTier1ImportRepository,
            ILogger<LarsImportService> logger)
        {
            _dataDownloadService = dataDownloadService;
            _zipArchiveHelper = zipArchiveHelper;
            _apprenticeshipFundingImportRepository = apprenticeshipFundingImportRepository;
            _larsStandardImportRepository = larsStandardImportRepository;
            _sectorSubjectAreaTier2ImportRepository = sectorSubjectAreaTier2ImportRepository;
            _sectorSubjectAreaTier1ImportRepository = sectorSubjectAreaTier1ImportRepository;
            _logger = logger;
        }

        public async Task Import(string filePath)
        {
            _logger.LogInformation($"LARS import - starting import of {filePath}");

            var content = await _dataDownloadService.GetFileStream(filePath);

            await InsertDataFromZipStreamToImportTables(content);
        }

        private async Task InsertDataFromZipStreamToImportTables(Stream content)
        {
            _logger.LogInformation("LARS Import - starting extract from ZIP");
            var standardsCsv = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<StandardCsv>(content, Constants.LarsStandardsFileName);
            var apprenticeshipFundingCsv = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<ApprenticeshipFundingCsv>(content, Constants.LarsApprenticeshipFundingFileName);
            var sectorSubjectAreaTier2Csv = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier2Csv>(content, Constants.LarsSectorSubjectAreaTier2FileName);
            var sectorSubjectAreaTier1Csv = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier1Csv>(content, Constants.LarsSectorSubjectAreaTier1FileName);

            await ClearStagingTables();

            var larsImportTask = _larsStandardImportRepository
                .InsertMany(standardsCsv.Select(c => (LarsStandardImport)c).ToList());

            var filterRecords = apprenticeshipFundingCsv
                .Where(c => c.ApprenticeshipType.StartsWith("STD", StringComparison.CurrentCultureIgnoreCase))
                .Select(c => (ApprenticeshipFundingImport)c).ToList();

            var apprenticeFundingImportTask =
                _apprenticeshipFundingImportRepository.InsertMany(filterRecords);

            var sectorSubjectAreaTier2ImportTask =
                _sectorSubjectAreaTier2ImportRepository.InsertMany(sectorSubjectAreaTier2Csv
                    .Select(x => (SectorSubjectAreaTier2Import)x).ToList());

            var sectorSubjectAreaTier1ImportTask =
                _sectorSubjectAreaTier1ImportRepository.InsertMany(sectorSubjectAreaTier1Csv
                    .Where(s => !s.SectorSubjectAreaTier1.Contains('-'))
                    .Select(x => (SectorSubjectAreaTier1Import)x).ToList());

            await Task.WhenAll(larsImportTask, apprenticeFundingImportTask, sectorSubjectAreaTier2ImportTask, sectorSubjectAreaTier1ImportTask);

            _logger.LogInformation("LARS Import - finished load into Import tables");
        }

        private async Task ClearStagingTables()
        {
            await _larsStandardImportRepository.DeleteAll();
            await _apprenticeshipFundingImportRepository.DeleteAll();
            await _sectorSubjectAreaTier2ImportRepository.DeleteAll();
            await _sectorSubjectAreaTier1ImportRepository.DeleteAll();
        }
    }
}
