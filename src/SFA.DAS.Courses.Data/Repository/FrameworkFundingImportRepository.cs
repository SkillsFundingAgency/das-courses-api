using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Repository
{
    public class FrameworkFundingImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public FrameworkFundingImportRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }
        public async Task InsertMany(IEnumerable<FrameworkFundingImport> frameworkFundingImports)
        {
            await _coursesDataContext.FrameworkFundingImport.AddRangeAsync(frameworkFundingImports);
            _coursesDataContext.SaveChanges();
        }

        public void DeleteAll()
        {
            _coursesDataContext.FrameworkFundingImport.RemoveRange(_coursesDataContext.FrameworkFundingImport);
            _coursesDataContext.SaveChanges();
        }

        public async Task<IEnumerable<FrameworkFundingImport>> GetAll()
        {
            var frameworkFundingImportIems = await _coursesDataContext.FrameworkFundingImport.ToListAsync();
            return frameworkFundingImportIems;
        }
    }
}