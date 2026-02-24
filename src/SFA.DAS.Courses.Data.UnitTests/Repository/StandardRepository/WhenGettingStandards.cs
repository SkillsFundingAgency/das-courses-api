using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.Customisations;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingStandards
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Available_Standards_Are_Returned_When_ActiveAvailableFilter_IsSpecified(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.ActiveAvailable, 
                false, null, CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Should().BeEquivalentTo(activeValidApprenticeshipStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Available_Standards_Are_Returned_When_ActiveAvailableFilter_IsSpecified_For_Export(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.ActiveAvailable, true, null, CourseType.Apprenticeship);

            actualStandards.Should().NotBeNull();
            actualStandards.Should().BeEquivalentTo(activeValidApprenticeshipStandards);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Active_Standards_Are_Returned_Including_Not_Available_To_Start_When_ActiveFilter_Specified(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.Active, false, null, CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(activeValidApprenticeshipStandards);
            expectedStandards.AddRange(activeInvalidApprenticeshipStandards);
            expectedStandards.AddRange(retiredApprenticeshipStandards);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Active_Standards_Are_Returned_Including_Retired_With_Distinct_LarsCode_When_ActiveFilter_Specified(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);

            // set up active version 
            var newActiveVersion = activeValidApprenticeshipStandards.First();
            newActiveVersion.IfateReferenceNumber = "ST0001";
            newActiveVersion.Version = "2";
            newActiveVersion.LarsCode = "100002";

            // add a retired version to have same IfateReferenceNumber and different LarsCode
            var retiredStandardWithDistinctLarsCode = activeValidApprenticeshipStandards.Select(x => new Standard { IfateReferenceNumber = x.IfateReferenceNumber, Status = "Retired", LarsCode = "100001", Version = "1", LarsStandard = newActiveVersion.LarsStandard }).First();
            allStandards.Add(retiredStandardWithDistinctLarsCode);

            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), 
                new List<int>(), 
                StandardFilter.Active, 
                false, 
                null, 
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(activeValidApprenticeshipStandards);
            expectedStandards.AddRange(activeInvalidApprenticeshipStandards);
            expectedStandards.AddRange(retiredApprenticeshipStandards);
            expectedStandards.Add(retiredStandardWithDistinctLarsCode);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_NotYetApproved_Standards_Are_Returned_When_NotYetApprovedFilter_Specified(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), 
                new List<int>(), 
                StandardFilter.NotYetApproved, 
                false,
                null,
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(notYetApprovedApprenticeshipStandards);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_All_Standards_Are_Returned_When_NoneFilter_Specified(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(), 
                new List<int>(), 
                StandardFilter.None, 
                false,
                null,
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            var expectedStandards = new List<Standard>();
            expectedStandards.AddRange(notYetApprovedApprenticeshipStandards);
            expectedStandards.AddRange(activeValidApprenticeshipStandards);
            expectedStandards.AddRange(activeInvalidApprenticeshipStandards);
            expectedStandards.AddRange(withdrawnApprenticeshipStandards);
            expectedStandards.AddRange(retiredApprenticeshipStandards);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Sector(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int> { activeValidApprenticeshipStandards[0].RouteCode },
                new List<int>(),
                StandardFilter.ActiveAvailable, 
                false,
                null,
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Should().BeEquivalentTo(new List<Standard> { activeValidApprenticeshipStandards[0] }, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Standard_Not_Valid_And_Does_Match_Route_Filter_Then_Standard_Not_Returned(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int> { activeInvalidApprenticeshipStandards[0].RouteCode },
                new List<int>(),
                StandardFilter.ActiveAvailable, 
                false,
                null,
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Should().BeEquivalentTo(new List<Standard>());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Level(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int> { activeValidApprenticeshipStandards[0].Level },
                StandardFilter.ActiveAvailable, 
                false,
                null,
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Should().BeEquivalentTo(new List<Standard> { activeValidApprenticeshipStandards[0] }, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task When_Filtering_By_Apprenticeship_Type_Then_Only_Apprenticeship_Standards_Are_Returned(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [FoundationApprenticeshipStandardsLarsValid] List<Standard> validFoundationApprenticeships,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(validFoundationApprenticeships);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int>(),
                StandardFilter.ActiveAvailable,
                includeAllProperties: false,
                ApprenticeshipType.Apprenticeship,
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Should().BeEquivalentTo(activeValidApprenticeshipStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task When_Filtering_By_FoundationApprenticeship_Type_Then_Only_FoundationApprenticeship_Standards_Are_Returned(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [FoundationApprenticeshipStandardsLarsValid] List<Standard> validFoundationApprenticeships,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(validFoundationApprenticeships);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int>(),
                StandardFilter.ActiveAvailable,
                false,
                ApprenticeshipType.FoundationApprenticeship,
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Should().BeEquivalentTo(validFoundationApprenticeships, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task When_Not_Filtering_By_ApprenticeshipType_Then_Only_All_Apprenticeships_Are_Returned(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [FoundationApprenticeshipStandardsLarsValid] List<Standard> validFoundationApprenticeships,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(validFoundationApprenticeships);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int>(),
                StandardFilter.ActiveAvailable,
                false,
                null,
                CourseType.Apprenticeship);

            // Assert
            actualStandards.Should().NotBeNull();
            var expectedStandards = activeValidApprenticeshipStandards.Concat(validFoundationApprenticeships);
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Level_And_Include_Not_Available_To_Start(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> activeValidShortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            activeInvalidApprenticeshipStandards[0].Level = activeValidApprenticeshipStandards[0].Level;
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(activeValidShortCourseStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(
                new List<int>(),
                new List<int> { activeValidApprenticeshipStandards[0].Level },
                StandardFilter.Active, 
                false,
                null,
                CourseType.Apprenticeship);

            var expectedStandards = new List<Standard>
            {
                activeValidApprenticeshipStandards[0],
                activeInvalidApprenticeshipStandards[0]
            };

            // Assert
            actualStandards.Should().NotBeNull();
            actualStandards.Should().BeEquivalentTo(expectedStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }
    }
}
