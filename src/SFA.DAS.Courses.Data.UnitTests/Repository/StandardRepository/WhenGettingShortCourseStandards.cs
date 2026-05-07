using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.Customisations;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingShortCourseStandards : StandardRepositoryTestBase
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Available_ShortCourses_Are_Returned_When_ActiveAvailableFilter_IsSpecified(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.ActiveValidShortCourseStandards);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.ActiveAvailable, false, new List<ApprenticeshipType>(), CourseType.ShortCourse);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(3);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Available_ShortCourses_Are_Returned_When_ActiveAvailableFilter_IsSpecified_For_Export(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.ActiveValidShortCourseStandards);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.ActiveAvailable, true, new List<ApprenticeshipType>(), CourseType.ShortCourse);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(3);
            actualStandards.Should().BeEquivalentTo(expectedStandards);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Active_ShortCourses_Are_Returned_Including_Not_Available_To_Start_When_ActiveFilter_Specified(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.ActiveValidShortCourseStandards);
            expectedStandards.AddRange(data.ActiveInvalidShortCourseStandards);
            expectedStandards.AddRange(data.RetiredShortCourseStandards);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.Active, false, new List<ApprenticeshipType>(), CourseType.ShortCourse);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(9);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Active_ShortCourses_Are_Returned_Including_Retired_With_Distinct_LarsCode_When_ActiveFilter_Specified(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedBaseStandards = new List<Standard>();
            expectedBaseStandards.AddRange(data.ActiveValidShortCourseStandards);
            expectedBaseStandards.AddRange(data.ActiveInvalidShortCourseStandards);
            expectedBaseStandards.AddRange(data.RetiredShortCourseStandards);

            var activeStandard = data.ActiveValidShortCourseStandards[0];
            var originalLarsCode = activeStandard.LarsCode;

            var retiredStandard = new Standard
            {
                IfateReferenceNumber = activeStandard.IfateReferenceNumber,
                LarsCode = originalLarsCode,
                Version = "1.0",
                VersionMajor = 1,
                VersionMinor = 0,
                ShortCourseDates = activeStandard.ShortCourseDates,
                Status = "Retired",
                ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                CourseType = CourseType.ShortCourse
            };
            data.RetiredShortCourseStandards.Add(retiredStandard);

            activeStandard.Version = "2.0";
            activeStandard.VersionMajor = 2;
            activeStandard.VersionMinor = 0;
            activeStandard.LarsCode = "ZSC00999";
            activeStandard.ShortCourseDates = new ShortCourseDates
            {
                LarsCode = "ZSC00999",
                EffectiveFrom = activeStandard.ShortCourseDates.EffectiveFrom,
                EffectiveTo = activeStandard.ShortCourseDates.EffectiveTo,
                LastDateStarts = activeStandard.ShortCourseDates.LastDateStarts
            };

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.Active, false, new List<ApprenticeshipType>(), CourseType.ShortCourse);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(10);
            actualStandards.Should().BeEquivalentTo(expectedBaseStandards.Concat([retiredStandard]), EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_NotYetApproved_ShortCourses_Are_Returned_When_NotYetApprovedFilter_Specified(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.NotYetApprovedShortCourseStandards);
            
            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.NotYetApproved, false, new List<ApprenticeshipType>(), CourseType.ShortCourse);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(3);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_All_ShortCourses_Are_Returned_When_NoneFilter_Specified(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.ActiveValidShortCourseStandards);
            expectedStandards.AddRange(data.ActiveInvalidShortCourseStandards);
            expectedStandards.AddRange(data.RetiredShortCourseStandards);
            expectedStandards.AddRange(data.WithdrawnShortCourseStandards);
            expectedStandards.AddRange(data.NotYetApprovedShortCourseStandards);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.None, false, new List<ApprenticeshipType>(), CourseType.ShortCourse);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(15);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_ShortCourses_Are_Filtered_By_Sector(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.Add(data.ActiveValidShortCourseStandards[0]);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int> { data.ActiveValidShortCourseStandards[0].RouteCode },
                new List<int>(), StandardFilter.ActiveAvailable, false, new List<ApprenticeshipType>(), CourseType.ShortCourse);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(1);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_ShortCourse_Not_Available_When_Does_Match_Route_Filter_Then_ShortCourse_Not_Returned(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            
            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int> { data.ActiveInvalidShortCourseStandards[0].RouteCode },
                new List<int>(), StandardFilter.ActiveAvailable, false, new List<ApprenticeshipType>(), CourseType.ShortCourse);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(0);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Available_ShortCourses_Are_Filtered_By_Level(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.Add(data.ActiveValidShortCourseStandards[0]);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int> { data.ActiveValidShortCourseStandards[0].Level },
                StandardFilter.ActiveAvailable, false, new List<ApprenticeshipType>(), CourseType.ShortCourse);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(1);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Active_ShortCourses_Are_Filtered_By_Level(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.Add(data.ActiveValidShortCourseStandards[0]);
            expectedStandards.Add(data.ActiveInvalidShortCourseStandards[0]);
            expectedStandards.Add(data.RetiredShortCourseStandards[0]);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int> { data.ActiveValidShortCourseStandards[0].Level, data.ActiveInvalidShortCourseStandards[0].Level, data.RetiredShortCourseStandards[0].Level },
                StandardFilter.Active,
                false,
                new List<ApprenticeshipType>(),
                CourseType.ShortCourse);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(3);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Apprenticeship_ShortCourses_Are_Returned_When_Filtering_By_Apprenticeship_Type(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.ActiveValidShortCourseStandards);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int>(),
                StandardFilter.ActiveAvailable,
                includeAllProperties: false,
                new List<ApprenticeshipType> { ApprenticeshipType.ApprenticeshipUnit },
                CourseType.ShortCourse);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(3);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }
    }
}
