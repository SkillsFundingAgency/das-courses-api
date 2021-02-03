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
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingFilteredStandards
    {
        [Test,RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Sector(
            [StandardsAreLarsValid] List<Standard> validStandards,
            [StandardsNotLarsValid] List<Standard> invalidStandards,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(validStandards);
            allStandards.AddRange(invalidStandards);
            mockCoursesDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);
            
            //Act
            var actual = await repository.GetFilteredStandards(
                new List<Guid> { validStandards[0].RouteId },
                new List<int>(),
                StandardFilter.ActiveAvailable);
            
            //Assert
            Assert.IsNotNull(actual);
            actual.Should().BeEquivalentTo(new List<Standard>{validStandards[0]});
        }

        [Test,RecursiveMoqAutoData]
        public async Task And_Standard_Not_Valid_And_Does_Match_Route_Filter_Then_Standard_Not_Returned(
            [StandardsAreLarsValid] List<Standard> validStandards,
            [StandardsNotLarsValid] List<Standard> invalidStandards,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(validStandards);
            allStandards.AddRange(invalidStandards);
            mockCoursesDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            //Act
            var actual = await repository.GetFilteredStandards(
                new List<Guid>{invalidStandards[0].RouteId}, 
                new List<int>(),
                StandardFilter.ActiveAvailable);
            
            //Assert
            Assert.IsNotNull(actual);
            actual.Should().BeEquivalentTo(new List<Standard>());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Level(
            [StandardsAreLarsValid] List<Standard> validStandards,
            [StandardsNotLarsValid] List<Standard> invalidStandards,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDbContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(validStandards);
            allStandards.AddRange(invalidStandards);
            mockCoursesDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var actual = await repository.GetFilteredStandards(
                new List<Guid>(), 
                new List<int> {validStandards[0].Level},
                StandardFilter.ActiveAvailable);
            
            actual.Should().BeEquivalentTo(new List<Standard>{validStandards[0]});
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Level_And_Include_Not_Available_To_Start(
            [StandardsAreLarsValid] List<Standard> validStandards,
            [StandardsNotLarsValid] List<Standard> invalidStandards,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDbContext,
            Data.Repository.StandardRepository repository)
        {
            invalidStandards[0].Level = validStandards[0].Level;
            var allStandards = new List<Standard>();
            allStandards.AddRange(validStandards);
            allStandards.AddRange(invalidStandards);
            mockCoursesDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var actual = await repository.GetFilteredStandards(
                new List<Guid>(), 
                new List<int> {validStandards[0].Level}, 
                StandardFilter.Active);

            var expectedList = new List<Standard>
            {
                validStandards[0],
                invalidStandards[0]
            };
            actual.Should().BeEquivalentTo(expectedList);
        }
    }
}
