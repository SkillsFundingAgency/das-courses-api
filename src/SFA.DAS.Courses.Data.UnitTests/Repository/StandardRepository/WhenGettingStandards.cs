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
            
            var actualStandards = await repository.GetAll(StandardFilter.ActiveAvailable);
            
            Assert.IsNotNull(actualStandards);
            actualStandards.Should().BeEquivalentTo(validStandards);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_All_Standards_Are_Returned_Including_Not_Available_To_Start_If_Filter_Available_To_Start_Is_False(
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

            var actualStandards = await repository.GetAll(StandardFilter.Active);
            
            Assert.IsNotNull(actualStandards);
            var expectedList = new List<Standard>();
            expectedList.AddRange(validStandards);
            expectedList.AddRange(invalidStandards);
            actualStandards.Should().BeEquivalentTo(expectedList);
        }
    }
}
