using System;
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
            
            var actualStandards = await repository.GetAll(StandardFilter.ActiveAvailable);
            
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

            var actualStandards = await repository.GetAll(StandardFilter.Active);
            
            Assert.IsNotNull(actualStandards);
            var expectedList = new List<Standard>();
            expectedList.AddRange(activeValidStandards);
            expectedList.AddRange(activeInvalidStandards);
            actualStandards.Should().BeEquivalentTo(expectedList);
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

            var actualStandards = await repository.GetAll(StandardFilter.NotYetApproved);

            Assert.IsNotNull(actualStandards);
            var expectedList = new List<Standard>();
            expectedList.AddRange(notYetApprovedStandards);
            actualStandards.Should().BeEquivalentTo(expectedList);
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

            var actualStandards = await repository.GetAll(StandardFilter.None);

            Assert.IsNotNull(actualStandards);
            var expectedList = new List<Standard>();
            expectedList.AddRange(notYetApprovedStandards);
            expectedList.AddRange(activeValidStandards);
            expectedList.AddRange(activeInvalidStandards);
            expectedList.AddRange(withdrawnStandards);
            expectedList.AddRange(retiredStandards);
            actualStandards.Should().BeEquivalentTo(expectedList);
        }
    }
}
