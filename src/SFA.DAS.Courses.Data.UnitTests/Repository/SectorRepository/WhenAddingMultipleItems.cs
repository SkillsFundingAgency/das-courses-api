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
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.SectorRepository _sectorRepository;
        private List<Sector> _sectors;

        [SetUp]
        public void Arrange()
        {
            _sectors = new List<Sector>
            {
                new Sector
                {
                    Id = Guid.NewGuid(),
                    Route = "Test Route"
                },
                new Sector
                {
                    Id = Guid.NewGuid(),
                    Route = "Test Route 2"
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Sectors).ReturnsDbSet(new List<Sector>());
            _sectorRepository = new Data.Repository.SectorRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Sectors_Are_Added()
        {
            //Act
            await _sectorRepository.InsertMany(_sectors);
            
            //Assert
            _coursesDataContext.Verify(x=>x.Sectors.AddRangeAsync(_sectors, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}