using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using Framework = SFA.DAS.Courses.Domain.ImportTypes.Framework;

namespace SFA.DAS.Courses.Application.CoursesImport.Services
{
    public class FrameworksImportService : IFrameworksImportService
    {
        private readonly IJsonFileHelper _jsonFileHelper;
        private readonly IImportAuditRepository _importAuditRepository;
        private readonly IFrameworkImportRepository _frameworkImportRepository;
        private readonly IFrameworkFundingImportRepository _frameworkFundingImportRepository;
        private readonly IFrameworkRepository _frameworkRepository;
        private readonly IFrameworkFundingRepository _frameworkFundingRepository;
        private readonly ILogger<FrameworksImportService> _logger;
        private int _rowsImported = 0;

        public FrameworksImportService (
            IJsonFileHelper jsonFileHelper, 
            IImportAuditRepository importAuditRepository, 
            IFrameworkImportRepository frameworkImportRepository,
            IFrameworkFundingImportRepository frameworkFundingImportRepository,
            IFrameworkRepository frameworkRepository, 
            IFrameworkFundingRepository frameworkFundingRepository,
            ILogger<FrameworksImportService> logger)
        {
            _jsonFileHelper = jsonFileHelper;
            _importAuditRepository = importAuditRepository;
            _frameworkImportRepository = frameworkImportRepository;
            _frameworkFundingImportRepository = frameworkFundingImportRepository;
            _frameworkRepository = frameworkRepository;
            _frameworkFundingRepository = frameworkFundingRepository;
            _logger = logger;
        }
        public async Task ImportData()
        {
            var latestFile = _jsonFileHelper.GetLatestFrameworkFileFromDataDirectory();

            if (string.IsNullOrEmpty(latestFile))
            {
                _logger.LogWarning("No framework data file found to import");
                return;
            }

            var lastFrameworkImport = await _importAuditRepository.GetLastImportByType(ImportType.FrameworkImport);

            if (latestFile == lastFrameworkImport.FileName)
            {
                _logger.LogInformation($"Framework file {latestFile} has already been imported");
                return;
            }

            var importStartTime = DateTime.UtcNow;
            
            await InsertFrameworksIntoStagingTable(latestFile);

            await InsertFrameworksFromImportTable();
            
            var audit = new ImportAudit(importStartTime,_rowsImported,ImportType.FrameworkImport,latestFile);
            await _importAuditRepository.Insert(audit);
        }

        private async Task InsertFrameworksIntoStagingTable(string latestFile)
        {
            var frameworksFromFile = _jsonFileHelper.ParseJsonFile<Framework>(latestFile).ToList();

            _frameworkImportRepository.DeleteAll();
            _frameworkFundingImportRepository.DeleteAll();

            var frameworkFundingImport = new List<FrameworkFundingImport>();
            foreach (var framework in frameworksFromFile)
            {
                frameworkFundingImport.AddRange(
                    framework.FundingPeriods.Select(frameworkFundingPeriod =>
                        new FrameworkFundingImport().Map(frameworkFundingPeriod, framework.Id)));
            }

            var frameworkImport = frameworksFromFile.Select(c => (FrameworkImport) c).ToList();

            var insertFrameworkImportTask = _frameworkImportRepository.InsertMany(frameworkImport);
            var insertFrameworkFundingImportTask = _frameworkFundingImportRepository.InsertMany(frameworkFundingImport);

            await Task.WhenAll(insertFrameworkImportTask, insertFrameworkFundingImportTask);
        }

        private async Task InsertFrameworksFromImportTable()
        {
            var frameworkImports = _frameworkImportRepository.GetAll();
            var frameworkFundingImports = _frameworkFundingImportRepository.GetAll();

            await Task.WhenAll(frameworkImports, frameworkFundingImports);

            _rowsImported = frameworkImports.Result.Count() + frameworkFundingImports.Result.Count();
            _frameworkRepository.DeleteAll();
            _frameworkFundingRepository.DeleteAll();

            var frameworkInsertTask = _frameworkRepository
                .InsertMany(frameworkImports.Result.Select(c => (Domain.Entities.Framework) c).ToList());
            var frameworkFundingInsertTask = _frameworkFundingRepository
                .InsertMany(frameworkFundingImports.Result.Select(c => (FrameworkFunding) c).ToList());

            await Task.WhenAll(frameworkInsertTask, frameworkFundingInsertTask);
        }
    }
}