using System.Threading.Tasks;
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
    }
}