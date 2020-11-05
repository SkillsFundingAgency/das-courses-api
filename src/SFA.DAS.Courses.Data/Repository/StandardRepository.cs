using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Data.Extensions;
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
            var result = await _coursesDataContext
                .Standards
                .FilterAvailableToStart()
                .Include(c=>c.Sector)
                .Include(c=>c.ApprenticeshipFunding)
                .Include(c=>c.LarsStandard)
                .ThenInclude(c=>c.SectorSubjectArea)
                .ToListAsync();
            
            return result;
        }

        public async Task<int> Count()
        {
            return await _coursesDataContext.Standards.FilterAvailableToStart().CountAsync();
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
                .ThenInclude(c=>c.SectorSubjectArea)
                .SingleOrDefaultAsync(c=>c.Id.Equals(id));

            if (standard == null)
            {
                throw new InvalidOperationException($"Course with id {id} not found in repository");
            }
            
            return standard;
        }

        public async Task<IEnumerable<Standard>> GetFilteredStandards(IList<Guid> routeIds, IList<int> levels)
        {
            var standards = _coursesDataContext.Standards.AsQueryable();

            if (routeIds.Count > 0)
            {
                standards = standards.Where(standard => routeIds.Contains(standard.RouteId));
            }
            if (levels.Count > 0)
            {
                standards = standards.Where(standard => levels.Contains(standard.Level));
            }

            standards = standards
                .FilterAvailableToStart()
                .Include(c => c.Sector)
                .Include(c => c.ApprenticeshipFunding)
                .Include(c => c.LarsStandard)
                .ThenInclude(c=>c.SectorSubjectArea);
                

            return await standards.ToListAsync();
        }
    }
}
