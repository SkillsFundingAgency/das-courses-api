﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities.Versioning;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class StandardStagingRepository: IStandardStagingRepository
    {
        private readonly ICoursesDataContext coursesDataContext;

        public StandardStagingRepository(ICoursesDataContext coursesDataContext)
        {
            this.coursesDataContext = coursesDataContext;
        }

        public async Task InsertMany(IEnumerable<StandardStaging> standardsStaging)
        {
            coursesDataContext.StandardStaging.RemoveRange(coursesDataContext.StandardStaging);

            await coursesDataContext.StandardStaging.AddRangeAsync(standardsStaging);

            coursesDataContext.SaveChanges();
        }
    }
}
