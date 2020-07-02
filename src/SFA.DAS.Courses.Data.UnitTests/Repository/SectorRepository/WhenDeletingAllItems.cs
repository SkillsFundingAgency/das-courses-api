using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.SectorRepository
{
    public class WhenDeletingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<Sector> _sectorsImport;
        private Data.Repository.SectorRepository _sectorImportRepository;

        [SetUp]
        public void Arrange()
        {
            _sectorsImport = new List<Sector>
            {
                new Sector
                {
                    Id = Guid.NewGuid()
                },
                new Sector
                {
                    Id = Guid.NewGuid()
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Sectors).ReturnsDbSet(_sectorsImport);
            

            _sectorImportRepository = new Data.Repository.SectorRepository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_Sectors_Are_Removed()
        {
            //Act
            _sectorImportRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.Sectors.RemoveRange(_coursesDataContext.Object.Sectors), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}