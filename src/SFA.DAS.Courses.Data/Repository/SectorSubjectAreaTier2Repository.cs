using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class SectorSubjectAreaTier2Repository : ISectorSubjectAreaTier2Repository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public SectorSubjectAreaTier2Repository (ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task DeleteAll()
        {
            await _coursesDataContext.DeleteAllBatchedAsync<SectorSubjectAreaTier2>();
        }

        public async Task InsertMany(IEnumerable<SectorSubjectAreaTier2> sectorSubjectAreaTier2Items)
        {
            await _coursesDataContext.SectorSubjectAreaTier2.AddRangeAsync(sectorSubjectAreaTier2Items);
            await _coursesDataContext.SaveChangesAsync();
        }
    }
}
