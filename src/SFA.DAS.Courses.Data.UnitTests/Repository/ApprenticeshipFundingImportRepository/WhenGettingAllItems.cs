using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.ApprenticeshipFundingImportRepository
{
    public class WhenGettingAllItems
    {
        private List<ApprenticeshipFundingImport> _apprenticeshipFundingImports;  
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.ApprenticeshipFundingImportRepository _apprenticeshipFundingImportRepository;

        [SetUp]
        public void Arrange()
        {
            _apprenticeshipFundingImports = new List<ApprenticeshipFundingImport>
            {
                new ApprenticeshipFundingImport
                {
                    Id = Guid.NewGuid()
                },
                new ApprenticeshipFundingImport
                {
                    Id = Guid.NewGuid()
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.ApprenticeshipFundingImport).ReturnsDbSet(_apprenticeshipFundingImports);
            
            _apprenticeshipFundingImportRepository = new Data.Repository.ApprenticeshipFundingImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_ApprenticeshipFundingImport_Items_Are_Returned()
        {
            //Act
            var sectorsImport = await _apprenticeshipFundingImportRepository.GetAll();
            
            //Assert
            Assert.IsNotNull(sectorsImport);
            sectorsImport.Should().BeEquivalentTo(_apprenticeshipFundingImports);
        }
    }
}