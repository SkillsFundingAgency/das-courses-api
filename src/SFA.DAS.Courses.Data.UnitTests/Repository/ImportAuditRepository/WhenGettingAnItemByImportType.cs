using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.ImportAuditRepository
{
    public class WhenGettingAnItemByImportType
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<ImportAudit> _importAudits;
        private Data.Repository.ImportAuditRepository _importAuditRepository;
        private  const string ExpectedFileName = "TestFile1";

        [SetUp]
        public void Arrange()
        {
            _importAudits = new List<ImportAudit>
            {
                new ImportAudit(new DateTime(2020,10,01), 100),
                new ImportAudit(new DateTime(2020,09,30), 100, ImportType.LarsImport, "TestFile"),
                new ImportAudit(new DateTime(2020,10,01), 101, ImportType.LarsImport, ExpectedFileName)
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.ImportAudit).ReturnsDbSet(_importAudits);
            

            _importAuditRepository = new Data.Repository.ImportAuditRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Latest_ImportAudit_Record_Is_Returned()
        {
            //Act
            var auditRecord = await _importAuditRepository.GetLastImportByType(ImportType.LarsImport);
            
            //Assert
            Assert.IsNotNull(auditRecord);
            auditRecord.FileName.Should().Be(ExpectedFileName);
        }

        [Test]
        public async Task Then_No_File_Returns_Null()
        {
            //Arrange
            _importAudits = new List<ImportAudit>
            {
                new ImportAudit(new DateTime(2020,10,01), 100)
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.ImportAudit).ReturnsDbSet(_importAudits);
            _importAuditRepository = new Data.Repository.ImportAuditRepository(_coursesDataContext.Object);
            
            //Act
            var auditRecord = await _importAuditRepository.GetLastImportByType(ImportType.LarsImport);
            
            //Assert
            Assert.IsNull(auditRecord);
        }
    }
}