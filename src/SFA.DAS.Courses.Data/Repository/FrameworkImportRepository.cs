using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class FrameworkImportRepository : IFrameworkImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public FrameworkImportRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task InsertMany(IEnumerable<FrameworkImport> frameworks)
        {
            await _coursesDataContext.FrameworksImport.AddRangeAsync(frameworks);
            _coursesDataContext.SaveChanges();
        }

        public void DeleteAll()
        {
            _coursesDataContext.FrameworksImport.RemoveRange(_coursesDataContext.FrameworksImport);
            _coursesDataContext.SaveChanges();
        }

        public async Task<IEnumerable<FrameworkImport>> GetAll()
        {
            var frameworkImports = await _coursesDataContext.FrameworksImport.ToListAsync();
            return frameworkImports;
        }
    }
}