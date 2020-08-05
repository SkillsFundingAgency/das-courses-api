using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Repository
{
    public class SectorSubjectAreaTier2ImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;
        public SectorSubjectAreaTier2ImportRepository (ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task<IEnumerable<SectorSubjectAreaTier2Import>> GetAll()
        {
            var items = await _coursesDataContext.SectorSubjectAreaTier2Import.ToListAsync();
            return items;
        }

        public void DeleteAll()
        {
            _coursesDataContext.SectorSubjectAreaTier2Import.RemoveRange(_coursesDataContext.SectorSubjectAreaTier2Import);
            _coursesDataContext.SaveChanges();
        }

        public async Task InsertMany(IEnumerable<SectorSubjectAreaTier2Import> sectorSubjectAreaTier2Imports)
        {
            await _coursesDataContext.SectorSubjectAreaTier2Import.AddRangeAsync(sectorSubjectAreaTier2Imports);
            _coursesDataContext.SaveChanges();
        }
    }
}