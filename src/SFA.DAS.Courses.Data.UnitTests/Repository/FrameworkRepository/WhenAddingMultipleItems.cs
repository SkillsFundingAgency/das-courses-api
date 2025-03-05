using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkRepository
{
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.FrameworkRepository _frameworkRepository;
        private List<Framework> _frameworks;

        [SetUp]
        public void Arrange()
        {
            _frameworks = new List<Framework>
            {
                new Framework
                {
                    Id = "1-1-1",
                    Title="Test"
                },
                new Framework
                {
                    Id = "2-2-2",
                    Title="Test 2"
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Frameworks).ReturnsDbSet(new List<Framework>());
            _frameworkRepository = new Data.Repository.FrameworkRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Frameworks_Are_Added()
        {
            //Act
            await _frameworkRepository.InsertMany(_frameworks);
            
            //Assert
            _coursesDataContext.Verify(x=>x.Frameworks.AddRangeAsync(_frameworks, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
