using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Services
{
    public class LarsDataImportService : ILarsDataImportService
    {
        private readonly ILarsPageParser _larsPageParser;
        private readonly IDataDownloadService _dataDownloadService;
        private readonly IZipArchiveHelper _zipArchiveHelper;
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
        private readonly ICoursesDataContext _coursesDataContext;
        private readonly CoursesConfiguration _coursesConfiguration;
        private readonly ILogger<LarsDataImportService> _logger;

        public LarsDataImportService(
            ILarsPageParser larsPageParser,
            IDataDownloadService dataDownloadService,
            IZipArchiveHelper zipArchiveHelper,
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
            ICoursesDataContext coursesDataContext,
            IOptions<CoursesConfiguration> config,
            ILogger<LarsDataImportService> logger)
        {
            _larsPageParser = larsPageParser;
            _dataDownloadService = dataDownloadService;
            _zipArchiveHelper = zipArchiveHelper;
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
            _coursesDataContext = coursesDataContext;
            _coursesConfiguration = config.Value;
            _logger = logger;
        }

        public async Task<(bool Success, string FileName)> ImportDataIntoStaging()
        {
            try
            {
                _logger.LogInformation("LARS Import - data into staging - started");

                var filePath = await _larsPageParser.GetCurrentLarsDataDownloadFilePath();

                await ImportIntoStaging(filePath);

                _logger.LogInformation("LARS Import - data into staging - finished");

                return (true, filePath);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "LARS Import - an error occurred while trying to import data from LARS file.");
                throw;
            }
        }

        public async Task LoadDataFromStaging(DateTime importAuditStartTime, string filePath)
        {
            try
            {
                await _coursesDataContext.ExecuteInTransactionAsync(async () =>
                {
                    _logger.LogInformation("LARS Import - data load from staging - started");

                    var larsStandardImports = await _larsStandardImportRepository.GetAll();
                    var sectorSubjectAreaTier2Imports = await _sectorSubjectAreaTier2ImportRepository.GetAll();
                    var sectorSubjectAreaTier1Imports = await _sectorSubjectAreaTier1ImportRepository.GetAll();

                    await _larsStandardRepository.DeleteAll();
                    await _sectorSubjectAreaTier2Repository.DeleteAll();
                    await _sectorSubjectAreaTier1Repository.DeleteAll();

                    await _larsStandardRepository.InsertMany(
                        larsStandardImports.Select(c => (LarsStandard)c).ToList());

                    var apprenticeshipFundingRowsImported = await LoadApprenticeshipFunding();

                    await _sectorSubjectAreaTier2Repository.InsertMany(
                        sectorSubjectAreaTier2Imports.Select(c => (SectorSubjectAreaTier2)c).ToList());

                    await _sectorSubjectAreaTier1Repository.InsertMany(
                        sectorSubjectAreaTier1Imports.Select(c => (SectorSubjectAreaTier1)c).ToList());

                    var rowsImported = larsStandardImports.Count() +
                                       apprenticeshipFundingRowsImported +
                                       sectorSubjectAreaTier2Imports.Count();

                    await _importAuditRepository.Insert(new ImportAudit(
                        importAuditStartTime,
                        rowsImported,
                        ImportType.LarsImport,
                        filePath));
                });

                _logger.LogInformation("LARS Import - data load from staging - finished");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "LARS Import - an error occurred while trying to load data from staging tables.");
                throw;
            }
        }

        private async Task ImportIntoStaging(string filePath)
        {
            _logger.LogInformation("LARS import - starting import of {FilePath}", filePath);

            await using var content = await _dataDownloadService.GetFileStream(filePath);

            await InsertDataFromZipStreamToImportTables(content);
        }

        private async Task InsertDataFromZipStreamToImportTables(Stream content)
        {
            _logger.LogInformation("LARS Import - starting extract from ZIP");

            var standardsCsv = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<StandardCsv>(content, Constants.LarsStandardsFileName);
            var apprenticeshipFundingCsv = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<ApprenticeshipFundingCsv>(content, Constants.LarsApprenticeshipFundingFileName);
            var fundingCsv = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<FundingCsv>(content, Constants.LarsFundingFileName);
            var sectorSubjectAreaTier2Csv = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier2Csv>(content, Constants.LarsSectorSubjectAreaTier2FileName);
            var sectorSubjectAreaTier1Csv = _zipArchiveHelper.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier1Csv>(content, Constants.LarsSectorSubjectAreaTier1FileName);

            await _coursesDataContext.ExecuteInTransactionAsync(async () =>
            {
                await _larsStandardImportRepository.DeleteAll();
                await _apprenticeshipFundingImportRepository.DeleteAll();
                await _fundingImportRepository.DeleteAll();
                await _sectorSubjectAreaTier2ImportRepository.DeleteAll();
                await _sectorSubjectAreaTier1ImportRepository.DeleteAll();

                await _larsStandardImportRepository.InsertMany(
                    standardsCsv.Select(c => (LarsStandardImport)c).ToList());

                var filteredApprenticeshipFundingCsv = apprenticeshipFundingCsv
                    .Where(c => c.ApprenticeshipType?.StartsWith("STD", StringComparison.CurrentCultureIgnoreCase) ?? false)
                    .Select(c => (ApprenticeshipFundingImport)c)
                    .ToList();

                await _apprenticeshipFundingImportRepository.InsertMany(filteredApprenticeshipFundingCsv);

                var filteredFundingCsv = fundingCsv
                    .Where(c => c.FundingCategory?.Equals(
                        _coursesConfiguration.LarsImportConfiguration.LarsFundingCategory,
                        StringComparison.CurrentCultureIgnoreCase) ?? false)
                    .Select(x => (FundingImport)x)
                    .ToList();

                await _fundingImportRepository.InsertMany(filteredFundingCsv);

                await _sectorSubjectAreaTier2ImportRepository.InsertMany(
                    sectorSubjectAreaTier2Csv.Select(x => (SectorSubjectAreaTier2Import)x).ToList());

                var filteredSectorSubjectAreaTier1Csv = sectorSubjectAreaTier1Csv
                    .Where(s => !s.SectorSubjectAreaTier1?.Contains('-') ?? false)
                    .Select(x => (SectorSubjectAreaTier1Import)x)
                    .ToList();

                await _sectorSubjectAreaTier1ImportRepository.InsertMany(filteredSectorSubjectAreaTier1Csv);
            });

            _logger.LogInformation("LARS Import - finished load into Import tables");
        }

        private async Task<int> LoadApprenticeshipFunding()
        {
            var fundingImports = await _fundingImportRepository.GetAll();
            var apprenticeshipFundingImports = await _apprenticeshipFundingImportRepository.GetAll();

            await _apprenticeshipFundingRepository.DeleteAll();

            var fundings = fundingImports
                .Select(c => (ApprenticeshipFunding)c)
                .ToList();

            await _apprenticeshipFundingRepository.InsertMany(fundings);

            var apprenticeshipFundings = apprenticeshipFundingImports
                .Select(c => (ApprenticeshipFunding)c)
                .ToList();

            await _apprenticeshipFundingRepository.InsertMany(apprenticeshipFundings);

            return fundings.Count + apprenticeshipFundings.Count;
        }
    }
}
