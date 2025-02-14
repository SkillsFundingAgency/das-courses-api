using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class LarsStandardImportRepository : ILarsStandardImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public LarsStandardImportRepository (ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }
        public async Task InsertMany(IEnumerable<LarsStandardImport> larsStandardImports)
        {
            await _coursesDataContext.LarsStandardsImport.AddRangeAsync(larsStandardImports);
            
            await _coursesDataContext.SaveChangesAsync();
        }
        public async Task DeleteAll()
        {
            _coursesDataContext.LarsStandardsImport.RemoveRange(_coursesDataContext.LarsStandardsImport);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<LarsStandardImport>> GetAll()
        {
            var results = await _coursesDataContext.LarsStandardsImport.ToListAsync();
            return results;
        }
    }
}
