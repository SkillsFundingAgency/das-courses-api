using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.SectorRepository
{
    public class WhenAddingAnItem
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Sector _sector;
        private Data.Repository.SectorRepository _sectorRepository;
        
        [SetUp]
        public void Arrange()
        {
            _sector = new Sector
            {
                Id = Guid.NewGuid(),
                Route = "Test Route"
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Sectors).ReturnsDbSet(new List<Sector>());
            _sectorRepository = new Data.Repository.SectorRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Sector_Is_Added()
        {
            //Act
            await _sectorRepository.Insert(_sector);
            
            //Assert
            _coursesDataContext.Verify(x=>
                    x.Sectors.AddAsync(
                        It.Is<Sector>(c=>c.Id.Equals(_sector.Id)
                                              && c.Route.Equals(_sector.Route))
                        ,It.IsAny<CancellationToken>())
                , Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}