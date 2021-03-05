using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.RouteRepository
{
    public class WhenDeletingAllItems
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
            _coursesDataContext.Setup(x => x.Routes).ReturnsDbSet(_routes);
            

            _routeRepository = new Data.Repository.RouteRepository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_Routes_Are_Removed()
        {
            //Act
            _routeRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.Routes.RemoveRange(_coursesDataContext.Object.Routes), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}