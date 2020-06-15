using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardImportRepository
{
    public class WhenAddingMultipleItems
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
                    Id = 1
                },
                new StandardImport
                {
                    Id = 2
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.StandardsImport).ReturnsDbSet(_standardsImport);
            

            _standardImportRepository = new Data.Repository.StandardImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_StandardsImport_Items_Are_Added()
        {
            //Act
            await _standardImportRepository.InsertMany(_standardsImport);
            
            //Assert
            _coursesDataContext.Verify(x=>x.StandardsImport.AddRangeAsync(_standardsImport, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}