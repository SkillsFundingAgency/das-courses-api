using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Services;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Services
{
    [TestFixture]
    public class WhenImportingLarsDataIntoStaging
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_it_gets_the_current_file_path_downloads_the_zip_and_imports_to_staging(
            string filePath,
            string content,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> dataDownloadService,
            [Frozen] Mock<IZipArchiveHelper> zipHelper,
            LarsDataImportService _sut)
        {
            // Arrange
            pageParser
                .Setup(x => x.GetCurrentLarsDataDownloadFilePath())
                .ReturnsAsync(filePath);

            dataDownloadService
                .Setup(x => x.GetFileStream(filePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            SetupEmptyZipExtracts(zipHelper);

            // Act
            var result = await _sut.ImportDataIntoStaging();

            // Assert
            pageParser.Verify(x => x.GetCurrentLarsDataDownloadFilePath(), Times.Once);
            dataDownloadService.Verify(x => x.GetFileStream(filePath), Times.Once);

            Assert.That(result.Success, Is.True);
            Assert.That(result.FileName, Is.EqualTo(filePath));
        }

        [Test, RecursiveMoqAutoData]
        public void Then_it_rethrows_if_staging_import_throws(
            string filePath,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> dataDownloadService,
            LarsDataImportService _sut)
        {
            // Arrange
            pageParser
                .Setup(x => x.GetCurrentLarsDataDownloadFilePath())
                .ReturnsAsync(filePath);

            dataDownloadService
                .Setup(x => x.GetFileStream(filePath))
                .ThrowsAsync(new InvalidOperationException("boom"));

            // Act / Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _sut.ImportDataIntoStaging());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_it_deletes_existing_import_rows_before_loading_new_staging_data_for_all_5_sources(
            string filePath,
            string content,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> dataDownloadService,
            [Frozen] Mock<IZipArchiveHelper> zipHelper,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<IFundingImportRepository> fundingImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier1ImportRepository> sectorSubjectAreaTier1ImportRepository,
            [Frozen] Mock<ICoursesDataContext> coursesDataContext,
            LarsDataImportService _sut)
        {
            // Arrange
            SetupTransaction(coursesDataContext);

            pageParser
                .Setup(x => x.GetCurrentLarsDataDownloadFilePath())
                .ReturnsAsync(filePath);

            dataDownloadService
                .Setup(x => x.GetFileStream(filePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            SetupEmptyZipExtracts(zipHelper);

            // Act
            await _sut.ImportDataIntoStaging();

            // Assert
            larsStandardImportRepository.Verify(x => x.DeleteAll(), Times.Once);
            apprenticeshipFundingImportRepository.Verify(x => x.DeleteAll(), Times.Once);
            fundingImportRepository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier2ImportRepository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier1ImportRepository.Verify(x => x.DeleteAll(), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_it_extracts_the_expected_csv_files_from_the_archive_including_Funding_csv(
            string filePath,
            string content,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> dataDownloadService,
            [Frozen] Mock<IZipArchiveHelper> zipHelper,
            [Frozen] Mock<ICoursesDataContext> coursesDataContext,
            LarsDataImportService _sut)
        {
            // Arrange
            SetupTransaction(coursesDataContext);

            pageParser
                .Setup(x => x.GetCurrentLarsDataDownloadFilePath())
                .ReturnsAsync(filePath);

            dataDownloadService
                .Setup(x => x.GetFileStream(filePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            SetupEmptyZipExtracts(zipHelper);

            // Act
            await _sut.ImportDataIntoStaging();

            // Assert
            zipHelper.Verify(x =>
                x.ExtractModelFromCsvFileZipStream<StandardCsv>(It.IsAny<Stream>(), Constants.LarsStandardsFileName),
                Times.Once);

            zipHelper.Verify(x =>
                x.ExtractModelFromCsvFileZipStream<ApprenticeshipFundingCsv>(It.IsAny<Stream>(), Constants.LarsApprenticeshipFundingFileName),
                Times.Once);

            zipHelper.Verify(x =>
                x.ExtractModelFromCsvFileZipStream<FundingCsv>(It.IsAny<Stream>(), Constants.LarsFundingFileName),
                Times.Once);

            zipHelper.Verify(x =>
                x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier2Csv>(It.IsAny<Stream>(), Constants.LarsSectorSubjectAreaTier2FileName),
                Times.Once);

            zipHelper.Verify(x =>
                x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier1Csv>(It.IsAny<Stream>(), Constants.LarsSectorSubjectAreaTier1FileName),
                Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_it_inserts_imported_rows_for_all_5_staging_sources_and_applies_all_filters(
            string filePath,
            string content,
            List<StandardCsv> standardCsv,
            List<ApprenticeshipFundingCsv> apprenticeshipFundingCsv,
            List<FundingCsv> fundingCsv,
            List<SectorSubjectAreaTier2Csv> sectorSubjectAreaTier2Csv,
            List<SectorSubjectAreaTier1Csv> sectorSubjectAreaTier1Csv,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> dataDownloadService,
            [Frozen] Mock<IZipArchiveHelper> zipHelper,
            [Frozen] Mock<IImportAuditRepository> importAuditRepository,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<IFundingImportRepository> fundingImportRepository,
            [Frozen] Mock<IApprenticeshipFundingRepository> apprenticeshipFundingRepository,
            [Frozen] Mock<ILarsStandardRepository> larsStandardRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2Repository> sectorSubjectAreaTier2Repository,
            [Frozen] Mock<ISectorSubjectAreaTier1ImportRepository> sectorSubjectAreaTier1ImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier1Repository> sectorSubjectAreaTier1Repository,
            [Frozen] Mock<ICoursesDataContext> coursesDataContext,
            [Frozen] Mock<ILogger<LarsDataImportService>> logger)
        {
            // Arrange
            SetupTransaction(coursesDataContext);

            var allowedFundingCategory = "ALLOWED_CATEGORY";

            var options = Options.Create(new CoursesConfiguration
            {
                LarsImportConfiguration = new LarsImportConfiguration(allowedFundingCategory)
            });

            var _sut = new LarsDataImportService(
                pageParser.Object,
                dataDownloadService.Object,
                zipHelper.Object,
                importAuditRepository.Object,
                apprenticeshipFundingImportRepository.Object,
                fundingImportRepository.Object,
                larsStandardImportRepository.Object,
                apprenticeshipFundingRepository.Object,
                larsStandardRepository.Object,
                sectorSubjectAreaTier2ImportRepository.Object,
                sectorSubjectAreaTier2Repository.Object,
                sectorSubjectAreaTier1ImportRepository.Object,
                sectorSubjectAreaTier1Repository.Object,
                coursesDataContext.Object,
                options,
                logger.Object);

            pageParser
                .Setup(x => x.GetCurrentLarsDataDownloadFilePath())
                .ReturnsAsync(filePath);

            dataDownloadService
                .Setup(x => x.GetFileStream(filePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            apprenticeshipFundingCsv.ForEach(x => x.ApprenticeshipType = "STD");
            if (apprenticeshipFundingCsv.Count > 0)
            {
                apprenticeshipFundingCsv[0].ApprenticeshipType = "FRMK";
            }

            fundingCsv.ForEach(x => x.FundingCategory = allowedFundingCategory);
            if (fundingCsv.Count > 0)
            {
                fundingCsv[0].FundingCategory = "NOT_ALLOWED";
            }

            sectorSubjectAreaTier1Csv.ForEach(x => x.SectorSubjectAreaTier1 = "1.00");
            if (sectorSubjectAreaTier1Csv.Count > 0)
            {
                sectorSubjectAreaTier1Csv[0].SectorSubjectAreaTier1 = "1.00-2.00";
            }

            zipHelper
                .Setup(x => x.ExtractModelFromCsvFileZipStream<StandardCsv>(It.IsAny<Stream>(), Constants.LarsStandardsFileName))
                .Returns(standardCsv);

            zipHelper
                .Setup(x => x.ExtractModelFromCsvFileZipStream<ApprenticeshipFundingCsv>(It.IsAny<Stream>(), Constants.LarsApprenticeshipFundingFileName))
                .Returns(apprenticeshipFundingCsv);

            zipHelper
                .Setup(x => x.ExtractModelFromCsvFileZipStream<FundingCsv>(It.IsAny<Stream>(), Constants.LarsFundingFileName))
                .Returns(fundingCsv);

            zipHelper
                .Setup(x => x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier2Csv>(It.IsAny<Stream>(), Constants.LarsSectorSubjectAreaTier2FileName))
                .Returns(sectorSubjectAreaTier2Csv);

            zipHelper
                .Setup(x => x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier1Csv>(It.IsAny<Stream>(), Constants.LarsSectorSubjectAreaTier1FileName))
                .Returns(sectorSubjectAreaTier1Csv);

            var expectedApprenticeshipFundingInserted = apprenticeshipFundingCsv.Count(x =>
                x.ApprenticeshipType.StartsWith("STD", StringComparison.CurrentCultureIgnoreCase));

            var expectedFundingInserted = fundingCsv.Count(x =>
                x.FundingCategory == allowedFundingCategory);

            var expectedTier1Inserted = sectorSubjectAreaTier1Csv.Count(x =>
                x.SectorSubjectAreaTier1 != null && !x.SectorSubjectAreaTier1.Contains('-'));

            // Act
            await _sut.ImportDataIntoStaging();

            // Assert
            larsStandardImportRepository.Verify(x =>
                    x.InsertMany(It.Is<List<LarsStandardImport>>(list => list.Count == standardCsv.Count)),
                Times.Once);

            apprenticeshipFundingImportRepository.Verify(x =>
                    x.InsertMany(It.Is<List<ApprenticeshipFundingImport>>(list => list.Count == expectedApprenticeshipFundingInserted)),
                Times.Once);

            fundingImportRepository.Verify(x =>
                    x.InsertMany(It.Is<List<FundingImport>>(list => list.Count == expectedFundingInserted)),
                Times.Once);

            sectorSubjectAreaTier2ImportRepository.Verify(x =>
                    x.InsertMany(It.Is<List<SectorSubjectAreaTier2Import>>(list => list.Count == sectorSubjectAreaTier2Csv.Count)),
                Times.Once);

            sectorSubjectAreaTier1ImportRepository.Verify(x =>
                    x.InsertMany(It.Is<List<SectorSubjectAreaTier1Import>>(list => list.Count == expectedTier1Inserted)),
                Times.Once);
        }

        private static void SetupTransaction(Mock<ICoursesDataContext> coursesDataContext)
        {
            coursesDataContext
                .Setup(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .Returns<Func<Task>, CancellationToken>(
                    async (operation, _) => await operation());
        }

        private static void SetupEmptyZipExtracts(Mock<IZipArchiveHelper> zipHelper)
        {
            zipHelper
                .Setup(x => x.ExtractModelFromCsvFileZipStream<StandardCsv>(
                    It.IsAny<Stream>(),
                    Constants.LarsStandardsFileName))
                .Returns(new List<StandardCsv>());

            zipHelper
                .Setup(x => x.ExtractModelFromCsvFileZipStream<ApprenticeshipFundingCsv>(
                    It.IsAny<Stream>(),
                    Constants.LarsApprenticeshipFundingFileName))
                .Returns(new List<ApprenticeshipFundingCsv>());

            zipHelper
                .Setup(x => x.ExtractModelFromCsvFileZipStream<FundingCsv>(
                    It.IsAny<Stream>(),
                    Constants.LarsFundingFileName))
                .Returns(new List<FundingCsv>());

            zipHelper
                .Setup(x => x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier2Csv>(
                    It.IsAny<Stream>(),
                    Constants.LarsSectorSubjectAreaTier2FileName))
                .Returns(new List<SectorSubjectAreaTier2Csv>());

            zipHelper
                .Setup(x => x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier1Csv>(
                    It.IsAny<Stream>(),
                    Constants.LarsSectorSubjectAreaTier1FileName))
                .Returns(new List<SectorSubjectAreaTier1Csv>());
        }
    }
}
