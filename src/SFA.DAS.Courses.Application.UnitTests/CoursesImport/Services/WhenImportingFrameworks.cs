using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
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

            await frameworksImportService.ImportDataIntoStaging();
            
            jsonFileHelper.Verify(x=>x.ParseJsonFile<Framework>(frameworkFile), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_There_Is_No_File_No_Data_Is_Imported(
            [Frozen] Mock<IJsonFileHelper> jsonFileHelper,
            [Frozen] Mock<IFrameworkImportRepository> frameworkImportRepository,
            [Frozen] Mock<IFrameworkFundingImportRepository> frameworkFundingImportRepository,
            FrameworksImportService frameworksImportService)
        {
            jsonFileHelper.Setup(x => x.GetLatestFrameworkFileFromDataDirectory()).Returns(string.Empty);

            var result = await frameworksImportService.ImportDataIntoStaging();

            result.Success.Should().BeFalse();
            result.LatestFile.Should().BeNull();

            frameworkImportRepository.Verify(x => x.DeleteAll(), Times.Never());
            frameworkImportRepository.Verify(x=>x.InsertMany(It.IsAny<IEnumerable<Domain.Entities.FrameworkImport>>()), Times.Never);
            frameworkFundingImportRepository.Verify(x => x.DeleteAll(), Times.Never());
            frameworkFundingImportRepository.Verify(x=>x.InsertMany(It.IsAny<IEnumerable<Domain.Entities.FrameworkFundingImport>>()), Times.Never);
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task Then_If_The_File_Has_Already_Been_Imported_No_Other_Data_Is_Imported(
            string frameworkFile,
            ImportAudit auditFile,
            [Frozen] Mock<IImportAuditRepository> importAuditRepository,
            [Frozen] Mock<IJsonFileHelper> jsonFileHelper,
            [Frozen] Mock<IFrameworkImportRepository> frameworkImportRepository,
            [Frozen] Mock<IFrameworkFundingImportRepository> frameworkFundingImportRepository,
            FrameworksImportService frameworksImportService)
        {
            auditFile.FileName = frameworkFile;
            jsonFileHelper.Setup(x => x.GetLatestFrameworkFileFromDataDirectory()).Returns(frameworkFile);
            importAuditRepository.Setup(x => x.GetLastImportByType(ImportType.FrameworkImport)).ReturnsAsync(auditFile);

            var result = await frameworksImportService.ImportDataIntoStaging();

            result.Success.Should().BeFalse();
            result.LatestFile.Should().BeNull();

            frameworkImportRepository.Verify(x => x.DeleteAll(), Times.Never());
            frameworkImportRepository.Verify(x=>x.InsertMany(It.IsAny<IEnumerable<Domain.Entities.FrameworkImport>>()), Times.Never);
            frameworkFundingImportRepository.Verify(x => x.DeleteAll(), Times.Never());
            frameworkFundingImportRepository.Verify(x=>x.InsertMany(It.IsAny<IEnumerable<Domain.Entities.FrameworkFundingImport>>()), Times.Never);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Import_Tables_Are_Deleted_And_New_Data_Imported_Where_Frameworks_Have_FundingPeriods(
            string frameworkFile,
            List<Framework> frameworks,
            Framework framework,
            [Frozen] Mock<IImportAuditRepository> importAuditRepository,
            [Frozen] Mock<IJsonFileHelper> jsonFileHelper,
            [Frozen] Mock<IFrameworkImportRepository> frameworkImportRepository,
            [Frozen] Mock<IFrameworkFundingImportRepository> frameworkFundingImportRepository,
            FrameworksImportService frameworksImportService)
        {
            framework.FundingPeriods = null;
            frameworks.Add(framework);

            var countOfFundingPeriods = frameworks.Where(f=>f.FundingPeriods != null).SelectMany(f => f.FundingPeriods).Count();

            importAuditRepository.Setup(x => x.GetLastImportByType(ImportType.FrameworkImport)).ReturnsAsync((ImportAudit)null);

            jsonFileHelper.Setup(x => x.GetLatestFrameworkFileFromDataDirectory()).Returns(frameworkFile);
            jsonFileHelper.Setup(x => x.ParseJsonFile<Framework>(It.IsAny<string>())).Returns(frameworks);

            var result = await frameworksImportService.ImportDataIntoStaging();

            result.Success.Should().BeTrue();
            result.LatestFile.Should().Be(frameworkFile);

            frameworkImportRepository.Verify(x=>x.DeleteAll(), Times.Once);
            frameworkFundingImportRepository.Verify(x=>x.DeleteAll(), Times.Once);
            frameworkImportRepository.Verify(x=>x.InsertMany(It.Is<List<FrameworkImport>>(c=>c.Count.Equals(frameworks.Count-1))), Times.Once);
            frameworkFundingImportRepository.Verify(x=>x.InsertMany(It.Is<List<FrameworkFundingImport>>(c=>c.Count.Equals(countOfFundingPeriods))), Times.Once);
        }
    }
}
