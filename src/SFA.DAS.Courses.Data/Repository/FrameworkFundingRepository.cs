using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class FrameworkFundingRepository : IFrameworkFundingRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public FrameworkFundingRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }
        public async Task InsertMany(IEnumerable<FrameworkFunding> frameworkFunding)
        {
            await _coursesDataContext.FrameworkFunding.AddRangeAsync(frameworkFunding);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task DeleteAll()
        {
            _coursesDataContext.FrameworkFunding.RemoveRange(_coursesDataContext.FrameworkFunding);
            await _coursesDataContext.SaveChangesAsync();
        }
    }
}
