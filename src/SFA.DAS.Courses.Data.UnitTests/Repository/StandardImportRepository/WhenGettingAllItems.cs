using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardImportRepository
{
    public class WhenGettingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<StandardImport> _standards;
        private Data.Repository.StandardImportRepository _repository;

        [SetUp]
        public void Arrange()
        {
            _standards = new List<StandardImport>
            {
                new StandardImport
                {
                    Id = 1
                },
                new StandardImport
                {
                    Id = 2
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext
                .Setup(x => x.StandardsImport)
                .ReturnsDbSet(_standards);
            

            _repository = new Data.Repository.StandardImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Standards_Are_Returned()
        {
            //Act
            var standards = await _repository.GetAll();
            
            //Assert
            Assert.IsNotNull(standards);
            standards.Should().BeEquivalentTo(_standards);
        }

    }
}