using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.StandardsImport.Services
{
    public class StandardsImportService : IStandardsImportService
    {
        private readonly IInstituteOfApprenticeshipService _instituteOfApprenticeshipService;
        private readonly IStandardImportRepository _standardImportRepository;
        private readonly IStandardRepository _standardRepository;
        private readonly IImportAuditRepository _auditRepository;
        private readonly ILogger<StandardsImportService> _logger;

        public StandardsImportService (IInstituteOfApprenticeshipService instituteOfApprenticeshipService, 
                                        IStandardImportRepository standardImportRepository, 
                                        IStandardRepository standardRepository,
                                        IImportAuditRepository auditRepository,
                                        ILogger<StandardsImportService> logger)
        {
            _instituteOfApprenticeshipService = instituteOfApprenticeshipService;
            _standardImportRepository = standardImportRepository;
            _standardRepository = standardRepository;
            _auditRepository = auditRepository;
            _logger = logger;
        }
        public async Task ImportStandards()
        {
            try
            {
                var timeStarted = DateTime.UtcNow;
                await GetStandardsFromApiAndInsertIntoStagingTable();

                var standardsToInsert = (await _standardImportRepository.GetAll()).ToList();

                if (!standardsToInsert.Any())
                {
                    await AuditImport(timeStarted, 0);
                    _logger.LogWarning("No standards loaded. No standards retrieved from API");
                    return;
                }
            
                _standardRepository.DeleteAll();
                
                _logger.LogInformation($"Adding {standardsToInsert.Count} to Standards table.");

                var standards = standardsToInsert.Select(c=>(Standard)c).ToList();
                await _standardRepository.InsertMany(standards);
                
                await AuditImport(timeStarted, standards.Count);
            }
            catch (Exception e)
            {
                _logger.LogError("Unable to update Standards data",e);
                throw;
            }
            
        }

        private async Task AuditImport(DateTime timeStarted, int rowsImported)
        {
            var auditRecord = new ImportAudit(timeStarted, rowsImported);
            await _auditRepository.Insert(auditRecord);
        }

        private async Task GetStandardsFromApiAndInsertIntoStagingTable()
        {
            var standards = (await _instituteOfApprenticeshipService.GetStandards()).ToList();
            _logger.LogInformation($"Retrieved {standards.Count} standards from API");
            _standardImportRepository.DeleteAll();
            await _standardImportRepository.InsertMany(standards
                .Where(c => c.LarsCode > 0
                            && c.Status.Equals("Approved for Delivery", StringComparison.CurrentCultureIgnoreCase))
                .Select(c => (StandardImport) c)
                .ToList());
        }
    }
}