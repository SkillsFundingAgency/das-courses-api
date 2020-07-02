using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.SectorImportRepository
{
    public class WhenDeletingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<SectorImport> _sectorsImport;
        private Data.Repository.SectorImportRepository _sectorImportRepository;

        [SetUp]
        public void Arrange()
        {
            _sectorsImport = new List<SectorImport>
            {
                new SectorImport
                {
                    Id = Guid.NewGuid()
                },
                new SectorImport
                {
                    Id = Guid.NewGuid()
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.SectorsImport).ReturnsDbSet(_sectorsImport);
            

            _sectorImportRepository = new Data.Repository.SectorImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_SectorsImport_Items_Are_Removed()
        {
            //Act
            _sectorImportRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.SectorsImport.RemoveRange(_coursesDataContext.Object.SectorsImport), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}