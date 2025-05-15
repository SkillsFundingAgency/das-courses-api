using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.RouteImportRepository
{
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.RouteImportRepository _routeImportRepository;
        private List<RouteImport> _routes;

        [SetUp]
        public void Arrange()
        {
            _routes = new List<RouteImport>
            {
                new RouteImport
                {
                    Id = 1,
                    Name = "Test Route"
                },
                new RouteImport
                {
                    Id = 2,
                    Name = "Test Route 2"
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.RoutesImport).ReturnsDbSet(new List<RouteImport>());
            _routeImportRepository = new Data.Repository.RouteImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_RoutesImport_Are_Added()
        {
            //Act
            await _routeImportRepository.InsertMany(_routes);
            
            //Assert
            _coursesDataContext.Verify(x=>x.RoutesImport.AddRangeAsync(_routes, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
