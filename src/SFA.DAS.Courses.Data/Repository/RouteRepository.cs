using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Repository
{
    public class RouteRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public RouteRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task InsertMany(IEnumerable<Route> routes)
        {
            await _coursesDataContext.Routes.AddRangeAsync(routes);
            _coursesDataContext.SaveChanges();
        }

        public void DeleteAll()
        {
            _coursesDataContext.Routes.RemoveRange(_coursesDataContext.Routes);
            _coursesDataContext.SaveChanges();
        }

        public async Task<IEnumerable<Route>> GetAll()
        {
            return await _coursesDataContext.Routes.ToListAsync();
        }
    }
}