using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.Customisations;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingCount : StandardRepositoryTestBase
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Available_Standards(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedCount = data.ActiveValidApprenticeshipStandards.Count +
                data.ActiveValidFoundationApprenticeshipStandards.Count;

            SetupContext(mockDataContext, data);

            // Act
            var count = await repository.Count(StandardFilter.ActiveAvailable, CourseType.Apprenticeship);

            // Assert
            count.Should().Be(expectedCount);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Active_Standards(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedCount =
                data.ActiveValidApprenticeshipStandards.Count +
                data.ActiveInvalidApprenticeshipStandards.Count +
                data.RetiredApprenticeshipStandards.Count +
                data.ActiveValidFoundationApprenticeshipStandards.Count;

            SetupContext(mockDataContext, data);

            // Act
            var count = await repository.Count(StandardFilter.Active, CourseType.Apprenticeship);

            // Assert
            count.Should().Be(expectedCount);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Count_Includes_A_Retired_Standard_With_A_Distinct_LarsCode_For_The_Same_IfateReferenceNumber(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedBaseCount =
                data.ActiveValidApprenticeshipStandards.Count +
                data.ActiveInvalidApprenticeshipStandards.Count +
                data.RetiredApprenticeshipStandards.Count +
                data.ActiveValidFoundationApprenticeshipStandards.Count;

            var activeStandard = data.ActiveValidApprenticeshipStandards[0];
            var originalLarsCode = activeStandard.LarsCode;

            data.RetiredApprenticeshipStandards.Add(new Standard
            {
                IfateReferenceNumber = activeStandard.IfateReferenceNumber,
                LarsCode = originalLarsCode,
                Version = "1.0",
                VersionMajor = 1,
                VersionMinor = 0,
                LarsStandard = activeStandard.LarsStandard,
                Status = "Retired",
                ApprenticeshipType = ApprenticeshipType.Apprenticeship,
                CourseType = CourseType.Apprenticeship
            });

            activeStandard.Version = "2.0";
            activeStandard.VersionMajor = 2;
            activeStandard.VersionMinor = 0;
            activeStandard.LarsCode = "999";
            activeStandard.LarsStandard = new LarsStandard
            { 
                LarsCode = "999",
                EffectiveFrom = activeStandard.LarsStandard.EffectiveFrom,
                EffectiveTo = activeStandard.LarsStandard.EffectiveTo,
                LastDateStarts = activeStandard.LarsStandard.LastDateStarts
            };


            SetupContext(mockDataContext, data);

            // Act
            var count = await repository.Count(StandardFilter.Active, CourseType.Apprenticeship);

            // Assert
            count.Should().Be(expectedBaseCount + 1); // The additional retired standard with unique lars code
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_NotYetApproved_Standards(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            SetupContext(mockDataContext, data);

            // Act
            var count = await repository.Count(StandardFilter.NotYetApproved, CourseType.Apprenticeship);

            // Assert
            count.Should().Be(data.NotYetApprovedApprenticeshipStandards.Count);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_All_Standards(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            SetupContext(mockDataContext, data);

            // Act
            var count = await repository.Count(StandardFilter.None, CourseType.Apprenticeship);

            // Assert
            var total = data.ActiveValidApprenticeshipStandards.Count
                + data.ActiveValidFoundationApprenticeshipStandards.Count
                + data.ActiveInvalidApprenticeshipStandards.Count
                + data.NotYetApprovedApprenticeshipStandards.Count
                + data.WithdrawnApprenticeshipStandards.Count
                + data.RetiredApprenticeshipStandards.Count;

            count.Should().Be(total);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Available_ShortCourses(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedCount =
                data.ActiveValidShortCourseStandards.Count;

            SetupContext(mockDataContext, data);

            // Act
            var count = await repository.Count(StandardFilter.ActiveAvailable, CourseType.ShortCourse);

            // Assert
            count.Should().Be(expectedCount);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Active_ShortCourses(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedCount =
                data.ActiveValidShortCourseStandards.Count +
                data.ActiveInvalidShortCourseStandards.Count +
                data.RetiredShortCourseStandards.Count;

            SetupContext(mockDataContext, data);

            // Act
            var count = await repository.Count(StandardFilter.Active, CourseType.ShortCourse);

            // Assert
            count.Should().Be(expectedCount);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_NotYetApproved_ShortCourses(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedCount =
                data.NotYetApprovedApprenticeshipStandards.Count;

            SetupContext(mockDataContext, data);

            // Act
            var count = await repository.Count(StandardFilter.NotYetApproved, CourseType.ShortCourse);

            // Assert
            count.Should().Be(expectedCount);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_All_ShortCourses(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedCount =
                data.ActiveValidShortCourseStandards.Count +
                data.ActiveInvalidShortCourseStandards.Count +
                data.RetiredShortCourseStandards.Count + 
                data.WithdrawnShortCourseStandards.Count + 
                data.NotYetApprovedApprenticeshipStandards.Count;

            SetupContext(mockDataContext, data);

            // Act
            var count = await repository.Count(StandardFilter.None, CourseType.ShortCourse);

            // Assert
            count.Should().Be(expectedCount);
        }
    }
}
