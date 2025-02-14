using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.ImportAuditRepository
{
    public class WhenAddingAnItem
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private ImportAudit _importAudit;
        private Data.Repository.ImportAuditRepository _importAuditRepository;
        private readonly DateTime _expectedDate = new DateTime(2020,10,30);
        private const int ExpectedRowsImported = 100;
        
        [SetUp]
        public void Arrange()
        {
            _importAudit = new ImportAudit(_expectedDate, 100);
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.ImportAudit).ReturnsDbSet(new List<ImportAudit>());
            _importAuditRepository = new Data.Repository.ImportAuditRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Import_Audit_Record_Is_Added()
        {
            //Act
            await _importAuditRepository.Insert(_importAudit);
            
            //Assert
            _coursesDataContext.Verify(x=>
                x.ImportAudit.AddAsync(
                    It.Is<ImportAudit>(c=>c.TimeStarted.Equals(_expectedDate)
                                          && c.RowsImported.Equals(ExpectedRowsImported))
                    ,It.IsAny<CancellationToken>())
                , Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
