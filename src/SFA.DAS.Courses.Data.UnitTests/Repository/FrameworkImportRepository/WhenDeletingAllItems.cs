using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkImportRepository
{
    public class WhenDeletingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<FrameworkImport> _frameworkImports;
        private Data.Repository.FrameworkImportRepository _frameworkImportRepository;

        [SetUp]
        public void Arrange()
        {
            _frameworkImports = new List<FrameworkImport>
            {
                new FrameworkImport
                {
                    Id = "2-2-2"
                },
                new FrameworkImport
                {
                    Id = "3-3-3"
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.FrameworksImport).ReturnsDbSet(_frameworkImports);
            

            _frameworkImportRepository = new Data.Repository.FrameworkImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_FrameworkImports_Are_Removed()
        {
            //Act
            _frameworkImportRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.FrameworksImport.RemoveRange(_coursesDataContext.Object.FrameworksImport), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}