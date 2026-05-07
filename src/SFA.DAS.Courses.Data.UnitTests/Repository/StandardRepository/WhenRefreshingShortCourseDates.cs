using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.Customisations;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenRefreshingShortCourseDates : StandardRepositoryTestBase
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Short_Course_Dates_Are_Rebuilt_Using_Earliest_VersionEarliestStartDate_And_Latest_VersionLatestStartDate(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var shortCourse = data.ActiveValidShortCourseStandards[0];

            shortCourse.LarsCode = "ZSC00001";
            shortCourse.Version = "1.0";
            shortCourse.VersionMajor = 1;
            shortCourse.VersionMinor = 0;
            shortCourse.VersionEarliestStartDate = new DateTime(2024, 01, 10, 0, 0, 0, DateTimeKind.Utc);
            shortCourse.VersionLatestStartDate = new DateTime(2024, 06, 10, 0, 0, 0, DateTimeKind.Utc);
            shortCourse.ApprovedForDelivery = new DateTime(2024, 02, 10, 0, 0, 0, DateTimeKind.Utc);

            var laterVersion = new Standard
            {
                IfateReferenceNumber = shortCourse.IfateReferenceNumber,
                LarsCode = shortCourse.LarsCode,
                Version = "1.1",
                VersionMajor = 1,
                VersionMinor = 1,
                VersionEarliestStartDate = new DateTime(2024, 03, 10, 0, 0, 0, DateTimeKind.Utc),
                VersionLatestStartDate = new DateTime(2024, 09, 10, 0, 0, 0, DateTimeKind.Utc),
                ApprovedForDelivery = new DateTime(2024, 04, 10, 0, 0, 0, DateTimeKind.Utc),
                Status = shortCourse.Status,
                ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                CourseType = CourseType.ShortCourse,
                ShortCourseDates = new ShortCourseDates
                {
                    LarsCode = shortCourse.LarsCode,
                    EffectiveFrom = shortCourse.ShortCourseDates?.EffectiveFrom ?? DateTime.MinValue,
                    EffectiveTo = shortCourse.ShortCourseDates?.EffectiveTo,
                    LastDateStarts = shortCourse.ShortCourseDates?.LastDateStarts
                }
            };

            data.ActiveValidShortCourseStandards.Clear();
            data.ActiveValidShortCourseStandards.Add(shortCourse);
            data.ActiveValidShortCourseStandards.Add(laterVersion);

            var storedShortCourseDates = new List<ShortCourseDates>();

            SetupContext(mockDataContext, data);
            SetupMutableShortCourseDates(mockDataContext, storedShortCourseDates);

            // Act
            await repository.RefreshShortCourseDates();

            // Assert
            storedShortCourseDates.Should().HaveCount(data.ActiveInvalidShortCourseStandards.Count
                + data.RetiredShortCourseStandards.Count + 1);

            var shortCourseDates = storedShortCourseDates
                .Single(p => p.LarsCode == shortCourse.LarsCode);

            shortCourseDates.LarsCode.Should().Be("ZSC00001");
            shortCourseDates.EffectiveFrom.Should().Be(new DateTime(2024, 01, 10, 0, 0, 0, DateTimeKind.Utc));
            shortCourseDates.EffectiveFrom.Should().NotBe(new DateTime(2024, 02, 10, 0, 0, 0, DateTimeKind.Utc));
            shortCourseDates.EffectiveTo.Should().Be(new DateTime(2024, 09, 10, 0, 0, 0, DateTimeKind.Utc));
            shortCourseDates.LastDateStarts.Should().Be(new DateTime(2024, 09, 10, 0, 0, 0, DateTimeKind.Utc));

            mockDataContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Invalid_Active_Short_Courses_Are_Included_In_The_Refresh(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var valid = data.ActiveValidShortCourseStandards[0];
            var invalid = data.ActiveInvalidShortCourseStandards[0];

            valid.LarsCode = "ZSC00010";
            invalid.LarsCode = "ZSC00020";

            var storedShortCourseDates = new List<ShortCourseDates>();

            SetupContext(mockDataContext, data);
            SetupMutableShortCourseDates(mockDataContext, storedShortCourseDates);

            // Act
            await repository.RefreshShortCourseDates();

            // Assert
            storedShortCourseDates.Select(x => x.LarsCode).Should().Contain("ZSC00010");
            storedShortCourseDates.Select(x => x.LarsCode).Should().Contain("ZSC00020");
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Standards_With_Empty_LarsCode_Are_Excluded_From_The_Refresh(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var shortCourse = data.ActiveValidShortCourseStandards[0];
            shortCourse.LarsCode = string.Empty;
            shortCourse.ShortCourseDates = null;

            data.ActiveValidShortCourseStandards.Clear();
            data.ActiveValidShortCourseStandards.Add(shortCourse);

            var storedShortCourseDates = new List<ShortCourseDates>();

            SetupContext(mockDataContext, data);
            SetupMutableShortCourseDates(mockDataContext, storedShortCourseDates);

            // Act
            await repository.RefreshShortCourseDates();

            // Assert
            storedShortCourseDates.Should().HaveCount(data.ActiveInvalidShortCourseStandards.Count
                + data.RetiredShortCourseStandards.Count);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Non_Short_Courses_Are_Not_Included_In_The_Refresh(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            data.ActiveValidShortCourseStandards.Clear();
            data.ActiveInvalidShortCourseStandards.Clear();
            data.RetiredShortCourseStandards.Clear();
            data.NotYetApprovedShortCourseStandards.Clear();
            data.WithdrawnShortCourseStandards.Clear();

            var storedShortCourseDates = new List<ShortCourseDates>();

            SetupContext(mockDataContext, data);
            SetupMutableShortCourseDates(mockDataContext, storedShortCourseDates);

            // Act
            await repository.RefreshShortCourseDates();

            // Assert
            storedShortCourseDates.Should().BeEmpty();
        }

        private static void SetupMutableShortCourseDates(
            Mock<ICoursesDataContext> mockDataContext,
            List<ShortCourseDates> storedShortCourseDates)
        {
            mockDataContext
                .Setup(c => c.ShortCourseDates)
                .ReturnsDbSet(storedShortCourseDates);

            mockDataContext
                .Setup(c => c.SaveChangesAsync(default))
                .ReturnsAsync(1);

            mockDataContext
                .Setup(c => c.ShortCourseDates.AddRangeAsync(It.IsAny<IEnumerable<ShortCourseDates>>(), default))
                .Callback<IEnumerable<ShortCourseDates>, System.Threading.CancellationToken>((items, _) =>
                {
                    storedShortCourseDates.AddRange(items);
                })
                .Returns(Task.CompletedTask);
        }
    }
}
