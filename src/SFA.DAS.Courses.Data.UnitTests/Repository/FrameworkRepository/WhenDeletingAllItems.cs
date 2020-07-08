using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkRepository
{
    public class WhenDeletingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<Framework> _frameworks;
        private Data.Repository.FrameworkRepository _frameworkRepository;

        [SetUp]
        public void Arrange()
        {
            _frameworks = new List<Framework>
            {
                new Framework
                {
                    Id = "2-2-2"
                },
                new Framework
                {
                    Id = "3-3-3"
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Frameworks).ReturnsDbSet(_frameworks);
            

            _frameworkRepository = new Data.Repository.FrameworkRepository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_Frameworks_Are_Removed()
        {
            //Act
            _frameworkRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.Frameworks.RemoveRange(_coursesDataContext.Object.Frameworks), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}