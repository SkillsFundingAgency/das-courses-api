using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.RouteRepository
{
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.RouteRepository _routeRepository;
        private List<Route> _routes;

        [SetUp]
        public void Arrange()
        {
            _routes = new List<Route>
            {
                new Route
                {
                    Id = 1,
                    Name = "Test Route"
                },
                new Route
                {
                    Id = 2,
                    Name = "Test Route 2"
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Routes).ReturnsDbSet(new List<Route>());
            _routeRepository = new Data.Repository.RouteRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Routes_Are_Added()
        {
            //Act
            await _routeRepository.InsertMany(_routes);
            
            //Assert
            _coursesDataContext.Verify(x=>x.Routes.AddRangeAsync(_routes, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
