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
    public class WhenGettingStandardsByIFateReferenceNumber
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_All_Versions_Of_That_Standard_Are_Returned(
            [StandardsAreLarsValid] List<Standard> activeValidStandards,
            [StandardsNotLarsValid] List<Standard> activeInvalidStandards,
            [StandardsNotYetApproved] List<Standard> notYetApprovedStandards,
            [StandardsWithdrawn] List<Standard> withdrawnStandards,
            [StandardsRetired] List<Standard> retiredStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var iFateReferenceNumber = "ST001";
            var active = activeValidStandards[0];
            active.IfateReferenceNumber = iFateReferenceNumber;
            active.Version = 1.1m;
            active.StandardUId = "ST001_1.1";

            var retired = retiredStandards[0];
            retired.IfateReferenceNumber = iFateReferenceNumber;
            retired.Version = 1.0m;
            retired.StandardUId = "ST001_1.0";


            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidStandards);
            allStandards.AddRange(activeInvalidStandards);
            allStandards.AddRange(notYetApprovedStandards);
            allStandards.AddRange(withdrawnStandards);
            allStandards.AddRange(retiredStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var actualStandards = await repository.GetStandards(iFateReferenceNumber);

            Assert.IsNotNull(actualStandards);
            actualStandards.Should().BeEquivalentTo(new List<Standard> { active, retired });
        }
    }
}
