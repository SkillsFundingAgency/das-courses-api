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
    public class WhenGettingCount
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Available_Standards(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var count = await repository.Count(StandardFilter.ActiveAvailable);

            count.Should().Be(activeValidStandards.Count);
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Active_Standards(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var count = await repository.Count(StandardFilter.Active);

            count.Should().Be(activeValidStandards.Count + activeInvalidStandards.Count + retiredStandards.Count);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Active_Standards_Including_Retired_With_Distinct_LarsCode(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
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
            allStandards.Add(activeValidStandards.Select(x => new Standard { IfateReferenceNumber = x.IfateReferenceNumber, Status = "Retired", LarsCode = 100001, Version = "1", LarsStandard = newActiveVersion.LarsStandard }).First());

            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var count = await repository.Count(StandardFilter.Active);

            count.Should().Be(activeValidStandards.Count + activeInvalidStandards.Count + retiredStandards.Count + 1);
        }


        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_NotYetApproved_Standards(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var count = await repository.Count(StandardFilter.NotYetApproved);


            count.Should().Be(notYetApprovedStandards.Count);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_All_Standards(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var count = await repository.Count(StandardFilter.None);

            var total = activeInvalidStandards.Count + activeInvalidStandards.Count + notYetApprovedStandards.Count
                + withdrawnStandards.Count + retiredStandards.Count;
            count.Should().Be(total);
        }
    }
}
