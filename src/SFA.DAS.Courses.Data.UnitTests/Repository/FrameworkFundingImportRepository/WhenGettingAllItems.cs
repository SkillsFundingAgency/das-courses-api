using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.FrameworkFundingImportRepository
{
    public class WhenGettingAllItems
    {
        private List<FrameworkFundingImport> _frameworkFundingImports;  
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.FrameworkFundingImportRepository _frameworkFundingImportRepository;

        [SetUp]
        public void Arrange()
        {
            _frameworkFundingImports = new List<FrameworkFundingImport>
            {
                new FrameworkFundingImport
                {
                    Id = 1
                },
                new FrameworkFundingImport
                {
                    Id = 2
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.FrameworkFundingImport).ReturnsDbSet(_frameworkFundingImports);
            
            _frameworkFundingImportRepository = new Data.Repository.FrameworkFundingImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_FrameworkFundingImport_Items_Are_Returned()
        {
            //Act
            var actual = await _frameworkFundingImportRepository.GetAll();
            
            //Assert
            Assert.IsNotNull(actual);
            actual.Should().BeEquivalentTo(_frameworkFundingImports);
        }
    }
}