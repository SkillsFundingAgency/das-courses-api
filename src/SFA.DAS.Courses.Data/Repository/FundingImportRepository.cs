using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class FundingImportRepository : IFundingImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public FundingImportRepository (ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task InsertMany(IEnumerable<FundingImport> fundingImports)
        {
            await _coursesDataContext.FundingImport.AddRangeAsync(fundingImports);
            await _coursesDataContext.SaveChangesAsync();
        }
        public async Task DeleteAll()
        {
            _coursesDataContext.FundingImport.RemoveRange(_coursesDataContext.FundingImport);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<FundingImport>> GetAll()
        {
            var results = await _coursesDataContext.FundingImport.ToListAsync();

            return results;
        }
    }
}
