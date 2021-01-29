using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.LarsStandardImportRepository
{
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.LarsStandardImportRepository _larsStandardImportRepository;
        private List<LarsStandardImport> _larsStandardImports;

        [SetUp]
        public void Arrange()
        {
            _larsStandardImports = new List<LarsStandardImport>
            {
                new LarsStandardImport
                {
                    Id = Guid.NewGuid(),
                    LarsCode = 1
                },
                new LarsStandardImport
                {
                    Id = Guid.NewGuid(),
                    LarsCode = 2
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.LarsStandardsImport).ReturnsDbSet(new List<LarsStandardImport>());
            _larsStandardImportRepository = new Data.Repository.LarsStandardImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_LarsStandardImport_Items_Are_Added()
        {
            //Act
            await _larsStandardImportRepository.InsertMany(_larsStandardImports);
            
            //Assert
            _coursesDataContext.Verify(x=>x.LarsStandardsImport.AddRangeAsync(_larsStandardImports, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}