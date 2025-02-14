using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class RouteRepository : IRouteRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public RouteRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task<int> InsertMany(IEnumerable<Route> routes)
        {
            await _coursesDataContext.Routes.AddRangeAsync(routes);
            return await _coursesDataContext.SaveChangesAsync();
        }

        public async Task DeleteAll()
        {
            _coursesDataContext.Routes.RemoveRange(_coursesDataContext.Routes);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Route>> GetAll()
        {
            return await _coursesDataContext.Routes.ToListAsync();
        }
    }
}
