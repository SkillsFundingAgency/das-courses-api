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
    public class WhenFilteringActiveStandards
    {
        [Test]
        public void Then_All_Active_Apprenticeships_With_Lars_Records_Are_Returned()
        {
            // Arrange
            var sameDate = DateTime.UtcNow;
            var standards = new List<Standard>
            {
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    CourseType = CourseType.Apprenticeship,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = sameDate,
                        LastDateStarts = sameDate
                    },
                    ShortCourseDates = null,
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    CourseType = CourseType.Apprenticeship,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                        LastDateStarts = DateTime.UtcNow.AddDays(-1)
                    },
                    ShortCourseDates = null,
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    CourseType = CourseType.Apprenticeship,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(1),
                        LastDateStarts = null
                    },
                    ShortCourseDates = null,
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    CourseType = CourseType.Apprenticeship,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-2),
                        LastDateStarts = DateTime.UtcNow.AddMonths(-1)
                    },
                    ShortCourseDates = null,
                    Status = "Approved for delivery"
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.Active);

            // Assert
            actual.Count().Should().Be(4);
            actual.ToList().TrueForAll(c => c.Title.Equals("Available"));
        }

        [Test]
        public void Then_Apprenticeships_Without_Lars_Records_Are_Not_Returned()
        {
            // Arrange
            var sameDate = DateTime.UtcNow;
            var standards = new List<Standard>
            {
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    CourseType = CourseType.Apprenticeship,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = sameDate,
                        LastDateStarts = sameDate
                    },
                    ShortCourseDates = null,
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    CourseType = CourseType.Apprenticeship,
                    LarsStandard = null,
                    ShortCourseDates = null,
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    CourseType = CourseType.Apprenticeship,
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-2),
                        LastDateStarts = DateTime.UtcNow.AddMonths(-1)
                    },
                    ShortCourseDates = null,
                    Status = "Approved for delivery"
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.Active);

            // Assert
            actual.Count().Should().Be(2);
            actual.ToList().TrueForAll(c => c.Title.Equals("Available"));
        }

        [Test]
        public void Then_Valid_ShortCourses_Are_Returned_Without_Lars_Records()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var standards = new List<Standard>
            {
                new Standard
                {
                    Title = "Available Short Course",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    CourseType = CourseType.ShortCourse,
                    LarsCode = "ZSC00001",
                    LarsStandard = null,
                    ShortCourseDates = new ShortCourseDates
                    {
                        LarsCode = "ZSC00001",
                        EffectiveFrom = now,
                        EffectiveTo = now,
                        LastDateStarts = now
                    },
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Available Short Course",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    CourseType = CourseType.ShortCourse,
                    LarsCode = "ZSC00002",
                    LarsStandard = null,
                    ShortCourseDates = new ShortCourseDates
                    {
                        LarsCode = "ZSC00002",
                        EffectiveFrom = now,
                        EffectiveTo = now,
                        LastDateStarts = now
                    },
                    Status = "Retired"
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.Active);

            // Assert
            actual.Count().Should().Be(2);
            actual.ToList().TrueForAll(c => c.Title.Equals("Available Short Course"));
        }

        [Test]
        public void Then_ShortCourses_With_Missing_ShortCourseDates_Are_Not_Returned()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var standards = new List<Standard>
            {
                new Standard
                {
                    Title = "Available Short Course",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    CourseType = CourseType.ShortCourse,
                    LarsCode = "ZSC00001",
                    LarsStandard = null,
                    ShortCourseDates = new ShortCourseDates
                    {
                        LarsCode = "ZSC00001",
                        EffectiveFrom = now,
                        EffectiveTo = now,
                        LastDateStarts = now
                    },
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    CourseType = CourseType.ShortCourse,
                    LarsCode = string.Empty,
                    LarsStandard = null,
                    ShortCourseDates = null,
                    Status = "Approved for delivery"
                }
            }.AsQueryable();

            // Act
            var actual = standards.FilterStandards(StandardFilter.Active);

            // Assert
            actual.Count().Should().Be(1);
            actual.Single().Title.Should().Be("Available Short Course");
        }
    }
}
