﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
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
    public class WhenImportingLarsData
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Download_Path_Is_Parsed_From_The_Url_And_The_Current_File_Is_Checked_Against_The_Existing_And_If_Same_Then_Download(
            string filePath,
            string content,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> service,
            [Frozen] Mock<IImportAuditRepository> repository,
            LarsImportService larsImportService)
        {
            //Arrange
            service.Setup(x => x.GetFileStream(filePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            pageParser.Setup(x => x.GetCurrentLarsDataDownloadFilePath()).ReturnsAsync(filePath);
            repository.Setup(x => x.GetLastImportByType(ImportType.LarsImport))
                .ReturnsAsync(new ImportAudit(DateTime.Now, 100, ImportType.LarsImport, filePath));

            //Act
            await larsImportService.ImportDataIntoStaging();

            //Assert
            pageParser.Verify(x => x.GetCurrentLarsDataDownloadFilePath(), Times.Once);
            service.Verify(x => x.GetFileStream(filePath), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Data_Is_Downloaded_From_The_Api_If_No_Previous_File_Returned(
            string filePath,
            string content,
            string newFilePath,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> service,
            [Frozen] Mock<IImportAuditRepository> repository,
            LarsImportService larsImportService)
        {
            //Arrange
            service.Setup(x => x.GetFileStream(newFilePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            pageParser.Setup(x => x.GetCurrentLarsDataDownloadFilePath()).ReturnsAsync(newFilePath);
            repository.Setup(x => x.GetLastImportByType(ImportType.LarsImport))
                .ReturnsAsync((ImportAudit)null);

            //Act
            await larsImportService.ImportDataIntoStaging();

            //Assert
            service.Verify(x => x.GetFileStream(newFilePath), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Data_Is_Downloaded_From_The_Api_If_Different_File_To_Previous_File_Returned(
            string filePath,
            string downloadPath,
            string content,
            string newFilePath,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> service,
            [Frozen] Mock<IImportAuditRepository> repository,
            LarsImportService larsImportService)
        {
            //Arrange
            service.Setup(x => x.GetFileStream(newFilePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            pageParser.Setup(x => x.GetCurrentLarsDataDownloadFilePath()).ReturnsAsync(newFilePath);
            repository.Setup(x => x.GetLastImportByType(ImportType.LarsImport))
                .ReturnsAsync(new ImportAudit(DateTime.Now, 100, ImportType.LarsImport, downloadPath));

            //Act
            await larsImportService.ImportDataIntoStaging();

            //Assert
            service.Verify(x => x.GetFileStream(newFilePath), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Csv_File_Is_Extracted_From_The_Archive(
            string filePath,
            string content,
            string newFilePath,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> service,
            [Frozen] Mock<IImportAuditRepository> repository,
            [Frozen] Mock<IZipArchiveHelper> zipHelper,
            LarsImportService larsImportService)
        {
            //Arrange
            service.Setup(x => x.GetFileStream(newFilePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            pageParser.Setup(x => x.GetCurrentLarsDataDownloadFilePath()).ReturnsAsync(newFilePath);
            repository.Setup(x => x.GetLastImportByType(ImportType.LarsImport))
                .ReturnsAsync((ImportAudit)null);

            //Act
            await larsImportService.ImportDataIntoStaging();

            //Assert
            zipHelper.Verify(x =>
                    x.ExtractModelFromCsvFileZipStream<StandardCsv>(It.IsAny<Stream>(), Constants.LarsStandardsFileName),
                Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_ApprenticeFunding_Csv_Is_Extracted_From_The_Archive(
            string filePath,
            string content,
            string newFilePath,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> service,
            [Frozen] Mock<IImportAuditRepository> repository,
            [Frozen] Mock<IZipArchiveHelper> zipHelper,
            LarsImportService larsImportService)
        {
            //Arrange
            service.Setup(x => x.GetFileStream(newFilePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            pageParser.Setup(x => x.GetCurrentLarsDataDownloadFilePath()).ReturnsAsync(newFilePath);
            repository.Setup(x => x.GetLastImportByType(ImportType.LarsImport))
                .ReturnsAsync((ImportAudit)null);

            //Act
            await larsImportService.ImportDataIntoStaging();

            //Assert
            zipHelper.Verify(x =>
                    x.ExtractModelFromCsvFileZipStream<ApprenticeshipFundingCsv>(It.IsAny<Stream>(), Constants.LarsApprenticeshipFundingFileName),
                Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_SectorSubjectAreaTier2_Csv_Is_Extracted_From_The_Archive(
            string filePath,
            string content,
            string newFilePath,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> service,
            [Frozen] Mock<IImportAuditRepository> repository,
            [Frozen] Mock<IZipArchiveHelper> zipHelper,
            LarsImportService larsImportService)
        {
            //Arrange
            service.Setup(x => x.GetFileStream(newFilePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            pageParser.Setup(x => x.GetCurrentLarsDataDownloadFilePath()).ReturnsAsync(newFilePath);
            repository.Setup(x => x.GetLastImportByType(ImportType.LarsImport))
                .ReturnsAsync((ImportAudit)null);

            //Act
            await larsImportService.ImportDataIntoStaging();

            //Assert
            zipHelper.Verify(x =>
                    x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier2Csv>(It.IsAny<Stream>(), Constants.LarsSectorSubjectAreaTier2FileName),
                Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_SectorSubjectAreaTier1_Csv_Is_Extracted_From_The_Archive(
            string filePath,
            string content,
            string newFilePath,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> service,
            [Frozen] Mock<IImportAuditRepository> repository,
            [Frozen] Mock<IZipArchiveHelper> zipHelper,
            LarsImportService larsImportService)
        {
            //Arrange
            service.Setup(x => x.GetFileStream(newFilePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            pageParser.Setup(x => x.GetCurrentLarsDataDownloadFilePath()).ReturnsAsync(newFilePath);
            repository.Setup(x => x.GetLastImportByType(ImportType.LarsImport))
                .ReturnsAsync((ImportAudit)null);

            //Act
            await larsImportService.ImportDataIntoStaging();

            //Assert
            zipHelper.Verify(x =>
                    x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier2Csv>(It.IsAny<Stream>(), Constants.LarsSectorSubjectAreaTier2FileName),
                Times.Once);
        }


        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Current_Lars_Import_Data_Is_Deleted(
            string filePath,
            string content,
            string newFilePath,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> service,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier1ImportRepository> sectorSubjectAreaTier1ImportRepository,
            [Frozen] Mock<IImportAuditRepository> repository,
            LarsImportService larsImportService)
        {
            //Arrange
            service.Setup(x => x.GetFileStream(newFilePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            pageParser.Setup(x => x.GetCurrentLarsDataDownloadFilePath()).ReturnsAsync(newFilePath);
            repository.Setup(x => x.GetLastImportByType(ImportType.LarsImport))
                .ReturnsAsync((ImportAudit)null);

            //Act
            await larsImportService.ImportDataIntoStaging();

            //Assert
            larsStandardImportRepository.Verify(x => x.DeleteAll(), Times.Once);
            apprenticeshipFundingImportRepository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier2ImportRepository.Verify(x => x.DeleteAll(), Times.Once);
            sectorSubjectAreaTier1ImportRepository.Verify(x => x.DeleteAll(), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standard_Data_Is_Loaded_Into_The_Import_Repository(
            string filePath,
            string content,
            string newFilePath,
            ApprenticeshipFundingCsv frameWorkCsv,
            List<StandardCsv> standardCsv,
            List<ApprenticeshipFundingCsv> apprenticeFundingCsv,
            List<SectorSubjectAreaTier2Csv> sectorSubjectAreaTier2Data,
            List<SectorSubjectAreaTier1Csv> sectorSubjectAreaTier1Data,
            [Frozen] Mock<ILarsPageParser> pageParser,
            [Frozen] Mock<IDataDownloadService> service,
            [Frozen] Mock<ILarsStandardImportRepository> larsStandardImportRepository,
            [Frozen] Mock<IApprenticeshipFundingImportRepository> apprenticeshipFundingImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier2ImportRepository> sectorSubjectAreaTier2ImportRepository,
            [Frozen] Mock<ISectorSubjectAreaTier1ImportRepository> sectorSubjectAreaTier1ImportRepository,
            [Frozen] Mock<IImportAuditRepository> repository,
            [Frozen] Mock<IZipArchiveHelper> zipHelper,
            LarsImportService larsImportService)
        {
            //Arrange
            sectorSubjectAreaTier1Data.ForEach(s => s.SectorSubjectAreaTier1 = "1.00");
            apprenticeFundingCsv = apprenticeFundingCsv.Select(c => { c.ApprenticeshipType = "STD"; return c; }).ToList();
            frameWorkCsv.ApprenticeshipType = "FRMK";
            apprenticeFundingCsv.Add(frameWorkCsv);
            service.Setup(x => x.GetFileStream(newFilePath))
                .ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            pageParser.Setup(x => x.GetCurrentLarsDataDownloadFilePath()).ReturnsAsync(newFilePath);
            repository.Setup(x => x.GetLastImportByType(ImportType.LarsImport))
                .ReturnsAsync((ImportAudit)null);
            zipHelper.Setup(x =>
                    x.ExtractModelFromCsvFileZipStream<StandardCsv>(It.IsAny<Stream>(),
                        Constants.LarsStandardsFileName))
                .Returns(standardCsv);
            zipHelper.Setup(x =>
                    x.ExtractModelFromCsvFileZipStream<ApprenticeshipFundingCsv>(It.IsAny<Stream>(),
                        Constants.LarsApprenticeshipFundingFileName))
                .Returns(apprenticeFundingCsv);
            zipHelper.Setup(x =>
                    x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier2Csv>(It.IsAny<Stream>(),
                        Constants.LarsSectorSubjectAreaTier2FileName))
                .Returns(sectorSubjectAreaTier2Data);
            zipHelper.Setup(x =>
                    x.ExtractModelFromCsvFileZipStream<SectorSubjectAreaTier1Csv>(It.IsAny<Stream>(),
                        Constants.LarsSectorSubjectAreaTier1FileName))
                .Returns(sectorSubjectAreaTier1Data);

            //Act
            await larsImportService.ImportDataIntoStaging();

            //Assert
            larsStandardImportRepository.Verify(x =>
                x.InsertMany(It.Is<List<LarsStandardImport>>(c => c.Count.Equals(standardCsv.Count))), Times.Once);
            apprenticeshipFundingImportRepository.Verify(x =>
                x.InsertMany(It.Is<List<ApprenticeshipFundingImport>>(c => c.Count.Equals(apprenticeFundingCsv.Count - 1))), Times.Once);
            sectorSubjectAreaTier2ImportRepository.Verify(x => x.InsertMany(It.Is<List<SectorSubjectAreaTier2Import>>(c => c.Count.Equals(sectorSubjectAreaTier2Data.Count))), Times.Once);
            sectorSubjectAreaTier1ImportRepository.Verify(x => x.InsertMany(It.Is<List<SectorSubjectAreaTier1Import>>(c => c.Count.Equals(sectorSubjectAreaTier1Data.Count))), Times.Once);
        }
    }
}
