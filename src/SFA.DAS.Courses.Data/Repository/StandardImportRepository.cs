using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Repository
{
    public class StandardImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public StandardImportRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task InsertMany(IEnumerable<StandardImport> standardsImport)
        {
            await _coursesDataContext.StandardsImport.AddRangeAsync(standardsImport);

            _coursesDataContext.SaveChanges();
        }

        public void DeleteAll()
        {
            _coursesDataContext.StandardsImport.RemoveRange(_coursesDataContext.StandardsImport);
            _coursesDataContext.SaveChanges();
        }

        public async Task<IEnumerable<StandardImport>> GetAll()
        {
            var standardImportItems = await _coursesDataContext.StandardsImport.ToListAsync();

            return standardImportItems;
        }
    }
}