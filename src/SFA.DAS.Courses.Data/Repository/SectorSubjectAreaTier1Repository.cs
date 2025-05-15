using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class SectorSubjectAreaTier1Repository : ISectorSubjectAreaTier1Repository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public SectorSubjectAreaTier1Repository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task DeleteAll()
        {
            _coursesDataContext.SectorSubjectAreaTier1.RemoveRange(_coursesDataContext.SectorSubjectAreaTier1);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task<List<SectorSubjectAreaTier1>> GetAll(CancellationToken cancellationToken)
            => await _coursesDataContext.SectorSubjectAreaTier1.AsNoTracking().ToListAsync(cancellationToken);

        public async Task InsertMany(IEnumerable<SectorSubjectAreaTier1> sectorSubjectAreaTier1Items)
        {
            await _coursesDataContext.SectorSubjectAreaTier1.AddRangeAsync(sectorSubjectAreaTier1Items);
            await _coursesDataContext.SaveChangesAsync();
        }
    }
}
