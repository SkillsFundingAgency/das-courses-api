using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.ApprenticeshipFundingImportRepository
{
    public class WhenDeletingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<ApprenticeshipFundingImport> _sectorsImport;
        private Data.Repository.ApprenticeshipFundingImportRepository _apprenticeshipFundingImportRepository;

        [SetUp]
        public void Arrange()
        {
            _sectorsImport = new List<ApprenticeshipFundingImport>
            {
                new ApprenticeshipFundingImport
                {
                    Id = Guid.NewGuid()
                },
                new ApprenticeshipFundingImport
                {
                    Id = Guid.NewGuid()
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.ApprenticeshipFundingImport).ReturnsDbSet(_sectorsImport);
            

            _apprenticeshipFundingImportRepository = new Data.Repository.ApprenticeshipFundingImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_ApprenticeshipFundingImport_Items_Are_Removed()
        {
            //Act
            _apprenticeshipFundingImportRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.ApprenticeshipFundingImport.RemoveRange(_coursesDataContext.Object.ApprenticeshipFundingImport), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}