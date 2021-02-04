using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingAStandardByStandardUId
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<Standard> _standards;
        private Data.Repository.StandardRepository _standardRepository;
        private const string ExpectedStandardUId = "ST0001_1.0";

        [SetUp]
        public void Arrange()
        {
            _standards = new List<Standard>
            {
                new Standard()
                {
                    StandardUId = "unknown"
                },
                new Standard
                {
                    StandardUId = ExpectedStandardUId
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Standards).ReturnsDbSet(_standards);

            _standardRepository = new Data.Repository.StandardRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Standard_Is_Returned_By_StandardUId()
        {
            //Act
            var standard = await _standardRepository.Get(ExpectedStandardUId);

            //Assert
            Assert.IsNotNull(standard);
            standard.StandardUId.Should().Be(ExpectedStandardUId);
        }

        [Test]
        public void Then_An_Entity_Not_Found_Exception_Is_Thrown()
        {
            //Arrange
            _coursesDataContext.Setup(x => x.Standards).ReturnsDbSet(new List<Standard>());

            //Act Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _standardRepository.Get(ExpectedStandardUId));
        }
    }
}
