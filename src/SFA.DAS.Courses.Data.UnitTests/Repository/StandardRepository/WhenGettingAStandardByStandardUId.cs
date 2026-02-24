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
        private const string OtherStandardUId = "ST0010_2.0";

        [SetUp]
        public void Arrange()
        {
            _standards = new List<Standard>
            {
                new Standard()
                {
                    StandardUId = OtherStandardUId
                },
                new Standard
                {
                    ApprenticeshipType = ApprenticeshipType.Apprenticeship,
                    CourseType = CourseType.Apprenticeship,
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
            // Act
            var standard = await _standardRepository.Get(ExpectedStandardUId, CourseType.Apprenticeship);

            // Assert
            Assert.That(standard, Is.Not.Null);
            standard.StandardUId.Should().Be(ExpectedStandardUId);
        }

        [Test]
        public async Task Then_Null_Is_Returned()
        {
            // Arrange
            _coursesDataContext.Setup(x => x.Standards).ReturnsDbSet(new List<Standard>());

            // Act & Assert
            var result = await _standardRepository.Get(ExpectedStandardUId, CourseType.Apprenticeship);
            result.Should().BeNull();
        }
    }
}
