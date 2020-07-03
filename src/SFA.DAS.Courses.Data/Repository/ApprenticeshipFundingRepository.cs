using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class ApprenticeshipFundingRepository : IApprenticeshipFundingRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public ApprenticeshipFundingRepository (ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task InsertMany(IEnumerable<ApprenticeshipFunding> apprenticeshipFunding)
        {
            await _coursesDataContext.ApprenticeshipFunding.AddRangeAsync(apprenticeshipFunding);
            _coursesDataContext.SaveChanges();
        }

        public void DeleteAll()
        {
            _coursesDataContext.ApprenticeshipFunding.RemoveRange(_coursesDataContext.ApprenticeshipFunding);
            _coursesDataContext.SaveChanges();
        }
    }
}