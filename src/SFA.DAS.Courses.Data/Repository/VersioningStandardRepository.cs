using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<Standard> GetStandardByUId(string standardUId)
        {
            return coursesDataContext.VersioningStandard.FirstOrDefaultAsync(v => v.StandardUId == standardUId);
        }

        public async Task InsertMany(IEnumerable<Standard> standards, IEnumerable<StandardAdditionalInformation> additionalInformation)
        {
            coursesDataContext.VersioningStandard.RemoveRange(coursesDataContext.VersioningStandard);

            await coursesDataContext.VersioningStandard.AddRangeAsync(standards);

            coursesDataContext.VersioningStandardAdditionalInformation.RemoveRange(coursesDataContext.VersioningStandardAdditionalInformation);

            await coursesDataContext.VersioningStandardAdditionalInformation.AddRangeAsync(additionalInformation);

            coursesDataContext.SaveChanges();
        }

        public async Task<IEnumerable<Standard>> GetAllActiveStandardsSummary()
        {
            return await coursesDataContext.VersioningStandard.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetOptions(string standardUId)
        {
            var standardInfo = await coursesDataContext.VersioningStandardAdditionalInformation.Where(v => v.StandardUId == standardUId).FirstOrDefaultAsync();
            if (standardInfo == null) throw new ArgumentException("Invalid standardUId");
            return standardInfo.Options?.ToArray() ?? Array.Empty<string>();
        }
    }
}
