using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Data.Extensions;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Data.UnitTests.Extensions
{
    public class WhenFilteringNotYetApprovedStandards
    {
        [Test]
        public void Then_In_Development_And_Proposal_In_Development_Standards_Are_Returned()
        {
            // Arrange
            var standards = new List<Standard>
            {
                new Standard
                {
                    CourseType = CourseType.Apprenticeship,
                    LarsCode = "0",
                    Title = "Not Yet Approved Apprenticeship",
                    Status = "In development",
                },
                new Standard
                {
                    CourseType = CourseType.Apprenticeship,
                    LarsCode = "0",
                    Title = "Not Yet Approved Apprenticeship",
                    Status = "Proposal in development",
                },
                new Standard
                {
                    CourseType = CourseType.ShortCourse,
                    LarsCode = string.Empty,
                    Title = "Not Yet Approved Short Course",
                    Status = "In development",
                },
                new Standard
                {
                    CourseType = CourseType.ShortCourse,
                    LarsCode = string.Empty,
                    Title = "Not Yet Approved Short Course",
                    Status = "Proposal in development",
                },
                new Standard
                {
                    CourseType = CourseType.Apprenticeship,
                    LarsCode = "1",
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                        LastDateStarts = DateTime.UtcNow.AddDays(-1)
                    },
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    CourseType = CourseType.Apprenticeship,
                    LarsCode = "2",
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(1),
                        LastDateStarts = null
                    },
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    CourseType = CourseType.ShortCourse,
                    LarsCode = "ZSC00001",
                    Title = "Available Short Course",
                    Status = "Approved for delivery",
                    VersionEarliestStartDate = DateTime.UtcNow.AddMonths(-2),
                    VersionLatestStartDate = DateTime.UtcNow.AddMonths(-1)
                }
            }.AsQueryable();

            // Act
            var actual = standards
                .FilterStandards(StandardFilter.NotYetApproved)
                .ToList();

            // Assert
            actual.Should().HaveCount(4);
            actual.Should().OnlyContain(x =>
                x.Status == "In development" || x.Status == "Proposal in development");
            actual.Should().Contain(x => x.CourseType == CourseType.Apprenticeship && x.LarsCode == "0");
            actual.Should().Contain(x => x.CourseType == CourseType.ShortCourse && x.LarsCode == string.Empty);
        }
    }
}
