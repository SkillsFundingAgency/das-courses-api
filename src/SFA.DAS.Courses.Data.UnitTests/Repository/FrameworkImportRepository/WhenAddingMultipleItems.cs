using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkImportRepository
{
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.FrameworkImportRepository _frameworkImportRepository;
        private List<FrameworkImport> _frameworkImports;

        [SetUp]
        public void Arrange()
        {
            _frameworkImports = new List<FrameworkImport>
            {
                new FrameworkImport
                {
                    Id = "1-1-1",
                    Title="Test"
                },
                new FrameworkImport
                {
                    Id = "2-2-2",
                    Title="Test 2"
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.FrameworksImport).ReturnsDbSet(new List<FrameworkImport>());
            _frameworkImportRepository = new Data.Repository.FrameworkImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_FrameworkImport_Items_Are_Added()
        {
            //Act
            await _frameworkImportRepository.InsertMany(_frameworkImports);
            
            //Assert
            _coursesDataContext.Verify(x=>x.FrameworksImport.AddRangeAsync(_frameworkImports, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
