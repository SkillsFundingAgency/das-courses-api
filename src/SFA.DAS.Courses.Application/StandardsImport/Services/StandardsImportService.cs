using System;
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
        private readonly ILogger<StandardsImportService> _logger;

        public StandardsImportService (IInstituteOfApprenticeshipService instituteOfApprenticeshipService, 
                                        IStandardImportRepository standardImportRepository, 
                                        IStandardRepository standardRepository,
                                        ILogger<StandardsImportService> logger)
        {
            _instituteOfApprenticeshipService = instituteOfApprenticeshipService;
            _standardImportRepository = standardImportRepository;
            _standardRepository = standardRepository;
            _logger = logger;
        }
        public async Task ImportStandards()
        {
            try
            {
                await GetStandardsFromApiAndInsertIntoStagingTable();

                var standardsToInsert = (await _standardImportRepository.GetAll()).ToList();

                if (!standardsToInsert.Any())
                {
                    _logger.LogWarning("No standards loaded. No standards retrieved from API");
                    return;
                }
            
                _standardRepository.DeleteAll();
                
                _logger.LogInformation($"Adding {standardsToInsert.Count} to Standards table.");
                
                await _standardRepository.InsertMany(standardsToInsert.Select(c=>(Standard)c).ToList());
            }
            catch (Exception e)
            {
                _logger.LogError("Unable to update Standards data",e);
                throw;
            }
            
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