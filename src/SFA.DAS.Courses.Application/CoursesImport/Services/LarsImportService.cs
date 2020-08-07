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
            IZipArchiveHelper zipArchiveHelper,
            IQualificationSectorSubjectAreaService qualificationSectorSubjectAreaService,
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
            _logger = logger;
            _larsImportStaging = new LarsImportStaging(
                dataDownloadService,
                zipArchiveHelper,
                _apprenticeshipFundingImportRepository,
                _larsStandardImportRepository,
                _sectorSubjectAreaTier2ImportRepository,
                qualificationSectorSubjectAreaService,
                _logger);
        }
        public async Task ImportData()
        {
            try
            {
                var importAuditStartTime = DateTime.UtcNow;
                
                _logger.LogInformation("LARS Import - starting data transfer from import tables");
                _larsStandardRepository.DeleteAll();
                _apprenticeshipFundingRepository.DeleteAll();
                _sectorSubjectAreaTier2Repository.DeleteAll();
                
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

                await _larsImportStaging.Import(filePath.Result);

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

        
    }
}
