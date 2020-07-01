using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class SectorRepository : ISectorRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public SectorRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task InsertMany(IEnumerable<Sector> sector)
        {
            await _coursesDataContext.Sectors.AddRangeAsync(sector);
            
            _coursesDataContext.SaveChanges();
        }

        public async Task<IEnumerable<Sector>> GetAll()
        {
            var sectors = await _coursesDataContext.Sectors.ToListAsync();

            return sectors.OrderBy(c=>c.Route);
        }

        public void DeleteAll()
        {
            _coursesDataContext.Sectors.RemoveRange(_coursesDataContext.Sectors);
            _coursesDataContext.SaveChanges();
        }
    }
}