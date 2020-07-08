using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Repository
{
    public class FrameworkRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public FrameworkRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task InsertMany(IEnumerable<Framework> frameworks)
        {
            await _coursesDataContext.Frameworks.AddRangeAsync(frameworks);
            _coursesDataContext.SaveChanges();
        }

        public void DeleteAll()
        {
            _coursesDataContext.Frameworks.RemoveRange(_coursesDataContext.Frameworks);
            _coursesDataContext.SaveChanges();
        }

        public async Task<Framework> Get(string id)
        {
            var framework = await _coursesDataContext
                .Frameworks
                .Include(c=>c.FundingPeriods)
                .SingleOrDefaultAsync(c=>c.Id.Equals(id));

            if (framework == null)
            {
                throw new InvalidOperationException($"Framework with id {id} not found in repository");
            }

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