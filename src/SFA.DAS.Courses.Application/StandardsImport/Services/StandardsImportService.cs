using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.StandardsImport.Services
{
    public class StandardsImportService
    {
        private readonly IInstituteOfApprenticeshipService _instituteOfApprenticeshipService;
        private readonly IStandardImportRepository _standardImportRepository;
        private readonly IStandardRepository _standardRepository;

        public StandardsImportService (IInstituteOfApprenticeshipService instituteOfApprenticeshipService, IStandardImportRepository standardImportRepository, IStandardRepository standardRepository)
        {
            _instituteOfApprenticeshipService = instituteOfApprenticeshipService;
            _standardImportRepository = standardImportRepository;
            _standardRepository = standardRepository;
        }
        public async Task ImportStandards()
        {
            await GetStandardsFromApiAndInsertIntoStagingTable();

            var standardsToInsert = (await _standardImportRepository.GetAll()).ToList();

            if (!standardsToInsert.Any())
            {
                return;
            }
            
            _standardRepository.DeleteAll();
            await _standardRepository.InsertMany(standardsToInsert);
        }

        private async Task GetStandardsFromApiAndInsertIntoStagingTable()
        {
            var standards = await _instituteOfApprenticeshipService.GetStandards();
            _standardImportRepository.DeleteAll();
            await _standardImportRepository.InsertMany(standards
                .Where(c => c.LarsCode > 0
                            && c.Status.Equals("Approved for Delivery", StringComparison.CurrentCultureIgnoreCase))
                .Select(c => (StandardImport) c)
                .ToList());
        }
    }
}