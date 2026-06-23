using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Services;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Services
{
    [TestFixture]
    public class WhenLoadingLarsDataFromStaging
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_LoadDataFromStaging_Is_Executed_Inside_A_Transaction(
            DateTime importAuditStartTime,
            string filePath,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier1ImportRepository> sectorSubjectAreaTier1ImportRepository,
            [Frozen] Mock<IFundingImportRepository> fundingImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            LarsDataImportService _sut)
        {
            SetupTransaction(mockCoursesDataContext);

            larsStandardImportRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(new List<LarsStandardImport>());

            sectorSubjectAreaTier2ImportRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(new List<SectorSubjectAreaTier2Import>());

            sectorSubjectAreaTier1ImportRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(new List<SectorSubjectAreaTier1Import>());

            fundingImportRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(new List<FundingImport>());

            apprenticeshipFundingImportRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(new List<ApprenticeshipFundingImport>());

            await _sut.LoadDataFromStaging(importAuditStartTime, filePath);

            mockCoursesDataContext.Verify(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
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
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
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
            LarsDataImportService _sut)
        {
            SetupTransaction(mockCoursesDataContext);

            larsStandardImportRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(larsStandardImports);

            sectorSubjectAreaTier2ImportRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(sectorSubjectAreaTier2Imports);

            sectorSubjectAreaTier1ImportRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(sectorSubjectAreaTier1Imports);

            fundingImportRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(fundingImports);

            apprenticeshipFundingImportRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(apprenticeshipFundingImports);

            larsStandardRepository
                .Setup(x => x.DeleteAll())
                .Returns(Task.CompletedTask);

            sectorSubjectAreaTier2Repository
                .Setup(x => x.DeleteAll())
                .Returns(Task.CompletedTask);

            sectorSubjectAreaTier1Repository
                .Setup(x => x.DeleteAll())
                .Returns(Task.CompletedTask);

            larsStandardRepository
                .Setup(x => x.InsertMany(It.IsAny<IEnumerable<LarsStandard>>()))
                .Returns(Task.CompletedTask);

            sectorSubjectAreaTier2Repository
                .Setup(x => x.InsertMany(It.IsAny<IEnumerable<SectorSubjectAreaTier2>>()))
                .Returns(Task.CompletedTask);

            sectorSubjectAreaTier1Repository
                .Setup(x => x.InsertMany(It.IsAny<IEnumerable<SectorSubjectAreaTier1>>()))
                .Returns(Task.CompletedTask);

            ImportAudit capturedAudit = null;

            importAuditRepository
                .Setup(x => x.Insert(It.IsAny<ImportAudit>()))
                .Callback<ImportAudit>(a => capturedAudit = a)
                .Returns(Task.CompletedTask);

            var insertedBatches = new List<List<ApprenticeshipFunding>>();

            var seq = new MockSequence();

            apprenticeshipFundingRepository.InSequence(seq)
                .Setup(x => x.DeleteAll())
                .Returns(Task.CompletedTask);

            apprenticeshipFundingRepository.InSequence(seq)
                .Setup(x => x.InsertMany(It.IsAny<IEnumerable<ApprenticeshipFunding>>()))
                .Callback<IEnumerable<ApprenticeshipFunding>>(batch => insertedBatches.Add(batch.ToList()))
                .Returns(Task.CompletedTask);

            apprenticeshipFundingRepository.InSequence(seq)
                .Setup(x => x.InsertMany(It.IsAny<IEnumerable<ApprenticeshipFunding>>()))
                .Callback<IEnumerable<ApprenticeshipFunding>>(batch => insertedBatches.Add(batch.ToList()))
                .Returns(Task.CompletedTask);

            await _sut.LoadDataFromStaging(importAuditStartTime, filePath);

            mockCoursesDataContext.Verify(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            larsStandardRepository.Verify(x => x.DeleteAll(), Times.Once);
            apprenticeshipFundingRepository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier2Repository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier1Repository.Verify(x => x.DeleteAll(), Times.Once);

            larsStandardRepository.Verify(x =>
                    x.InsertMany(It.Is<IEnumerable<LarsStandard>>(c => c.Count() == larsStandardImports.Count)),
                Times.Once);

            sectorSubjectAreaTier2Repository.Verify(x =>
                    x.InsertMany(It.Is<IEnumerable<SectorSubjectAreaTier2>>(c => c.Count() == sectorSubjectAreaTier2Imports.Count)),
                Times.Once);

            sectorSubjectAreaTier1Repository.Verify(x =>
                    x.InsertMany(It.Is<IEnumerable<SectorSubjectAreaTier1>>(c => c.Count() == sectorSubjectAreaTier1Imports.Count)),
                Times.Once);

            apprenticeshipFundingRepository.Verify(x =>
                    x.InsertMany(It.IsAny<IEnumerable<ApprenticeshipFunding>>()),
                Times.Exactly(2));

            Assert.That(insertedBatches.Count, Is.EqualTo(2));

            var firstBatch = insertedBatches[0];
            var secondBatch = insertedBatches[1];

            Assert.That(firstBatch, Is.Not.Empty);
            Assert.That(firstBatch.All(x => x.DurationUnits == DurationUnits.Hours), Is.True);

            Assert.That(secondBatch, Is.Not.Empty);
            Assert.That(secondBatch.All(x => x.DurationUnits == DurationUnits.Months), Is.True);
            Assert.That(secondBatch.All(x => x.FundingStream == ApprenticeshipType.Apprenticeship.ToString()), Is.True);

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

        [Test, RecursiveMoqAutoData]
        public void Then_it_rethrows_if_load_from_staging_fails(
            DateTime importAuditStartTime,
            string filePath,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            LarsDataImportService _sut)
        {
            mockCoursesDataContext
                .Setup(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("transaction failed"));

            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _sut.LoadDataFromStaging(importAuditStartTime, filePath));
        }

        private static void SetupTransaction(Mock<ICoursesDataContext> mockCoursesDataContext)
        {
            mockCoursesDataContext
                .Setup(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .Returns<Func<Task>, CancellationToken>(
                    async (operation, _) => await operation());
        }
    }
}
