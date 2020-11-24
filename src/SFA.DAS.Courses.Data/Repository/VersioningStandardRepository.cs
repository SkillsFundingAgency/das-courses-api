using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities.Versioning;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class VersioningStandardRepository : IVersioningStandardRepository
    {
        private readonly ICoursesDataContext coursesDataContext;

        public VersioningStandardRepository(ICoursesDataContext coursesDataContext)
        {
            this.coursesDataContext = coursesDataContext;
        }

        public async Task<Standard> GetStandardByUId(string standardUId)
        {
            return await coursesDataContext.VersioningStandard.FirstOrDefaultAsync(s => s.StandardUId == standardUId);
        }

        public async Task InsertMany(IEnumerable<Standard> standards, IEnumerable<StandardAdditionalInformation> additionalInformation)
        {
            coursesDataContext.VersioningStandard.RemoveRange(coursesDataContext.VersioningStandard);

            await coursesDataContext.VersioningStandard.AddRangeAsync(standards);

            coursesDataContext.VersioningStandardAdditionalInformation.RemoveRange(coursesDataContext.VersioningStandardAdditionalInformation);

            await coursesDataContext.VersioningStandardAdditionalInformation.AddRangeAsync(additionalInformation);

            coursesDataContext.SaveChanges();
        }
    }
}
