using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Repository
{
    public class ApprenticeshipFundingImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public ApprenticeshipFundingImportRepository (ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }
        public async Task InsertMany(IEnumerable<ApprenticeshipFundingImport> apprenticeshipFundingImports)
        {
            await _coursesDataContext.ApprenticeshipFundingImport.AddRangeAsync(apprenticeshipFundingImports);
            
            _coursesDataContext.SaveChanges();
        }
        public void DeleteAll()
        {
            _coursesDataContext.ApprenticeshipFundingImport.RemoveRange(_coursesDataContext.ApprenticeshipFundingImport);
            _coursesDataContext.SaveChanges();
        }
    }
}