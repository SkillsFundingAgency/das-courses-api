using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkFundingImportRepository
{
    public class WhenDeletingAllItem
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<FrameworkFundingImport> _frameworkFundingImports;
        private Data.Repository.FrameworkFundingImportRepository _frameworkFundingImportRepository;

        [SetUp]
        public void Arrange()
        {
            _frameworkFundingImports = new List<FrameworkFundingImport>
            {
                new FrameworkFundingImport
                {
                    FrameworkId = "1-1-1"
                },
                new FrameworkFundingImport
                {
                    FrameworkId = "2-2-2"
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.FrameworkFundingImport).ReturnsDbSet(_frameworkFundingImports);
            

            _frameworkFundingImportRepository = new Data.Repository.FrameworkFundingImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_FrameworkFundingImport_Items_Are_Removed()
        {
            //Act
            _frameworkFundingImportRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.FrameworkFundingImport.RemoveRange(_coursesDataContext.Object.FrameworkFundingImport), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}