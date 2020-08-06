using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private readonly ILarsDataDownloadService _larsDataDownloadService;
        private readonly IZipArchiveHelper _zipArchiveHelper;
        private readonly IApprenticeshipFundingImportRepository _apprenticeshipFundingImportRepository;
        private readonly ILarsStandardImportRepository _larsStandardImportRepository;
        private readonly ILogger<LarsImportService> _logger;
        private readonly ISectorSubjectAreaTier2ImportRepository _sectorSubjectAreaTier2ImportRepository;
        private readonly IQualificationSectorSubjectAreaService _qualificationSectorSubjectAreaService;
        private Dictionary<decimal, string> _qualificationData;

        public LarsImportStaging (
            ILarsDataDownloadService larsDataDownloadService,
            IZipArchiveHelper zipArchiveHelper,
            IApprenticeshipFundingImportRepository apprenticeshipFundingImportRepository,
            ILarsStandardImportRepository larsStandardImportRepository,
            ISectorSubjectAreaTier2ImportRepository sectorSubjectAreaTier2ImportRepository,
            IQualificationSectorSubjectAreaService qualificationSectorSubjectAreaService,
            ILogger<LarsImportService> logger)
        {
            _larsDataDownloadService = larsDataDownloadService;
            _zipArchiveHelper = zipArchiveHelper;
            _apprenticeshipFundingImportRepository = apprenticeshipFundingImportRepository;
            _larsStandardImportRepository = larsStandardImportRepository;
            _sectorSubjectAreaTier2ImportRepository = sectorSubjectAreaTier2ImportRepository;
            _qualificationSectorSubjectAreaService = qualificationSectorSubjectAreaService;
            _logger = logger;
        }

        public async Task Import(string filePath)
        {
            _logger.LogInformation($"LARS import - starting import of {filePath}");

            _qualificationData = await LoadQualificationSectorSubjectAreaData();
            
            var content = await _larsDataDownloadService.GetFileStream(filePath);

            await InsertDataFromZipStreamToImportTables(content);
        }

        private async Task<Dictionary<decimal,string>> LoadQualificationSectorSubjectAreaData()
        {
            var entries = await _qualificationSectorSubjectAreaService.GetEntries();
            
            var dictionary = new ConcurrentDictionary<decimal, string>();

            await Task.WhenAll(entries.Select(entry => GetEntry(entry, dictionary)).ToArray());

            return dictionary.ToDictionary(k=>k.Key, v=>v.Value);
        }

        private async Task GetEntry(QualificationItemList entry, ConcurrentDictionary<decimal, string> dictionary)
        {
            var result = await _qualificationSectorSubjectAreaService.GetEntry(entry.ItemHash.FirstOrDefault());
            decimal.TryParse(result.QualificationSectorSubjectArea, out var decimalResult);
            dictionary.TryAdd(decimalResult, result.Name);
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

            ClearStagingTables();

            var larsImportResult = _larsStandardImportRepository
                .InsertMany(standardsCsv.Select(c => (LarsStandardImport) c).ToList());

            var filterRecords = apprenticeshipFundingCsv
                .Where(c => c.ApprenticeshipType.StartsWith("STD", StringComparison.CurrentCultureIgnoreCase))
                .Select(c => (ApprenticeshipFundingImport) c).ToList();
            
            var apprenticeFundingImportResult =
                _apprenticeshipFundingImportRepository.InsertMany(filterRecords);

            var sectorSubjectAreaTier2ImportResult =
                _sectorSubjectAreaTier2ImportRepository.InsertMany(sectorSubjectAreaTier2Csv
                    .Select(x => new SectorSubjectAreaTier2Import().Map(x, 
                        _qualificationData.ContainsKey(x.SectorSubjectAreaTier2) 
                            ? _qualificationData[x.SectorSubjectAreaTier2] 
                            : "")).ToList());

            await Task.WhenAll(larsImportResult, apprenticeFundingImportResult, sectorSubjectAreaTier2ImportResult);
            
            _logger.LogInformation("LARS Import - finished load into Import tables");
        }

        private void ClearStagingTables()
        {
            _larsStandardImportRepository.DeleteAll();
            _apprenticeshipFundingImportRepository.DeleteAll();
            _sectorSubjectAreaTier2ImportRepository.DeleteAll();
        }
    }
}