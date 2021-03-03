using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Repository
{
    public class RouteImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public RouteImportRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task InsertMany(IEnumerable<RouteImport> routes)
        {
            await _coursesDataContext.RoutesImport.AddRangeAsync(routes);
            _coursesDataContext.SaveChanges();
        }

        public void DeleteAll()
        {
            _coursesDataContext.RoutesImport.RemoveRange(_coursesDataContext.RoutesImport);
            _coursesDataContext.SaveChanges();
        }

        public async Task<IEnumerable<RouteImport>> GetAll()
        {
            return await _coursesDataContext.RoutesImport.ToListAsync();
        }
    }
}