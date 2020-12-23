using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.Customisations;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingCount
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Available_Standards(
            [StandardsAreLarsValid] List<Standard> validStandards,
            [StandardsNotLarsValid] List<Standard> invalidStandards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(validStandards);
            allStandards.AddRange(invalidStandards);
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var count = await repository.Count();

            count.Should().Be(validStandards.Count);
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_All_Standards(
            [StandardsAreLarsValid] List<Standard> validStandards,
            [StandardsNotLarsValid] List<Standard> invalidStandards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(validStandards);
            allStandards.AddRange(invalidStandards);
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var count = await repository.Count(false);

            
            count.Should().Be(validStandards.Count + invalidStandards.Count);
        }
    }
}
