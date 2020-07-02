using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingStandardsBySector
    {
        [Test,RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Sector(
            List<Standard> standardsInDb,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDbContext,
            Data.Repository.StandardRepository repository)
        {
            mockCoursesDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(standardsInDb);

            //Act
            var actual = await repository.GetFilteredStandards(new List<Guid>{standardsInDb[0].RouteId}, new List<int>());
            
            //Assert
            Assert.IsNotNull(actual);
            actual.Should().BeEquivalentTo(new List<Standard>{standardsInDb[0]});
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Filtered_By_Level(
            List<Standard> standardsInDb,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDbContext,
            Data.Repository.StandardRepository repository)
        {
            mockCoursesDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(standardsInDb);

            var actual = await repository.GetFilteredStandards(
                new List<Guid>(), 
                new List<int> {standardsInDb[0].Level});
            
            actual.Should().BeEquivalentTo(new List<Standard>{standardsInDb[0]});
        }

        //todo: order test
    }
}
