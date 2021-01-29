using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardImportRepository
{
    public class WhenDeletingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<StandardImport> _standardsImport;
        private Data.Repository.StandardImportRepository _standardImportRepository;

        [SetUp]
        public void Arrange()
        {
            _standardsImport = new List<StandardImport>
            {
                new StandardImport()
                {
                    LarsCode = 1
                },
                new StandardImport
                {
                    LarsCode = 2
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.StandardsImport).ReturnsDbSet(_standardsImport);
            

            _standardImportRepository = new Data.Repository.StandardImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_StandardsImport_Items_Are_Removed()
        {
            //Act
            _standardImportRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.StandardsImport.RemoveRange(_coursesDataContext.Object.StandardsImport), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}
