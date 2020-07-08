using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using Framework = SFA.DAS.Courses.Domain.ImportTypes.Framework;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Services
{
    public class WhenImportingFrameworks
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_File_Is_Read_From_The_Data_Files_Location(
            string frameworkFile,
            [Frozen] Mock<IJsonFileHelper> jsonFileHelper,
            FrameworksImportService frameworksImportService)
        {
            jsonFileHelper.Setup(x => x.GetLatestFrameworkFileFromDataDirectory()).Returns(frameworkFile);

            await frameworksImportService.ImportData();
            
            jsonFileHelper.Verify(x=>x.ParseJsonFile<Framework>(frameworkFile), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_There_Is_No_File_No_Data_Is_Imported(
            [Frozen] Mock<IJsonFileHelper> jsonFileHelper,
            [Frozen] Mock<IFrameworkRepository> frameworkRepository,
            [Frozen] Mock<IFrameworkFundingRepository> frameworkFundingRepository,
            FrameworksImportService frameworksImportService)
        {
            jsonFileHelper.Setup(x => x.GetLatestFrameworkFileFromDataDirectory()).Returns(string.Empty);

            await frameworksImportService.ImportData();

            frameworkRepository.Verify(x => x.DeleteAll(), Times.Never());
            frameworkRepository.Verify(x=>x.InsertMany(It.IsAny<IEnumerable<Domain.Entities.Framework>>()), Times.Never);
            frameworkFundingRepository.Verify(x => x.DeleteAll(), Times.Never());
            frameworkFundingRepository.Verify(x=>x.InsertMany(It.IsAny<IEnumerable<Domain.Entities.FrameworkFunding>>()), Times.Never);
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task Then_If_The_File_Has_Already_Been_Imported_No_Other_Data_Is_Imported(
            string frameworkFile,
            ImportAudit auditFile,
            [Frozen] Mock<IImportAuditRepository> importAuditRepository,
            [Frozen] Mock<IJsonFileHelper> jsonFileHelper,
            [Frozen] Mock<IFrameworkRepository> frameworkRepository,
            [Frozen] Mock<IFrameworkFundingRepository> frameworkFundingRepository,
            FrameworksImportService frameworksImportService)
        {
            auditFile.FileName = frameworkFile;
            jsonFileHelper.Setup(x => x.GetLatestFrameworkFileFromDataDirectory()).Returns(frameworkFile);
            importAuditRepository.Setup(x => x.GetLastImportByType(ImportType.FrameworkImport)).ReturnsAsync(auditFile);
            
            await frameworksImportService.ImportData();

            frameworkRepository.Verify(x => x.DeleteAll(), Times.Never());
            frameworkRepository.Verify(x=>x.InsertMany(It.IsAny<IEnumerable<Domain.Entities.Framework>>()), Times.Never);
            frameworkFundingRepository.Verify(x => x.DeleteAll(), Times.Never());
            frameworkFundingRepository.Verify(x=>x.InsertMany(It.IsAny<IEnumerable<Domain.Entities.FrameworkFunding>>()), Times.Never);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Import_Tables_Are_Deleted_And_New_Data_Imported(
            List<Framework> frameworks,
            [Frozen] Mock<IJsonFileHelper> jsonFileHelper,
            [Frozen] Mock<IFrameworkImportRepository> frameworkImportRepository,
            [Frozen] Mock<IFrameworkFundingImportRepository> frameworkFundingImportRepository,
            FrameworksImportService frameworksImportService)
        {
            jsonFileHelper.Setup(x => x.ParseJsonFile<Framework>(It.IsAny<string>())).Returns(frameworks);

            await frameworksImportService.ImportData();
            
            frameworkImportRepository.Verify(x=>x.DeleteAll(), Times.Once);
            frameworkFundingImportRepository.Verify(x=>x.DeleteAll(), Times.Once);
            frameworkImportRepository.Verify(x=>x.InsertMany(It.Is<List<FrameworkImport>>(c=>c.Count.Equals(frameworks.Count))), Times.Once);
            frameworkFundingImportRepository.Verify(x
                =>x.InsertMany(It.Is<List<FrameworkFundingImport>>(
                    c=>c.Count.Equals(frameworks.Sum(frk=>frk.FundingPeriods.Count)))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Framework_Tables_Are_Updated_From_The_Import_Tables(
            List<Framework> frameworks,
            List<FrameworkImport> frameworkImports,
            List<FrameworkFundingImport> frameworkFundingImports,
            [Frozen] Mock<IJsonFileHelper> jsonFileHelper,
            [Frozen] Mock<IFrameworkImportRepository> frameworkImportRepository,
            [Frozen] Mock<IFrameworkFundingImportRepository> frameworkFundingImportRepository,
            [Frozen] Mock<IFrameworkRepository> frameworkRepository,
            [Frozen] Mock<IFrameworkFundingRepository> frameworkFundingRepository,
            FrameworksImportService frameworksImportService)
        {
            jsonFileHelper.Setup(x => x.ParseJsonFile<Framework>(It.IsAny<string>())).Returns(frameworks);
            frameworkImportRepository.Setup(x => x.GetAll()).ReturnsAsync(frameworkImports);
            frameworkFundingImportRepository.Setup(x => x.GetAll()).ReturnsAsync(frameworkFundingImports);
            
            await frameworksImportService.ImportData();
            
            frameworkRepository.Verify(x=>x.DeleteAll(), Times.Once);
            frameworkFundingRepository.Verify(x=>x.DeleteAll(), Times.Once);
            frameworkRepository.Verify(x=>x.InsertMany(It.Is<List<Domain.Entities.Framework>>(c=>c.Count.Equals(frameworkImports.Count))));
            frameworkFundingRepository.Verify(x=>x.InsertMany(It.Is<List<FrameworkFunding>>(c=>c.Count.Equals(frameworkFundingImports.Count))));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_An_Audit_Record_Is_Added(
            string frameworkFile,
            List<Framework> frameworks,
            List<FrameworkImport> frameworkImports,
            List<FrameworkFundingImport> frameworkFundingImports,
            [Frozen] Mock<IImportAuditRepository> importAuditRepository,
            [Frozen] Mock<IJsonFileHelper> jsonFileHelper,
            [Frozen] Mock<IFrameworkImportRepository> frameworkImportRepository,
            [Frozen] Mock<IFrameworkFundingImportRepository> frameworkFundingImportRepository,
            FrameworksImportService frameworksImportService)
        {
            jsonFileHelper.Setup(x => x.GetLatestFrameworkFileFromDataDirectory()).Returns(frameworkFile);
            jsonFileHelper.Setup(x => x.ParseJsonFile<Framework>(frameworkFile)).Returns(frameworks);
            frameworkImportRepository.Setup(x => x.GetAll()).ReturnsAsync(frameworkImports);
            frameworkFundingImportRepository.Setup(x => x.GetAll()).ReturnsAsync(frameworkFundingImports);

            await frameworksImportService.ImportData();
            
            importAuditRepository.Verify(x=>
                x.Insert(It.Is<ImportAudit>(
                    c=>
                        c.ImportType.Equals(ImportType.FrameworkImport)
                        && c.FileName.Equals(frameworkFile)
                        && c.RowsImported.Equals(frameworkImports.Count + frameworkFundingImports.Count)
                        )));
                
                    
        }
    }
}