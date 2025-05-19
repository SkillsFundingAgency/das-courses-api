using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class SectorSubjectAreaTier1ImportRepository : ISectorSubjectAreaTier1ImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;
        public SectorSubjectAreaTier1ImportRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task<IEnumerable<SectorSubjectAreaTier1Import>> GetAll()
        {
            var items = await _coursesDataContext.SectorSubjectAreaTier1Import.ToListAsync();
            return items;
        }

        public async Task DeleteAll()
        {
            _coursesDataContext.SectorSubjectAreaTier1Import.RemoveRange(_coursesDataContext.SectorSubjectAreaTier1Import);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task InsertMany(IEnumerable<SectorSubjectAreaTier1Import> sectorSubjectAreaTier1Imports)
        {
            await _coursesDataContext.SectorSubjectAreaTier1Import.AddRangeAsync(sectorSubjectAreaTier1Imports);
            await _coursesDataContext.SaveChangesAsync();
        }
    }
}
