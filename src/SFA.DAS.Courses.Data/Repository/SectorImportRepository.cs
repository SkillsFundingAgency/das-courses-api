using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Repository
{
    public class SectorImportRepository 
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public SectorImportRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task InsertMany(IEnumerable<SectorImport> sector)
        {
            await _coursesDataContext.SectorsImport.AddRangeAsync(sector);
            
            _coursesDataContext.SaveChanges();
        }

        public async Task<IEnumerable<SectorImport>> GetAll()
        {
            var sectors = await _coursesDataContext.SectorsImport.ToListAsync();

            return sectors;
        }
    }
}