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
    public class WhenGettingApprenticeshipStandards : StandardRepositoryTestBase
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Available_Standards_Are_Returned_When_ActiveAvailableFilter_IsSpecified(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.ActiveValidApprenticeshipStandards);
            expectedStandards.AddRange(data.ActiveValidFoundationApprenticeshipStandards);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.ActiveAvailable, false, new List<ApprenticeshipType>(), CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(6);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Available_Standards_Are_Returned_When_ActiveAvailableFilter_IsSpecified_For_Export(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.ActiveValidApprenticeshipStandards);
            expectedStandards.AddRange(data.ActiveValidFoundationApprenticeshipStandards);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.ActiveAvailable, true, new List<ApprenticeshipType>(), CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(6);
            actualStandards.Should().BeEquivalentTo(expectedStandards);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Active_Standards_Are_Returned_Including_Not_Available_To_Start_When_ActiveFilter_Specified(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.ActiveValidApprenticeshipStandards);
            expectedStandards.AddRange(data.ActiveInvalidApprenticeshipStandards);
            expectedStandards.AddRange(data.RetiredApprenticeshipStandards);
            expectedStandards.AddRange(data.ActiveValidFoundationApprenticeshipStandards);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.Active, false, new List<ApprenticeshipType>(), CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(12);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Active_Standards_Are_Returned_Including_Retired_With_Distinct_LarsCode_When_ActiveFilter_Specified(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedBaseStandards = new List<Standard>();
            expectedBaseStandards.AddRange(data.ActiveValidApprenticeshipStandards);
            expectedBaseStandards.AddRange(data.ActiveInvalidApprenticeshipStandards);
            expectedBaseStandards.AddRange(data.RetiredApprenticeshipStandards);
            expectedBaseStandards.AddRange(data.ActiveValidFoundationApprenticeshipStandards);

            var activeStandard = data.ActiveValidApprenticeshipStandards[0];
            var originalLarsCode = activeStandard.LarsCode;

            var retiredStandard = new Standard
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
            };
            data.RetiredApprenticeshipStandards.Add(retiredStandard);

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
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.Active, false, new List<ApprenticeshipType>(), CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(13);
            actualStandards.Should().BeEquivalentTo(expectedBaseStandards.Concat([retiredStandard]), EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_NotYetApproved_Standards_Are_Returned_When_NotYetApprovedFilter_Specified(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.NotYetApprovedApprenticeshipStandards);
            
            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.NotYetApproved, false, new List<ApprenticeshipType>(), CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(3);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_All_Standards_Are_Returned_When_NoneFilter_Specified(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.ActiveValidApprenticeshipStandards);
            expectedStandards.AddRange(data.ActiveInvalidApprenticeshipStandards);
            expectedStandards.AddRange(data.RetiredApprenticeshipStandards);
            expectedStandards.AddRange(data.WithdrawnApprenticeshipStandards);
            expectedStandards.AddRange(data.NotYetApprovedApprenticeshipStandards);
            expectedStandards.AddRange(data.ActiveValidFoundationApprenticeshipStandards);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), new List<int>(), StandardFilter.None, false, new List<ApprenticeshipType>(), CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(18);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Sector(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.Add(data.ActiveValidApprenticeshipStandards[0]);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int> { data.ActiveValidApprenticeshipStandards[0].RouteCode },
                new List<int>(), StandardFilter.ActiveAvailable, false, new List<ApprenticeshipType>(), CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(1);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Standard_Not_Available_When_Does_Match_Route_Filter_Then_Standard_Not_Returned(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            
            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int> { data.ActiveInvalidApprenticeshipStandards[0].RouteCode },
                new List<int>(), StandardFilter.ActiveAvailable, false, new List<ApprenticeshipType>(), CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(0);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Available_Standards_Are_Filtered_By_Level(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.Add(data.ActiveValidApprenticeshipStandards[0]);
            expectedStandards.Add(data.ActiveValidFoundationApprenticeshipStandards[0]);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int> { data.ActiveValidApprenticeshipStandards[0].Level, data.ActiveValidFoundationApprenticeshipStandards[0].Level },
                StandardFilter.ActiveAvailable, false, new List<ApprenticeshipType>(), CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(2);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Active_Standards_Are_Filtered_By_Level(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.Add(data.ActiveValidApprenticeshipStandards[0]);
            expectedStandards.Add(data.ActiveInvalidApprenticeshipStandards[0]);
            expectedStandards.Add(data.RetiredApprenticeshipStandards[0]);
            expectedStandards.Add(data.ActiveValidFoundationApprenticeshipStandards[0]);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int> { data.ActiveValidApprenticeshipStandards[0].Level, data.RetiredApprenticeshipStandards[0].Level, data.ActiveValidFoundationApprenticeshipStandards[0].Level },
                StandardFilter.Active,
                false,
                new List<ApprenticeshipType>(),
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(4);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Apprenticeship_Standards_Are_Returned_When_Filtering_By_Apprenticeship_Type(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.ActiveValidApprenticeshipStandards);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int>(),
                StandardFilter.ActiveAvailable,
                includeAllProperties: false,
                new List<ApprenticeshipType> { ApprenticeshipType.Apprenticeship },
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(3);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Foundation_Apprenticeship_Standards_Are_Returned_When_Filtering_By_FoundationApprenticeship_Type(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(data.ActiveValidFoundationApprenticeshipStandards);

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int>(),
                StandardFilter.ActiveAvailable,
                false,
                new List<ApprenticeshipType> { ApprenticeshipType.FoundationApprenticeship },
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Count().Should().Be(3);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }
    }
}
