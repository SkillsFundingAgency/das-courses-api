using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingStandards
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<Standard> _standards;
        private Data.Repository.StandardRepository _standardRepository;

        [SetUp]
        public void Arrange()
        {
            _standards = new List<Standard>
            {
                new Standard()
                {
                    Id = 1
                },
                new Standard
                {
                    Id = 2
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Standards).ReturnsDbSet(_standards);
            

            _standardRepository = new Data.Repository.StandardRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Standards_Are_Returned()
        {
            //Act
            var standards = await _standardRepository.GetAll();
            
            //Assert
            Assert.IsNotNull(standards);
            standards.Should().BeEquivalentTo(_standards);
        }

    }
}