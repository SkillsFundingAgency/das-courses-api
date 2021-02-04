using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Data.Extensions;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Data.Repository
{
    public class StandardRepository : IStandardRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public StandardRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task<int> Count(StandardFilter filter)
        {
            return await _coursesDataContext.Standards.AsQueryable().FilterStandards(filter).CountAsync();
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

        public async Task<Standard> Get(int larsCode)
        {
            // To maintain backwards compatibility, when retrieving a standard by Lars Code
            // It is assumed you always want the latest active version of that standard
            var standards = await _coursesDataContext
                .Standards
                .FilterStandards(StandardFilter.Active)
                .Include(c => c.Sector)
                .Include(c => c.ApprenticeshipFunding)
                .Include(c => c.LarsStandard)
                .ThenInclude(c => c.SectorSubjectArea)
                .Where(c => c.LarsCode.Equals(larsCode))
                .ToListAsync();

            // In Memory Filter for get latest version due to limitations in EF query translation
            // into expression tree
            var standard = standards.InMemoryFilterIsLatestVersion(StandardFilter.Active).SingleOrDefault();

            if (standard == null)
            {
                throw new InvalidOperationException($"Course with id {larsCode} not found in repository");
            }

            return standard;
        }

        public async Task<Standard> Get(string standardUId)
        {
            var standard = await _coursesDataContext
                .Standards
                .Include(c => c.Sector)
                .Include(c => c.ApprenticeshipFunding)
                .Include(c => c.LarsStandard)
                .ThenInclude(c => c.SectorSubjectArea)
                .SingleOrDefaultAsync(c => c.StandardUId.Equals(standardUId));

            if (standard == null)
            {
                throw new InvalidOperationException($"Course with standardUId {standardUId} not found in repository");
            }

            return standard;
        }

        public async Task<IEnumerable<Standard>> GetStandards(IList<Guid> routeIds, IList<int> levels, StandardFilter filter)
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
                .FilterStandards(filter)
                .Include(c => c.Sector)
                .Include(c => c.ApprenticeshipFunding)
                .Include(c => c.LarsStandard)
                .ThenInclude(c => c.SectorSubjectArea);

            var standardResults = await standards.ToListAsync();

            // Secondary filter performed in memory due to limitations in 
            // EF Core query translation on Group By selecting top row of each.
            return standardResults.InMemoryFilterIsLatestVersion(filter);
        }
    }
}
