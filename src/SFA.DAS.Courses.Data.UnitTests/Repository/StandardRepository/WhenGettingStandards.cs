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
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.ActiveAvailable, false);

            Assert.That(actualStandards, Is.Not.Null);
            actualStandards.Should().BeEquivalentTo(activeValidStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Available_Standards_Are_Returned_When_ActiveAvailableFilter_IsSpecified_For_Export(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.ActiveAvailable, true);

            Assert.That(actualStandards, Is.Not.Null);
            actualStandards.Should().BeEquivalentTo(activeValidStandards);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Active_Standards_Are_Returned_Including_Not_Available_To_Start_When_ActiveFilter_Specified(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.Active, false);

            Assert.That(actualStandards, Is.Not.Null);
            var expectedList = new List<Standard>();
            expectedList.AddRange(activeValidStandards);
            expectedList.AddRange(activeInvalidStandards);
            expectedList.AddRange(retiredStandards);
            actualStandards.Should().BeEquivalentTo(expectedList, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Active_Standards_Are_Returned_Including_Retired_With_Distinct_LarsCode_When_ActiveFilter_Specified(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);

            //set up active version 
            var newActiveVersion = activeValidStandards.First();
            newActiveVersion.IfateReferenceNumber = "ST0001";
            newActiveVersion.Version = "2";
            newActiveVersion.LarsCode = "100002";

            //add a retired version to have same IfateReferenceNumber and different LarsCode
            var retiredStandardWithDistinctLarsCode = activeValidStandards.Select(x => new Standard { IfateReferenceNumber = x.IfateReferenceNumber, Status = "Retired", LarsCode = "100001", Version = "1", LarsStandard = newActiveVersion.LarsStandard }).First();
            allStandards.Add(retiredStandardWithDistinctLarsCode);

            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.Active, false);

            Assert.That(actualStandards, Is.Not.Null);
            var expectedList = new List<Standard>();
            expectedList.AddRange(activeValidStandards);
            expectedList.AddRange(activeInvalidStandards);
            expectedList.AddRange(retiredStandards);
            expectedList.Add(retiredStandardWithDistinctLarsCode);
            actualStandards.Should().BeEquivalentTo(expectedList, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_NotYetApproved_Standards_Are_Returned_When_NotYetApprovedFilter_Specified(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.NotYetApproved, false);

            Assert.That(actualStandards, Is.Not.Null);
            var expectedList = new List<Standard>();
            expectedList.AddRange(notYetApprovedStandards);
            actualStandards.Should().BeEquivalentTo(expectedList, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_All_Standards_Are_Returned_When_NoneFilter_Specified(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.None, false);

            Assert.That(actualStandards, Is.Not.Null);
            var expectedList = new List<Standard>();
            expectedList.AddRange(notYetApprovedStandards);
            expectedList.AddRange(activeValidStandards);
            expectedList.AddRange(activeInvalidStandards);
            expectedList.AddRange(withdrawnStandards);
            expectedList.AddRange(retiredStandards);
            actualStandards.Should().BeEquivalentTo(expectedList, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Sector(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            //Act
            var actual = await repository.GetStandards(
                new List<int> { activeValidStandards[0].RouteCode },
                new List<int>(),
                StandardFilter.ActiveAvailable, false);

            //Assert
            Assert.That(actual, Is.Not.Null);
            actual.Should().BeEquivalentTo(new List<Standard> { activeValidStandards[0] }, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Standard_Not_Valid_And_Does_Match_Route_Filter_Then_Standard_Not_Returned(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            //Act
            var actual = await repository.GetStandards(
                new List<int> { activeInvalidStandards[0].RouteCode },
                new List<int>(),
                StandardFilter.ActiveAvailable, false);

            //Assert
            Assert.That(actual, Is.Not.Null);
            actual.Should().BeEquivalentTo(new List<Standard>());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Level(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actual = await repository.GetStandards(
                new List<int>(),
                new List<int> { activeValidStandards[0].Level },
                StandardFilter.ActiveAvailable, false);

            actual.Should().BeEquivalentTo(new List<Standard> { activeValidStandards[0] }, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task When_Filtering_By_Apprenticeship_Type_Then_Only_Apprenticeship_Standards_Are_Returned(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [ValidFoundationApprenticeships] List<Standard> validFoundationApprenticeships,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(validFoundationApprenticeships);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actual = await repository.GetStandards(
                new List<int>(),
                new List<int>(),
                StandardFilter.ActiveAvailable,
                includeAllProperties: false,
                ApprenticeshipType.Apprenticeship);

            actual.Should().BeEquivalentTo(activeValidStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task When_Filtering_By_FoundationApprenticeship_Type_Then_Only_FoundationApprenticeship_Standards_Are_Returned(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [ValidFoundationApprenticeships] List<Standard> validFoundationApprenticeships,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(validFoundationApprenticeships);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actual = await repository.GetStandards(
                new List<int>(),
                new List<int>(),
                StandardFilter.ActiveAvailable,
                false,
                ApprenticeshipType.FoundationApprenticeship);

            actual.Should().BeEquivalentTo(validFoundationApprenticeships, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task When_Not_Filtering_By_ApprenticeshipType_Then_Only_All_Apprenticeships_Are_Returned(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [ValidFoundationApprenticeships] List<Standard> validFoundationApprenticeships,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(validFoundationApprenticeships);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actual = await repository.GetStandards(
                new List<int>(),
                new List<int>(),
                StandardFilter.ActiveAvailable,
                false,
                null);

            actual.Should().BeEquivalentTo(allStandards, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Level_And_Include_Not_Available_To_Start(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            activeInvalidStandards[0].Level = activeValidStandards[0].Level;
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            var actual = await repository.GetStandards(
                new List<int>(),
                new List<int> { activeValidStandards[0].Level },
                StandardFilter.Active, false);

            var expectedList = new List<Standard>
            {
                activeValidStandards[0],
                activeInvalidStandards[0]
            };
            actual.Should().BeEquivalentTo(expectedList, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }
    }
}
