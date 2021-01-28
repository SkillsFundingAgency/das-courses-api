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

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Services
{
    public class WhenLoadingLarsDataFromStaging
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Data_Is_Deleted_From_The_Repository(
            DateTime importStartTime,
            string filePath,
            [Frozen] Mock<ILarsStandardRepository> larsStandardRepository,
            [Frozen] Mock<IApprenticeshipFundingRepository> apprenticeshipFundingRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2Repository> sectorSubjectAreaTier2Repository,
            LarsImportService larsImportService)
        {
            //Act
            await larsImportService.LoadDataFromStaging(importStartTime, filePath);

            //Assert
            larsStandardRepository.Verify(x => x.DeleteAll(), Times.Once);
            apprenticeshipFundingRepository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier2Repository.Verify(x => x.DeleteAll(), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Data_Is_Added_From_The_Import_Repositories(
            DateTime importStartTime,
            string filePath,
            List<LarsStandardImport> larsStandardImports,
            List<ApprenticeshipFundingImport> apprenticeshipFundingImports,
            List<SectorSubjectAreaTier2Import> sectorSubjectAreaTier2Imports,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            [Frozen] Mock<ILarsStandardRepository> larsStandardRepository,
            [Frozen] Mock<IApprenticeshipFundingRepository> apprenticeshipFundingRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2Repository> sectorSubjectAreaTier2Repository,
            LarsImportService larsImportService)
        {
            //Arrange
            larsStandardImportRepository.Setup(x => x.GetAll()).ReturnsAsync(larsStandardImports);
            apprenticeshipFundingImportRepository.Setup(x => x.GetAll()).ReturnsAsync(apprenticeshipFundingImports);
            sectorSubjectAreaTier2ImportRepository.Setup(x => x.GetAll()).ReturnsAsync(sectorSubjectAreaTier2Imports);

            //Act
            await larsImportService.LoadDataFromStaging(importStartTime, filePath);

            //Assert
            larsStandardRepository.Verify(x => x.InsertMany(It.Is<List<LarsStandard>>(c => c.Count.Equals(larsStandardImports.Count))), Times.Once);
            apprenticeshipFundingRepository.Verify(x => x.InsertMany(It.Is<List<ApprenticeshipFunding>>(c => c.Count.Equals(apprenticeshipFundingImports.Count))), Times.Once);
            sectorSubjectAreaTier2Repository.Verify(x => x.InsertMany(It.Is<List<SectorSubjectAreaTier2>>(c => c.Count.Equals(apprenticeshipFundingImports.Count))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_An_Audit_Record_Is_Created(
            DateTime importStartTime,
            string filePath,
            List<LarsStandardImport> larsStandardImports,
            List<ApprenticeshipFundingImport> apprenticeshipFundingImports,
            List<SectorSubjectAreaTier2Import> sectorSubjectAreaTier2Imports,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> service,
            [Frozen] Mock<IImportAuditRepository> importAuditRepository,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            LarsImportService larsImportService)
        {
            larsStandardImportRepository.Setup(x => x.GetAll()).ReturnsAsync(larsStandardImports);
            apprenticeshipFundingImportRepository.Setup(x => x.GetAll()).ReturnsAsync(apprenticeshipFundingImports);
            sectorSubjectAreaTier2ImportRepository.Setup(x => x.GetAll()).ReturnsAsync(sectorSubjectAreaTier2Imports);

            //Act
            await larsImportService.LoadDataFromStaging(importStartTime, filePath);

            //Assert
            var totalRecords = larsStandardImports.Count + apprenticeshipFundingImports.Count + sectorSubjectAreaTier2Imports.Count;
            importAuditRepository.Verify(x => x
                .Insert(It.Is<ImportAudit>(c
                    => c.ImportType.Equals(ImportType.LarsImport)
                      && c.FileName.Equals(filePath)
                      && c.RowsImported.Equals(totalRecords))),
                Times.Once);
        }
    }
}
