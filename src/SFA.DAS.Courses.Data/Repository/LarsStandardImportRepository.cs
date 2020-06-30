using System.Collections.Generic;
using System.Threading.Tasks;
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
            
            _coursesDataContext.SaveChanges();
        }
        public void DeleteAll()
        {
            _coursesDataContext.LarsStandardsImport.RemoveRange(_coursesDataContext.LarsStandardsImport);
            _coursesDataContext.SaveChanges();
        }
    }
}