using System;
using System.Collections.Generic;
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
    public class WhenLoadingFrameworksFromStaging
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Framework_Tables_Are_Updated_From_The_Import_Tables(
            DateTime importStartTime,
            string importFile,
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

            await frameworksImportService.LoadDataFromStaging(importStartTime, importFile);

            frameworkRepository.Verify(x => x.DeleteAll(), Times.Once);
            frameworkFundingRepository.Verify(x => x.DeleteAll(), Times.Once);
            frameworkRepository.Verify(x => x.InsertMany(It.Is<List<Domain.Entities.Framework>>(c => c.Count.Equals(frameworkImports.Count))));
            frameworkFundingRepository.Verify(x => x.InsertMany(It.Is<List<FrameworkFunding>>(c => c.Count.Equals(frameworkFundingImports.Count))));
        }


        [Test, RecursiveMoqAutoData]
        public async Task Then_An_Audit_Record_Is_Added(
            DateTime importStartTime,
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

            await frameworksImportService.LoadDataFromStaging(importStartTime, frameworkFile);

            importAuditRepository.Verify(x =>
                x.Insert(It.Is<ImportAudit>(
                    c =>
                        c.ImportType.Equals(ImportType.FrameworkImport)
                        && c.FileName.Equals(frameworkFile)
                        && c.RowsImported.Equals(frameworkImports.Count + frameworkFundingImports.Count)
                        )));
        }
    }
}
