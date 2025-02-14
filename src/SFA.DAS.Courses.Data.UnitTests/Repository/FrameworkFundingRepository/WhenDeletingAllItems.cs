using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkFundingRepository
{
    public class WhenDeletingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<FrameworkFunding> _frameworkFundingImports;
        private Data.Repository.FrameworkFundingRepository _frameworkFundingRepository;

        [SetUp]
        public void Arrange()
        {
            _frameworkFundingImports = new List<FrameworkFunding>
            {
                new FrameworkFunding
                {
                    FrameworkId = "1-1-1"
                },
                new FrameworkFunding
                {
                    FrameworkId = "2-2-2"
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.FrameworkFunding).ReturnsDbSet(_frameworkFundingImports);
            

            _frameworkFundingRepository = new Data.Repository.FrameworkFundingRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_FrameworkFunding_Items_Are_Removed()
        {
            //Act
            await _frameworkFundingRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.FrameworkFunding.RemoveRange(_coursesDataContext.Object.FrameworkFunding), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
