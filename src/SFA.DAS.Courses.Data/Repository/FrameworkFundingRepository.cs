using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Repository
{
    public class FrameworkFundingRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public FrameworkFundingRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }
        public async Task InsertMany(IEnumerable<FrameworkFunding> frameworkFunding)
        {
            await _coursesDataContext.FrameworkFunding.AddRangeAsync(frameworkFunding);
            _coursesDataContext.SaveChanges();
        }

        public void DeleteAll()
        {
            _coursesDataContext.FrameworkFunding.RemoveRange(_coursesDataContext.FrameworkFunding);
            _coursesDataContext.SaveChanges();
        }
    }
}