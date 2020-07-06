using System;
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
    public class WhenGettingStandards
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Returned(
            [StandardsAreLarsValid] List<Standard> validStandards,
            [StandardsNotLarsValid] List<Standard> invalidStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(validStandards);
            allStandards.AddRange(invalidStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var actualStandards = await repository.GetAll();
            
            Assert.IsNotNull(actualStandards);
            actualStandards.Should().BeEquivalentTo(validStandards);
        }
    }
}
