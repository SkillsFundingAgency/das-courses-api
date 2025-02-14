using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class RouteImportRepository : IRouteImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public RouteImportRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task<int> InsertMany(IEnumerable<RouteImport> routes)
        {
            if (!routes.Any())
                return 0;

            await _coursesDataContext.RoutesImport.AddRangeAsync(routes);
            return await _coursesDataContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAll()
        {
            _coursesDataContext.RoutesImport.RemoveRange(_coursesDataContext.RoutesImport);
            return await _coursesDataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RouteImport>> GetAll()
        {
            return await _coursesDataContext.RoutesImport.ToListAsync();
        }
    }
}
