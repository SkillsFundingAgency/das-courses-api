using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.ApprenticeshipFundingImportRepository
{
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.ApprenticeshipFundingImportRepository _apprenticeshipFundingImportRepository;
        private List<ApprenticeshipFundingImport> _apprenticeshipFundingImports;

        [SetUp]
        public void Arrange()
        {
            _apprenticeshipFundingImports = new List<ApprenticeshipFundingImport>
            {
                new ApprenticeshipFundingImport
                {
                    Id = Guid.NewGuid(),
                    LarsCode = 1
                },
                new ApprenticeshipFundingImport
                {
                    Id = Guid.NewGuid(),
                    LarsCode = 2
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.ApprenticeshipFundingImport).ReturnsDbSet(new List<ApprenticeshipFundingImport>());
            _apprenticeshipFundingImportRepository = new Data.Repository.ApprenticeshipFundingImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_ApprenticeshipFundingImport_Items_Are_Added()
        {
            //Act
            await _apprenticeshipFundingImportRepository.InsertMany(_apprenticeshipFundingImports);
            
            //Assert
            _coursesDataContext.Verify(x=>x.ApprenticeshipFundingImport.AddRangeAsync(_apprenticeshipFundingImports, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}