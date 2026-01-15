using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                    LarsCode = "1"
                },
                new LarsStandardImport
                {
                    LarsCode = "2"
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.LarsStandardsImport).ReturnsDbSet(_larsStandardImports);
            

            _larsStandardImportRepository = new Data.Repository.LarsStandardImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_LarsStandardImport_Items_Are_Removed()
        {
            //Act
            await _larsStandardImportRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.LarsStandardsImport.RemoveRange(_coursesDataContext.Object.LarsStandardsImport), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
