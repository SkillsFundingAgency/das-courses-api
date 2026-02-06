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
    [TestFixture]
    public class WhenImportingLarsData
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_it_gets_the_current_file_path_and_calls_the_staging_service_import(
            string filePath,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<ILarsImportStagingService> larsImportStagingService,
            LarsImportService _sut)
        {
            // Arrange
            pageParser.Setup(x => x.GetCurrentLarsDataDownloadFilePath()).ReturnsAsync(filePath);

            // Act
            var result = await _sut.ImportDataIntoStaging();

            // Assert
            pageParser.Verify(x => x.GetCurrentLarsDataDownloadFilePath(), Times.Once);
            larsImportStagingService.Verify(x => x.Import(filePath), Times.Once);
            Assert.That(result.Success, Is.True);
            Assert.That(result.FileName, Is.EqualTo(filePath));
        }

        [Test, RecursiveMoqAutoData]
        public void Then_it_rethrows_if_staging_import_throws(
            string filePath,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<ILarsImportStagingService> larsImportStagingService,
            LarsImportService _sut)
        {
            // Arrange
            pageParser.Setup(x => x.GetCurrentLarsDataDownloadFilePath()).ReturnsAsync(filePath);
            larsImportStagingService.Setup(x => x.Import(filePath))
                .ThrowsAsync(new InvalidOperationException("boom"));

            // Act / Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _sut.ImportDataIntoStaging());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_it_loads_from_import_repositories_deletes_live_tables_inserts_live_data_writes_audit_and_merges_funding_sources(
            DateTime importAuditStartTime,
            string filePath,
            List<LarsStandardImport> larsStandardImports,
            List<SectorSubjectAreaTier2Import> sectorSubjectAreaTier2Imports,
            List<SectorSubjectAreaTier1Import> sectorSubjectAreaTier1Imports,
            List<FundingImport> fundingImports,
            List<ApprenticeshipFundingImport> apprenticeshipFundingImports,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier1ImportRepository> sectorSubjectAreaTier1ImportRepository,
            [Frozen] Mock<IFundingImportRepository> fundingImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<IApprenticeshipFundingRepository> apprenticeshipFundingRepository,
            [Frozen] Mock<ILarsStandardRepository> larsStandardRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2Repository> sectorSubjectAreaTier2Repository,
            [Frozen] Mock<ISectorSubjectAreaTier1Repository> sectorSubjectAreaTier1Repository,
            [Frozen] Mock<IImportAuditRepository> importAuditRepository,
            LarsImportService _sut)
        {
            // Arrange - staging reads
            larsStandardImportRepository.Setup(x => x.GetAll()).ReturnsAsync(larsStandardImports);
            sectorSubjectAreaTier2ImportRepository.Setup(x => x.GetAll()).ReturnsAsync(sectorSubjectAreaTier2Imports);
            sectorSubjectAreaTier1ImportRepository.Setup(x => x.GetAll()).ReturnsAsync(sectorSubjectAreaTier1Imports);
            fundingImportRepository.Setup(x => x.GetAll()).ReturnsAsync(fundingImports);
            apprenticeshipFundingImportRepository.Setup(x => x.GetAll()).ReturnsAsync(apprenticeshipFundingImports);

            // Arrange - live deletes (explicitly verified below)
            larsStandardRepository.Setup(x => x.DeleteAll()).Returns(Task.CompletedTask);
            sectorSubjectAreaTier2Repository.Setup(x => x.DeleteAll()).Returns(Task.CompletedTask);
            sectorSubjectAreaTier1Repository.Setup(x => x.DeleteAll()).Returns(Task.CompletedTask);

            // Arrange - live inserts (explicitly verified below)
            larsStandardRepository.Setup(x => x.InsertMany(It.IsAny<IEnumerable<LarsStandard>>())).Returns(Task.CompletedTask);
            sectorSubjectAreaTier2Repository.Setup(x => x.InsertMany(It.IsAny<IEnumerable<SectorSubjectAreaTier2>>())).Returns(Task.CompletedTask);
            sectorSubjectAreaTier1Repository.Setup(x => x.InsertMany(It.IsAny<IEnumerable<SectorSubjectAreaTier1>>())).Returns(Task.CompletedTask);

            // Arrange - audit capture
            ImportAudit capturedAudit = null;
            importAuditRepository
                .Setup(x => x.Insert(It.IsAny<ImportAudit>()))
                .Callback<ImportAudit>(a => capturedAudit = a)
                .Returns(Task.CompletedTask);

            // Capture ApprenticeshipFunding inserts in order (FundingImport first, then ApprenticeshipFundingImport)
            var insertedBatches = new List<List<ApprenticeshipFunding>>();

            var seq = new MockSequence();
            apprenticeshipFundingRepository.InSequence(seq).Setup(x => x.DeleteAll()).Returns(Task.CompletedTask);

            apprenticeshipFundingRepository.InSequence(seq)
                .Setup(x => x.InsertMany(It.IsAny<IEnumerable<ApprenticeshipFunding>>()))
                .Callback<IEnumerable<ApprenticeshipFunding>>(batch => insertedBatches.Add(batch.ToList()))
                .Returns(Task.CompletedTask);

            apprenticeshipFundingRepository.InSequence(seq)
                .Setup(x => x.InsertMany(It.IsAny<IEnumerable<ApprenticeshipFunding>>()))
                .Callback<IEnumerable<ApprenticeshipFunding>>(batch => insertedBatches.Add(batch.ToList()))
                .Returns(Task.CompletedTask);

            // Act
            await _sut.LoadDataFromStaging(importAuditStartTime, filePath);

            // Assert - deletes on ALL live tables
            larsStandardRepository.Verify(x => x.DeleteAll(), Times.Once);
            apprenticeshipFundingRepository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier2Repository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier1Repository.Verify(x => x.DeleteAll(), Times.Once);

            // Assert - inserts into live tables from staging
            larsStandardRepository.Verify(x =>
                x.InsertMany(It.Is<IEnumerable<LarsStandard>>(c => c.Count() == larsStandardImports.Count)),
                Times.Once);

            sectorSubjectAreaTier2Repository.Verify(x =>
                x.InsertMany(It.Is<IEnumerable<SectorSubjectAreaTier2>>(c => c.Count() == sectorSubjectAreaTier2Imports.Count)),
                Times.Once);

            sectorSubjectAreaTier1Repository.Verify(x =>
                x.InsertMany(It.Is<IEnumerable<SectorSubjectAreaTier1>>(c => c.Count() == sectorSubjectAreaTier1Imports.Count)),
                Times.Once);

            // Assert - funding merge behaviour (unambiguous)
            apprenticeshipFundingRepository.Verify(x =>
                x.InsertMany(It.IsAny<IEnumerable<ApprenticeshipFunding>>()),
                Times.Exactly(2));

            Assert.That(insertedBatches.Count, Is.EqualTo(2));

            var firstBatch = insertedBatches[0];
            var secondBatch = insertedBatches[1];

            Assert.That(firstBatch, Is.Not.Empty);
            Assert.That(firstBatch.All(x => x.DurationUnits == "Hours"), Is.True);

            Assert.That(secondBatch, Is.Not.Empty);
            Assert.That(secondBatch.All(x => x.DurationUnits == "Months"), Is.True);
            Assert.That(secondBatch.All(x => x.FundingStream == "Apprenticeship"), Is.True);

            // Assert - audit record uses NEW rowsImported formula:
            // rowsImported = larsStandardImports.Count + (fundingImports.Count + apprenticeshipFundingImports.Count) + sectorSubjectAreaTier2Imports.Count
            Assert.That(capturedAudit, Is.Not.Null);
            Assert.That(capturedAudit.ImportType, Is.EqualTo(ImportType.LarsImport));
            Assert.That(capturedAudit.FileName, Is.EqualTo(filePath));

            var expectedRowsImported =
                larsStandardImports.Count +
                fundingImports.Count +
                apprenticeshipFundingImports.Count +
                sectorSubjectAreaTier2Imports.Count;

            Assert.That(capturedAudit.RowsImported, Is.EqualTo(expectedRowsImported));
        }
    }
}
