using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.LarsStandardImportRepository
{
    public class WhenDeletingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<LarsStandardImport> _larsStandardImports;
        private Data.Repository.LarsStandardImportRepository _larsStandardImportRepository;

        [SetUp]
        public void Arrange()
        {
            _larsStandardImports = new List<LarsStandardImport>
            {
                new LarsStandardImport
                {
                    Id = Guid.NewGuid()
                },
                new LarsStandardImport
                {
                    Id = Guid.NewGuid()
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.LarsStandardsImport).ReturnsDbSet(_larsStandardImports);
            

            _larsStandardImportRepository = new Data.Repository.LarsStandardImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_LarsStandardImport_Items_Are_Removed()
        {
            //Act
            _larsStandardImportRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.LarsStandardsImport.RemoveRange(_coursesDataContext.Object.LarsStandardsImport), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}