using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SFA.DAS.Courses.Data.UnitTests.Customisations;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class StandardRepositoryTestBase
    {
        protected void SetupContext(
            Mock<ICoursesDataContext> mockDataContext,
            StandardRepositoryTestData data)
        {
            mockDataContext
                .Setup(c => c.Standards)
                .ReturnsDbSet(data.AllStandards);

            mockDataContext
                .Setup(c => c.ShortCourseDates)
                .ReturnsDbSet(GetShortCourseDates(data.AllStandards));

            mockDataContext
                .Setup(c => c.LarsStandards)
                .ReturnsDbSet(data.AllStandards.Where(x => x.LarsStandard != null).Select(x => x.LarsStandard).ToList());

            mockDataContext
                .Setup(c => c.SectorSubjectAreaTier1)
                .ReturnsDbSet(data.AllStandards.Where(x => x.LarsStandard != null)
                    .Select(x => x.LarsStandard.SectorSubjectArea1));

            mockDataContext
                .Setup(c => c.SectorSubjectAreaTier2)
                .ReturnsDbSet(data.AllStandards.Where(x => x.LarsStandard != null)
                    .Select(x => x.LarsStandard.SectorSubjectArea2));

            mockDataContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());
        }

        private List<ShortCourseDates> GetShortCourseDates(List<Standard> standards)
        {
            return standards
               .Where(s => s.CourseType == CourseType.ShortCourse && s.LarsCode != string.Empty)
               .GroupBy(s => s.LarsCode)
               .Select(g => new ShortCourseDates
               {
                   LarsCode = g.Key,
                   EffectiveFrom = g
                        .OrderBy(x => x.VersionMajor)
                        .ThenBy(x => x.VersionMinor)
                        .Select(x => x.ApprovedForDelivery.GetValueOrDefault(DateTime.MinValue))
                        .FirstOrDefault(),
                   EffectiveTo = g
                        .OrderByDescending(x => x.VersionMajor)
                        .ThenByDescending(x => x.VersionMinor)
                        .Select(x => x.VersionLatestStartDate)
                        .FirstOrDefault(),
                   LastDateStarts = g
                        .OrderByDescending(x => x.VersionMajor)
                        .ThenByDescending(x => x.VersionMinor)
                    .Select(x => x.VersionLatestStartDate)
                        .FirstOrDefault()
               })
               .ToList();
        }
    }
}
