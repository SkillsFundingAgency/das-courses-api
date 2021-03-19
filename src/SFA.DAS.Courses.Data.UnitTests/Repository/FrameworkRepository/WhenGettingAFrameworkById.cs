using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkRepository
{
    public class WhenGettingAFrameworkById
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<Framework> _frameworks;
        private Data.Repository.FrameworkRepository _frameworkRepository;
        private const string ExpectedFrameworkId = "2-2-2";

        [SetUp]
        public void Arrange()
        {
            _frameworks = new List<Framework>
            {
                new Framework()
                {
                    Id = "1-1-1"
                },
                new Framework
                {
                    Id = ExpectedFrameworkId
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Frameworks).ReturnsDbSet(_frameworks);

            _frameworkRepository = new Data.Repository.FrameworkRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Framework_Is_Returned_By_Id()
        {
            //Act
            var standards = await _frameworkRepository.Get(ExpectedFrameworkId);
            
            //Assert
            Assert.IsNotNull(standards);
            standards.Should().BeEquivalentTo(_frameworks.SingleOrDefault(c=>c.Id.Equals(ExpectedFrameworkId)));
        }
    }
}
