using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class StandardRepository : IStandardRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public StandardRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task<IEnumerable<Standard>> GetAll()
        {
            var result = await _coursesDataContext.Standards
                .Include(c=>c.Sector)
                .Include(c=>c.ApprenticeshipFunding)
                .Include(c=>c.LarsStandard)
                .OrderBy(c=>c.Title)
                .ToListAsync();
            
            return result;
        }

        public async Task<int> Count()
        {
            return await _coursesDataContext.Standards.CountAsync();
        }

        public void DeleteAll()
        {
            _coursesDataContext.Standards.RemoveRange(_coursesDataContext.Standards);

            _coursesDataContext.SaveChanges();
        }

        public async Task InsertMany(IEnumerable<Standard> standards)
        {
            await _coursesDataContext.Standards.AddRangeAsync(standards);

            _coursesDataContext.SaveChanges();
        }

        public async Task<Standard> Get(int id)
        {
            var standard = await _coursesDataContext
                .Standards
                .Include(c=>c.Sector)
                .Include(c=>c.ApprenticeshipFunding)
                .Include(c=>c.LarsStandard)
                .SingleOrDefaultAsync(c=>c.Id.Equals(id));

            if (standard == null)
            {
                throw new InvalidOperationException($"Course with id {id} not found in repository");
            }
            
            return standard;
        }

        public async Task<IEnumerable<Standard>> GetFilteredStandards(List<Guid> routeIds)
        {
            var standards = await _coursesDataContext
                .Standards
                .Where(c => routeIds.Contains(c.RouteId))
                .Include(c => c.Sector)
                .Include(c=>c.ApprenticeshipFunding)
                .Include(c=>c.LarsStandard)
                .OrderBy(c=>c.Title)
                .ToListAsync();

            return standards;
        }
    }
}
