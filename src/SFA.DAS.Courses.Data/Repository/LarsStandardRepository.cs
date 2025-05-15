using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class LarsStandardRepository : ILarsStandardRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public LarsStandardRepository (ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }
        public async Task InsertMany(IEnumerable<LarsStandard> larsStandardImports)
        {
            await _coursesDataContext.LarsStandards.AddRangeAsync(larsStandardImports);
            
            
            await _coursesDataContext.SaveChangesAsync();
        }
        
        public async Task DeleteAll()
        {
            _coursesDataContext.LarsStandards.RemoveRange(_coursesDataContext.LarsStandards);
            await _coursesDataContext.SaveChangesAsync();
        }
    }
}
