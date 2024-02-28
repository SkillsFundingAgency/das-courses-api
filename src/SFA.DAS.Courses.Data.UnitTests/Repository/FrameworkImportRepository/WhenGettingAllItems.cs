using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkImportRepository
{
    public class WhenGettingAllItems
    {
        private List<FrameworkImport> _frameworkImports;  
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.FrameworkImportRepository _frameworkImportRepository;

        [SetUp]
        public void Arrange()
        {
            _frameworkImports = new List<FrameworkImport>
            {
                new FrameworkImport
                {
                    Id = "1-1-1"
                },
                new FrameworkImport
                {
                    Id = "2-2-2"
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.FrameworksImport).ReturnsDbSet(_frameworkImports);
            
            _frameworkImportRepository = new Data.Repository.FrameworkImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_FrameworkImport_Items_Are_Returned()
        {
            //Act
            var actual = await _frameworkImportRepository.GetAll();
            
            //Assert
            Assert.That(actual, Is.Not.Null);
            actual.Should().BeEquivalentTo(_frameworkImports);
        }
    }
}
