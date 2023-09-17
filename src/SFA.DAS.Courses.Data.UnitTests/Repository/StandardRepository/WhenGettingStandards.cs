using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Equivalency;
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
        public async Task Then_The_Available_Standards_Are_Returned_When_Filter_Set_To_ActiveAvailable(
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
            
            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.ActiveAvailable, false);
            
            Assert.IsNotNull(actualStandards);
            actualStandards.Should().BeEquivalentTo(activeValidStandards,EquivalentCheckExcludes());
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Available_Standards_Are_Returned_When_Filter_Set_To_ActiveAvailable_For_Export(
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
            
            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.ActiveAvailable, true);
            
            Assert.IsNotNull(actualStandards);
            actualStandards.Should().BeEquivalentTo(activeValidStandards);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Active_Standards_Are_Returned_Including_Not_Available_To_Start_When_Filter_Set_To_Active(
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

            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.Active, false);
            
            Assert.IsNotNull(actualStandards);
            var expectedList = new List<Standard>();
            expectedList.AddRange(activeValidStandards);
            expectedList.AddRange(activeInvalidStandards);
            expectedList.AddRange(retiredStandards);
            actualStandards.Should().BeEquivalentTo(expectedList, EquivalentCheckExcludes());
        }

        [Test, RecursiveMoqAutoData] 
        public async Task Then_Active_Standards_Are_Returned_Including_Retired_With_Distinct_LarsCode(
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
            newActiveVersion.LarsCode = 100002;

            //add a retired version to have same IfateReferenceNumber and different LarsCode
            var retiredStandardWithDistinctLarsCode = activeValidStandards.Select(x => new Standard { IfateReferenceNumber = x.IfateReferenceNumber, Status = "Retired", LarsCode = 100001, Version = "1", LarsStandard = newActiveVersion.LarsStandard }).First();
            allStandards.Add(retiredStandardWithDistinctLarsCode);

            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.Active, false);

            Assert.IsNotNull(actualStandards);
            var expectedList = new List<Standard>();
            expectedList.AddRange(activeValidStandards);
            expectedList.AddRange(activeInvalidStandards);
            expectedList.AddRange(retiredStandards);
            expectedList.Add(retiredStandardWithDistinctLarsCode);
            actualStandards.Should().BeEquivalentTo(expectedList,EquivalentCheckExcludes());
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

            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.NotYetApproved, false);

            Assert.IsNotNull(actualStandards);
            var expectedList = new List<Standard>();
            expectedList.AddRange(notYetApprovedStandards);
            actualStandards.Should().BeEquivalentTo(expectedList, EquivalentCheckExcludes());
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

            var actualStandards = await repository.GetStandards(new List<int>(), new List<int>(), StandardFilter.None, false);

            Assert.IsNotNull(actualStandards);
            var expectedList = new List<Standard>();
            expectedList.AddRange(notYetApprovedStandards);
            expectedList.AddRange(activeValidStandards);
            expectedList.AddRange(activeInvalidStandards);
            expectedList.AddRange(withdrawnStandards);
            expectedList.AddRange(retiredStandards);
            actualStandards.Should().BeEquivalentTo(expectedList, EquivalentCheckExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Sector(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockCoursesDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            //Act
            var actual = await repository.GetStandards(
                new List<int> { activeValidStandards[0].RouteCode },
                new List<int>(),
                StandardFilter.ActiveAvailable, false);

            //Assert
            Assert.IsNotNull(actual);
            actual.Should().BeEquivalentTo(new List<Standard> { activeValidStandards[0] },EquivalentCheckExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Standard_Not_Valid_And_Does_Match_Route_Filter_Then_Standard_Not_Returned(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockCoursesDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            //Act
            var actual = await repository.GetStandards(
                new List<int> { activeInvalidStandards[0].RouteCode },
                new List<int>(),
                StandardFilter.ActiveAvailable, false);

            //Assert
            Assert.IsNotNull(actual);
            actual.Should().BeEquivalentTo(new List<Standard>());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Level(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockCoursesDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var actual = await repository.GetStandards(
                new List<int>(),
                new List<int> { activeValidStandards[0].Level },
                StandardFilter.ActiveAvailable, false);

            actual.Should().BeEquivalentTo(new List<Standard> { activeValidStandards[0] }, EquivalentCheckExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Level_And_Include_Not_Available_To_Start(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDbContext,
            Data.Repository.StandardRepository repository)
        {
            activeInvalidStandards[0].Level = activeValidStandards[0].Level;
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockCoursesDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var actual = await repository.GetStandards(
                new List<int>(),
                new List<int> { activeValidStandards[0].Level },
                StandardFilter.Active, false);

            var expectedList = new List<Standard>
            {
                activeValidStandards[0],
                activeInvalidStandards[0]
            };
            actual.Should().BeEquivalentTo(expectedList, EquivalentCheckExcludes());
        }

        private static Func<EquivalencyAssertionOptions<Standard>, EquivalencyAssertionOptions<Standard>> EquivalentCheckExcludes()
        {
            return options=>options
                .Excluding(c=>c.SearchScore)
                .Excluding(c=>c.ProposedTypicalDuration)
                .Excluding(c=>c.ProposedMaxFunding)
                .Excluding(c=>c.OverviewOfRole)
                .Excluding(c=>c.AssessmentPlanUrl)
                .Excluding(c=>c.TrailBlazerContact)
                .Excluding(c=>c.EqaProviderName)
                .Excluding(c=>c.EqaProviderContactEmail)
                .Excluding(c=>c.EqaProviderContactName)
                .Excluding(c=>c.EqaProviderWebLink)
                .Excluding(c=>c.Duties)
                .Excluding(c=>c.CoreDuties)
                .Excluding(c=>c.Options)
                .Excluding(c=>c.CoreAndOptions)
                .Excluding(c=>c.EPAChanged);
        }
    }
}
