using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class ApprenticeshipFundingImportRepository : IApprenticeshipFundingImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public ApprenticeshipFundingImportRepository (ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }
        public async Task InsertMany(IEnumerable<ApprenticeshipFundingImport> apprenticeshipFundingImports)
        {
            await _coursesDataContext.ApprenticeshipFundingImport.AddRangeAsync(apprenticeshipFundingImports);
            
            await _coursesDataContext.SaveChangesAsync();
        }
        public async Task DeleteAll()
        {
            _coursesDataContext.ApprenticeshipFundingImport.RemoveRange(_coursesDataContext.ApprenticeshipFundingImport);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApprenticeshipFundingImport>> GetAll()
        {
            var results = await _coursesDataContext.ApprenticeshipFundingImport.ToListAsync();

            return results;
        }
    }
}
