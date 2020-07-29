using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkFundingImportRepository
{
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.FrameworkFundingImportRepository _frameworkFundingImportRepository;
        private List<FrameworkFundingImport> _frameworkFundingImports;

        [SetUp]
        public void Arrange()
        {
            _frameworkFundingImports = new List<FrameworkFundingImport>
            {
                new FrameworkFundingImport
                {
                    FrameworkId =  "1-1-1"
                },
                new FrameworkFundingImport
                {
                    FrameworkId = "2-2-2"
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.FrameworkFundingImport).ReturnsDbSet(new List<FrameworkFundingImport>());
            _frameworkFundingImportRepository = new Data.Repository.FrameworkFundingImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_FrameworkFundingImport_Items_Are_Added()
        {
            //Act
            await _frameworkFundingImportRepository.InsertMany(_frameworkFundingImports);
            
            //Assert
            _coursesDataContext.Verify(x=>x.FrameworkFundingImport.AddRangeAsync(_frameworkFundingImports, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}