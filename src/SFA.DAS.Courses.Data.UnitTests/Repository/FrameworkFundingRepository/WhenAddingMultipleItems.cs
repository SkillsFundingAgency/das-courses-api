using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkFundingRepository
{
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.FrameworkFundingRepository _frameworkFundingRepository;
        private List<FrameworkFunding> _frameworkFunding;

        [SetUp]
        public void Arrange()
        {
            _frameworkFunding = new List<FrameworkFunding>
            {
                new FrameworkFunding
                {
                    FrameworkId =  "1-1-1"
                },
                new FrameworkFunding
                {
                    FrameworkId = "2-2-2"
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.FrameworkFunding).ReturnsDbSet(new List<FrameworkFunding>());
            _frameworkFundingRepository = new Data.Repository.FrameworkFundingRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_FrameworkFunding_Items_Are_Added()
        {
            //Act
            await _frameworkFundingRepository.InsertMany(_frameworkFunding);
            
            //Assert
            _coursesDataContext.Verify(x=>x.FrameworkFunding.AddRangeAsync(_frameworkFunding, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
