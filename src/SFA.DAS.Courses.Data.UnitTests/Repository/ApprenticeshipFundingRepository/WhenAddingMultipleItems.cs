using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.ApprenticeshipFundingRepository
{
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.ApprenticeshipFundingRepository _apprenticeshipFundingRepository;
        private List<ApprenticeshipFunding> _apprenticeshipFunding;

        [SetUp]
        public void Arrange()
        {
            _apprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new ApprenticeshipFunding
                {
                    Id = Guid.NewGuid(),
                },
                new ApprenticeshipFunding
                {
                    Id = Guid.NewGuid(),
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.ApprenticeshipFunding).ReturnsDbSet(new List<ApprenticeshipFunding>());
            _apprenticeshipFundingRepository = new Data.Repository.ApprenticeshipFundingRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_ApprenticeshipFunding_Items_Are_Added()
        {
            //Act
            await _apprenticeshipFundingRepository.InsertMany(_apprenticeshipFunding);
            
            //Assert
            _coursesDataContext.Verify(x=>x.ApprenticeshipFunding.AddRangeAsync(_apprenticeshipFunding, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}
