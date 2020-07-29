using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkRepository
{
    public class WhenGettingAllItems
    {
        private List<Framework> _frameworks;  
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.FrameworkRepository _frameworkRepository;

        [SetUp]
        public void Arrange()
        {
            _frameworks = new List<Framework>
            {
                new Framework
                {
                    Id = "1-1-1"
                },
                new Framework
                {
                    Id = "2-2-2"
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Frameworks).ReturnsDbSet(_frameworks);
            
            _frameworkRepository = new Data.Repository.FrameworkRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Framework_Items_Are_Returned()
        {
            //Act
            var sectorsImport = await _frameworkRepository.GetAll();
            
            //Assert
            Assert.IsNotNull(sectorsImport);
            sectorsImport.Should().BeEquivalentTo(_frameworks);
        }
    }
}