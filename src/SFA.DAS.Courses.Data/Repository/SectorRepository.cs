using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Repository
{
    public class SectorRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public SectorRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task Insert(Sector sector)
        {
            await _coursesDataContext.Sectors.AddAsync(sector);
            
            _coursesDataContext.SaveChanges();
        }

        public async Task<IEnumerable<Sector>> GetAll()
        {
            var sectors = await _coursesDataContext.Sectors.ToListAsync();

            return sectors;
        }
    }
}