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
    public class WhenFilteringAvailableStandards
    {
        [Test]
        public void Then_Apprenticeships_With_Null_LastDateStarts_Are_Returned()
        {
            // Arrange
            var standards = new List<Standard>
            {
                ValidApprenticeship(),
                new Standard
                {
                    Title = "Also Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    Status = "Approved for delivery",
                    CourseType = CourseType.Apprenticeship,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-2),
                        LastDateStarts = null
                    }
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable).ToList();

            // Assert
            actual.Should().HaveCount(2);
            actual.Select(x => x.Title).Should().BeEquivalentTo("Available", "Also Available");
        }

        [Test]
        public void Then_Apprenticeships_With_A_Future_Start_Date_Are_Not_Returned()
        {
            // Arrange
            var standards = new List<Standard>
            {
                ValidApprenticeship(),
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    Status = "Approved for delivery",
                    CourseType = CourseType.Apprenticeship,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(1),
                        LastDateStarts = null
                    }
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable).ToList();

            // Assert
            actual.Should().ContainSingle();
            actual.Single().Title.Should().Be("Available");
        }

        [Test]
        public void Then_Apprenticeships_With_LastDateStarts_In_The_Past_Are_Not_Returned()
        {
            // Arrange
            var standards = new List<Standard>
            {
                ValidApprenticeship(),
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    Status = "Approved for delivery",
                    CourseType = CourseType.Apprenticeship,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                        LastDateStarts = DateTime.UtcNow.AddDays(-1)
                    }
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable).ToList();

            // Assert
            actual.Should().ContainSingle();
            actual.Single().Title.Should().Be("Available");
        }

        [Test]
        public void Then_Apprenticeships_With_The_Same_EffectiveFrom_And_LastDateStarts_Are_Not_Returned()
        {
            // Arrange
            var sameDate = DateTime.UtcNow;

            var standards = new List<Standard>
            {
                ValidApprenticeship(),
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    Status = "Approved for delivery",
                    CourseType = CourseType.Apprenticeship,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = sameDate,
                        LastDateStarts = sameDate
                    }
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable).ToList();

            // Assert
            actual.Should().ContainSingle();
            actual.Single().Title.Should().Be("Available");
        }

        [Test]
        public void Then_ShortCourses_With_Null_VersionLatestStartDate_Are_Returned()
        {
            // Arrange
            var standards = new List<Standard>
            {
                ValidShortCourse(),
                new Standard
                {
                    Title = "Also Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    Status = "Approved for delivery",
                    CourseType = CourseType.ShortCourse,
                    LarsCode = "ZSC00002",
                    VersionEarliestStartDate = DateTime.UtcNow.AddMonths(-2),
                    VersionLatestStartDate = null
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable).ToList();

            // Assert
            actual.Should().HaveCount(2);
            actual.Select(x => x.Title).Should().BeEquivalentTo("Available", "Also Available");
        }

        [Test]
        public void Then_ShortCourses_With_A_Future_EarliestStartDate_Are_Not_Returned()
        {
            // Arrange
            var standards = new List<Standard>
            {
                ValidShortCourse(),
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    Status = "Approved for delivery",
                    CourseType = CourseType.ShortCourse,
                    LarsCode = "ZSC00002",
                    VersionEarliestStartDate = DateTime.UtcNow.AddMonths(1),
                    VersionLatestStartDate = null
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable).ToList();

            // Assert
            actual.Should().ContainSingle();
            actual.Single().Title.Should().Be("Available");
        }

        [Test]
        public void Then_ShortCourses_With_VersionLatestStartDate_In_The_Past_Are_Not_Returned()
        {
            // Arrange
            var standards = new List<Standard>
            {
                ValidShortCourse(),
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    Status = "Approved for delivery",
                    CourseType = CourseType.ShortCourse,
                    LarsCode = "ZSC00002",
                    VersionEarliestStartDate = DateTime.UtcNow.AddMonths(-1),
                    VersionLatestStartDate = DateTime.UtcNow.AddDays(-1)
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable).ToList();

            // Assert
            actual.Should().ContainSingle();
            actual.Single().Title.Should().Be("Available");
        }

        [Test]
        public void Then_ShortCourses_With_The_Same_VersionEarliestStartDate_And_VersionLatestStartDate_Are_Not_Returned()
        {
            // Arrange
            var sameDate = DateTime.UtcNow;

            var standards = new List<Standard>
            {
                ValidShortCourse(),
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    Status = "Approved for delivery",
                    CourseType = CourseType.ShortCourse,
                    LarsCode = "ZSC00002",
                    VersionEarliestStartDate = sameDate,
                    VersionLatestStartDate = sameDate
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable).ToList();

            // Assert
            actual.Should().ContainSingle();
            actual.Single().Title.Should().Be("Available");
        }

        private static Standard ValidApprenticeship()
        {
            return new Standard
            {
                Title = "Available",
                ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                Status = "Approved for delivery",
                CourseType = CourseType.Apprenticeship,
                LarsStandard = new LarsStandard
                {
                    EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                    LastDateStarts = null
                }
            };
        }

        private static Standard ValidShortCourse()
        {
            return new Standard
            {
                Title = "Available",
                ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                Status = "Approved for delivery",
                CourseType = CourseType.ShortCourse,
                LarsCode = "ZSC00001",
                LarsStandard = null,
                VersionEarliestStartDate = DateTime.UtcNow.AddMonths(-1),
                VersionLatestStartDate = null
            };
        }
    }
}
