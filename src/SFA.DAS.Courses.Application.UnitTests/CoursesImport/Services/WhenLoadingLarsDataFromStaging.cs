using System;
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
            [Frozen] Mock<ISectorSubjectAreaTier1Repository> sectorSubjectAreaTier1Repository,
            LarsImportService larsImportService)
        {
            //Act
            await larsImportService.LoadDataFromStaging(importStartTime, filePath);

            //Assert
            larsStandardRepository.Verify(x => x.DeleteAll(), Times.Once);
            apprenticeshipFundingRepository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier2Repository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier1Repository.Verify(x => x.DeleteAll(), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Data_Is_Added_From_The_Import_Repositories(
            DateTime importStartTime,
            string filePath,
            List<LarsStandardImport> larsStandardImports,
            List<SectorSubjectAreaTier2Import> sectorSubjectAreaTier2Imports,
            List<SectorSubjectAreaTier1Import> sectorSubjectAreaTier1Imports,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier1ImportRepository> sectorSubjectAreaTier1ImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier1Repository> sectorSubjectAreaTier1Repository,
            [Frozen] Mock<ILarsStandardRepository> larsStandardRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2Repository> sectorSubjectAreaTier2Repository,
            LarsImportService larsImportService)
        {
            //Arrange
            larsStandardImportRepository.Setup(x => x.GetAll()).ReturnsAsync(larsStandardImports);
            sectorSubjectAreaTier2ImportRepository.Setup(x => x.GetAll()).ReturnsAsync(sectorSubjectAreaTier2Imports);
            sectorSubjectAreaTier1ImportRepository.Setup(x => x.GetAll()).ReturnsAsync(sectorSubjectAreaTier1Imports);

            //Act
            await larsImportService.LoadDataFromStaging(importStartTime, filePath);

            //Assert
            larsStandardRepository.Verify(x => x.InsertMany(It.Is<List<LarsStandard>>(c => c.Count.Equals(larsStandardImports.Count))), Times.Once);
            sectorSubjectAreaTier2Repository.Verify(x => x.InsertMany(It.Is<List<SectorSubjectAreaTier2>>(c => c.Count.Equals(sectorSubjectAreaTier2Imports.Count))), Times.Once);
            sectorSubjectAreaTier1Repository.Verify(x => x.InsertMany(It.Is<List<SectorSubjectAreaTier1>>(c => c.Count.Equals(sectorSubjectAreaTier1Imports.Count))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_FundingData_Is_Added_From_The_Import_Repository_Along_With_StandardUId(
            DateTime importStartTime,
            string filePath,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<IApprenticeshipFundingRepository> apprenticeshipFundingRepository,
            LarsImportService larsImportService)
        {
            //Arrange
            var standardImports = new List<StandardImport>
            {
                new StandardImport { LarsCode = "1", StandardUId = "ST0001_1.0" },
                new StandardImport { LarsCode = "2", StandardUId = "ST0002_1.0" },
                new StandardImport { LarsCode = "2", StandardUId = "ST0002_1.1" }
            };
            standardImportRepository.Setup(s => s.GetAll()).ReturnsAsync(standardImports);

            var apprenticeshipFundingImports = new List<ApprenticeshipFundingImport>
            {
                new ApprenticeshipFundingImport { LarsCode = 1, Id = Guid.NewGuid() },
                new ApprenticeshipFundingImport { LarsCode = 1, Id = Guid.NewGuid() },
                new ApprenticeshipFundingImport { LarsCode = 2, Id = Guid.NewGuid() },
                new ApprenticeshipFundingImport { LarsCode = 2, Id = Guid.NewGuid() }
            };
            apprenticeshipFundingImportRepository.Setup(x => x.GetAll()).ReturnsAsync(apprenticeshipFundingImports);

            //Act
            await larsImportService.LoadDataFromStaging(importStartTime, filePath);

            //Assert
            standardImportRepository.Verify(x => x.GetAll(), Times.Once);
            apprenticeshipFundingRepository.Verify(x => x.InsertMany(It.Is<List<ApprenticeshipFunding>>(c => c.Count.Equals(6))), Times.Once);
            apprenticeshipFundingRepository.Verify(x => x.InsertMany(It.Is<List<ApprenticeshipFunding>>(c => c.Where(s => s.LarsCode == "1234").Count().Equals(2))), Times.Once);
            apprenticeshipFundingRepository.Verify(x => x.InsertMany(It.Is<List<ApprenticeshipFunding>>(c => c.Where(s => s.LarsCode == "2345").Count().Equals(2))), Times.Once);
            apprenticeshipFundingRepository.Verify(x => x.InsertMany(It.Is<List<ApprenticeshipFunding>>(c => c.Where(s => s.LarsCode == "3456").Count().Equals(2))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_FundingData_Is_Added_From_The_Import_Repository_For_Matching_LarsCode_In_Standard(
            DateTime importStartTime,
            string filePath,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<IApprenticeshipFundingRepository> apprenticeshipFundingRepository,
            LarsImportService larsImportService)
        {
            //Arrange
            var standardImports = new List<StandardImport>
            {
                new StandardImport { LarsCode = "1", StandardUId = "ST0001_1.0" },
                new StandardImport { LarsCode = "2", StandardUId = "ST0002_1.1" }
            };
            standardImportRepository.Setup(s => s.GetAll()).ReturnsAsync(standardImports);

            var apprenticeshipFundingImports = new List<ApprenticeshipFundingImport>
            {
                new ApprenticeshipFundingImport { LarsCode = 1, Id = Guid.NewGuid() },
                new ApprenticeshipFundingImport { LarsCode = 1, Id = Guid.NewGuid() },
            };
            apprenticeshipFundingImportRepository.Setup(x => x.GetAll()).ReturnsAsync(apprenticeshipFundingImports);

            //Act
            await larsImportService.LoadDataFromStaging(importStartTime, filePath);

            //Assert
            standardImportRepository.Verify(x => x.GetAll(), Times.Once);
            apprenticeshipFundingRepository.Verify(x => x.InsertMany(It.Is<List<ApprenticeshipFunding>>(c => c.Count.Equals(2))), Times.Once);
            apprenticeshipFundingRepository.Verify(x => x.InsertMany(It.Is<List<ApprenticeshipFunding>>(c => c.Where(s => s.LarsCode == "1234").Count().Equals(2))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_An_Audit_Record_Is_Created(
            DateTime importStartTime,
            string filePath,
            List<LarsStandardImport> larsStandardImports,
            List<StandardImport> standards,
            List<ApprenticeshipFundingImport> apprenticeshipFundingImports,
            List<SectorSubjectAreaTier2Import> sectorSubjectAreaTier2Imports,
            [Frozen] Mock<IImportAuditRepository> importAuditRepository,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            LarsImportService larsImportService)
        {
            standards.ForEach(s => s.LarsCode = "1");
            apprenticeshipFundingImports.ForEach(i => i.LarsCode = 1);
            larsStandardImportRepository.Setup(x => x.GetAll()).ReturnsAsync(larsStandardImports);
            apprenticeshipFundingImportRepository.Setup(x => x.GetAll()).ReturnsAsync(apprenticeshipFundingImports);
            sectorSubjectAreaTier2ImportRepository.Setup(x => x.GetAll()).ReturnsAsync(sectorSubjectAreaTier2Imports);
            standardImportRepository.Setup(x => x.GetAll()).ReturnsAsync(standards);

            //Act
            await larsImportService.LoadDataFromStaging(importStartTime, filePath);

            //Assert
            var totalRecords = larsStandardImports.Count + (apprenticeshipFundingImports.Count * standards.Count) + sectorSubjectAreaTier2Imports.Count;
            importAuditRepository.Verify(x => x
                .Insert(It.Is<ImportAudit>(c
                    => c.ImportType.Equals(ImportType.LarsImport)
                      && c.FileName.Equals(filePath)
                      && c.RowsImported.Equals(totalRecords))),
                Times.Once);
        }
    }
}
