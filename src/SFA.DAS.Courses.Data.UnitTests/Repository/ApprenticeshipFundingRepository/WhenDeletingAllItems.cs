using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.ApprenticeshipFundingRepository
{
    public class WhenDeletingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<ApprenticeshipFunding> _apprenticeshipFundingItems;
        private Data.Repository.ApprenticeshipFundingRepository _apprenticeshipFundingRepository;

        [SetUp]
        public void Arrange()
        {
            _apprenticeshipFundingItems = new List<ApprenticeshipFunding>
            {
                new ApprenticeshipFunding
                {
                    Id = Guid.NewGuid()
                },
                new ApprenticeshipFunding
                {
                    Id = Guid.NewGuid()
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.ApprenticeshipFunding).ReturnsDbSet(_apprenticeshipFundingItems);
            

            _apprenticeshipFundingRepository = new Data.Repository.ApprenticeshipFundingRepository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_ApprenticeshipFunding_Items_Are_Removed()
        {
            //Act
            _apprenticeshipFundingRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.ApprenticeshipFunding.RemoveRange(_coursesDataContext.Object.ApprenticeshipFunding), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}