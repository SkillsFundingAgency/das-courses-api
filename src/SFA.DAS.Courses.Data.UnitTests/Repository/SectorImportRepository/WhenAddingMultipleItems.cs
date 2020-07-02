using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.SectorImportRepository
{
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.SectorImportRepository _sectorImportRepository;
        private List<SectorImport> _sectors;

        [SetUp]
        public void Arrange()
        {
            _sectors = new List<SectorImport>
            {
                new SectorImport
                {
                    Id = Guid.NewGuid(),
                    Route = "Test Route"
                },
                new SectorImport
                {
                    Id = Guid.NewGuid(),
                    Route = "Test Route 2"
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.SectorsImport).ReturnsDbSet(new List<SectorImport>());
            _sectorImportRepository = new Data.Repository.SectorImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_SectorsImport_Are_Added()
        {
            //Act
            await _sectorImportRepository.InsertMany(_sectors);
            
            //Assert
            _coursesDataContext.Verify(x=>x.SectorsImport.AddRangeAsync(_sectors, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}