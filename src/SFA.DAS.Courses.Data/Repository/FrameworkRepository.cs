using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class FrameworkRepository : IFrameworkRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public FrameworkRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task InsertMany(IEnumerable<Framework> frameworks)
        {
            await _coursesDataContext.Frameworks.AddRangeAsync(frameworks);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task DeleteAll()
        {
            _coursesDataContext.Frameworks.RemoveRange(_coursesDataContext.Frameworks);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task<Framework> Get(string id)
        {
            var framework = await _coursesDataContext
                .Frameworks
                .Include(c=>c.FundingPeriods)
                .SingleOrDefaultAsync(c=>c.Id.Equals(id));
            return framework;
        }

        public async Task<IEnumerable<Framework>> GetAll()
        {
            var frameworks = await _coursesDataContext
                .Frameworks
                .Include(c => c.FundingPeriods)
                .ToListAsync();

            return frameworks;
        }
    }
}
