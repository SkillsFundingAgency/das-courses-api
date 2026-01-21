using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Services;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Services
{
    [TestFixture]
    public class WhenLoadingLarsDataFromStaging
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_it_deletes_existing_import_rows_before_loading_new_data_for_all_5_sources(
            string filePath,
            string content,
            [Frozen] Mock<IDataDownloadService> dataDownloadService,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<IFundingImportRepository> fundingImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier1ImportRepository> sectorSubjectAreaTier1ImportRepository,
            LarsImportStagingService _sut)
        {
            // Arrange
            dataDownloadService.Setup(x => x.GetFileStream(filePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            // Act
            await _sut.Import(filePath);

            // Assert
            larsStandardImportRepository.Verify(x => x.DeleteAll(), Times.Once);
            apprenticeshipFundingImportRepository.Verify(x => x.DeleteAll(), Times.Once);
            fundingImportRepository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier2ImportRepository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier1ImportRepository.Verify(x => x.DeleteAll(), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_it_downloads_the_zip_stream_from_the_data_download_service(
            string filePath,
            string content,
            [Frozen] Mock<IDataDownloadService> dataDownloadService,
            LarsImportStagingService _sut)
        {
            // Arrange
            dataDownloadService.Setup(x => x.GetFileStream(filePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            // Act
            await _sut.Import(filePath);

            // Assert
            dataDownloadService.Verify(x => x.GetFileStream(filePath), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_it_extracts_the_expected_csv_files_from_the_archive_including_Funding_csv(
            string filePath,
            string content,
            [Frozen] Mock<IDataDownloadService> dataDownloadService,
            [Frozen] Mock<IZipArchiveHelper> zipHelper,
            LarsImportStagingService _sut)
        {
            // Arrange
            dataDownloadService.Setup(x => x.GetFileStream(filePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            // Act
            await _sut.Import(filePath);

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
        public async Task Then_it_inserts_imported_rows_for_all_5_sources_and_applies_all_filters_including_configured_funding_category(
            string filePath,
            string content,
            List<StandardCsv> standardCsv,
            List<ApprenticeshipFundingCsv> apprenticeshipFundingCsv,
            List<FundingCsv> fundingCsv,
            List<SectorSubjectAreaTier2Csv> sectorSubjectAreaTier2Csv,
            List<SectorSubjectAreaTier1Csv> sectorSubjectAreaTier1Csv,
            [Frozen] Mock<IDataDownloadService> dataDownloadService,
            [Frozen] Mock<IZipArchiveHelper> zipHelper,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<IFundingImportRepository> fundingImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier1ImportRepository> sectorSubjectAreaTier1ImportRepository,
            [Frozen] Mock<ILogger<LarsImportService>> logger)
        {
            // Arrange
            dataDownloadService.Setup(x => x.GetFileStream(filePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            var allowedFundingCategory = "ALLOWED_CATEGORY";

            // IMPORTANT: options must be created before sut construction because ctor reads config.Value
            var options = Options.Create(new CoursesConfiguration
            {
                LarsImportConfiguration = new LarsImportConfiguration(allowedFundingCategory)
            });

            var sut = new LarsImportStagingService(
                dataDownloadService.Object,
                zipHelper.Object,
                apprenticeshipFundingImportRepository.Object,
                fundingImportRepository.Object,
                larsStandardImportRepository.Object,
                sectorSubjectAreaTier2ImportRepository.Object,
                sectorSubjectAreaTier1ImportRepository.Object,
                options,
                logger.Object);

            // Apprenticeship funding filter: keep STD*, drop others
            apprenticeshipFundingCsv.ForEach(x => x.ApprenticeshipType = "STD");
            if (apprenticeshipFundingCsv.Count > 0)
            {
                apprenticeshipFundingCsv[0].ApprenticeshipType = "FRMK";
            }

            // Funding category filter: keep only matching category
            fundingCsv.ForEach(x => x.FundingCategory = allowedFundingCategory);
            if (fundingCsv.Count > 0)
            {
                fundingCsv[0].FundingCategory = "NOT_ALLOWED";
            }

            // SSA Tier1 filter: remove those containing '-'
            sectorSubjectAreaTier1Csv.ForEach(x => x.SectorSubjectAreaTier1 = "1.00");
            if (sectorSubjectAreaTier1Csv.Count > 0)
            {
                sectorSubjectAreaTier1Csv[0].SectorSubjectAreaTier1 = "1.00-2.00";
            }

            zipHelper.Setup(x =>
                    x.ExtractModelFromCsvFileZipStream<StandardCsv>(It.IsAny<Stream>(), Constants.LarsStandardsFileName))
                .Returns(standardCsv);

            zipHelper.Setup(x =>
                    x.ExtractModelFromCsvFileZipStream<ApprenticeshipFundingCsv>(It.IsAny<Stream>(), Constants.LarsApprenticeshipFundingFileName))
                .Returns(apprenticeshipFundingCsv);

            zipHelper.Setup(x =>
                    x.ExtractModelFromCsvFileZipStream<FundingCsv>(It.IsAny<Stream>(), Constants.LarsFundingFileName))
                .Returns(fundingCsv);

            zipHelper.Setup(x =>
                    x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier2Csv>(It.IsAny<Stream>(), Constants.LarsSectorSubjectAreaTier2FileName))
                .Returns(sectorSubjectAreaTier2Csv);

            zipHelper.Setup(x =>
                    x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier1Csv>(It.IsAny<Stream>(), Constants.LarsSectorSubjectAreaTier1FileName))
                .Returns(sectorSubjectAreaTier1Csv);

            var expectedApprenticeshipFundingInserted = apprenticeshipFundingCsv.Count(x =>
                x.ApprenticeshipType.StartsWith("STD", StringComparison.CurrentCultureIgnoreCase));

            var expectedFundingInserted = fundingCsv.Count(x => x.FundingCategory == allowedFundingCategory);

            var expectedTier1Inserted = sectorSubjectAreaTier1Csv.Count(x =>
                x.SectorSubjectAreaTier1 != null && !x.SectorSubjectAreaTier1.Contains('-'));

            // Act
            await sut.Import(filePath);

            // Assert - standards (no filter)
            larsStandardImportRepository.Verify(x =>
                x.InsertMany(It.Is<List<LarsStandardImport>>(list => list.Count == standardCsv.Count)),
                Times.Once);

            // Assert - apprenticeship funding filtered to STD*
            apprenticeshipFundingImportRepository.Verify(x =>
                x.InsertMany(It.Is<List<ApprenticeshipFundingImport>>(list => list.Count == expectedApprenticeshipFundingInserted)),
                Times.Once);

            // Assert - funding filtered by configured category
            fundingImportRepository.Verify(x =>
                x.InsertMany(It.Is<List<FundingImport>>(list => list.Count == expectedFundingInserted)),
                Times.Once);

            // Assert - tier2 (no filter)
            sectorSubjectAreaTier2ImportRepository.Verify(x =>
                x.InsertMany(It.Is<List<SectorSubjectAreaTier2Import>>(list => list.Count == sectorSubjectAreaTier2Csv.Count)),
                Times.Once);

            // Assert - tier1 filtered to remove those with '-'
            sectorSubjectAreaTier1ImportRepository.Verify(x =>
                x.InsertMany(It.Is<List<SectorSubjectAreaTier1Import>>(list => list.Count == expectedTier1Inserted)),
                Times.Once);
        }
    }
}
