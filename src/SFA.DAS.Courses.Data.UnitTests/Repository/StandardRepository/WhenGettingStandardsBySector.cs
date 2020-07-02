using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingStandardsBySector
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<Standard> _standards;
        private Data.Repository.StandardRepository _standardRepository;
        private List<Guid> _routeIds;
        private Guid _filterGuid;

        [SetUp]
        public void Arrange()
        {
            _filterGuid = Guid.NewGuid();
            _routeIds =new List<Guid> { _filterGuid };
            _standards = new List<Standard>
            {
                new Standard
                {
                    Id = 1,
                    RouteId = _filterGuid
                },
                new Standard
                {
                    Id = 2,
                    RouteId = Guid.NewGuid()
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Standards).ReturnsDbSet(_standards);
            

            _standardRepository = new Data.Repository.StandardRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Standards_Are_Filtered_By_Sector()
        {
            //Act
            var actual = await _standardRepository.GetFilteredStandards(_routeIds);
            
            //Assert
            Assert.IsNotNull(actual);
            actual.Should().BeEquivalentTo(_standards.Where(c=>c.RouteId.Equals(_filterGuid)));
        }
    }
}