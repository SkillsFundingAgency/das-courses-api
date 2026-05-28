using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Handlers
{
    public class WhenHandlingImportDataCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Import_Is_Executed_Inside_The_Application_Lock(
            ImportDataCommand command,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            ImportDataCommandHandler handler)
        {
            SetupApplicationLock(mockCoursesDataContext);

            await handler.Handle(command, CancellationToken.None);

            mockCoursesDataContext.Verify(x => x.ExecuteWithApplicationLockAsync(
                    "ImportData",
                    It.IsAny<Func<Task<ImportDataCommandResult>>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Services_Are_Called_To_Import_Data(
            ImportDataCommand command,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<IStandardsImportService> standardsImportService,
            [Frozen] Mock<ILarsDataImportService> larsImportService,
            [Frozen] Mock<IFrameworksImportService> frameworksImportService,
            ImportDataCommandHandler handler)
        {
            SetupApplicationLock(mockCoursesDataContext);

            await handler.Handle(command, CancellationToken.None);

            larsImportService.Verify(x => x.ImportDataIntoStaging(), Times.Once);
            frameworksImportService.Verify(x => x.ImportDataIntoStaging(), Times.Once);
            standardsImportService.Verify(x => x.ImportDataIntoStaging(), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Services_Are_Called_To_Load_Data_From_Staging(
            ImportDataCommand command,
            string frameworkFile,
            string larsFile,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<IStandardsImportService> standardsImportService,
            [Frozen] Mock<ILarsDataImportService> larsImportService,
            [Frozen] Mock<IFrameworksImportService> frameworksImportService,
            [Frozen] Mock<IIndexBuilder> indexBuilder,
            ImportDataCommandHandler handler)
        {
            SetupApplicationLock(mockCoursesDataContext);

            frameworksImportService
                .Setup(s => s.ImportDataIntoStaging())
                .ReturnsAsync((true, frameworkFile));

            larsImportService
                .Setup(s => s.ImportDataIntoStaging())
                .ReturnsAsync((true, larsFile));

            standardsImportService
                .Setup(s => s.ImportDataIntoStaging())
                .ReturnsAsync((true, new List<string>()));

            await handler.Handle(command, CancellationToken.None);

            larsImportService.Verify(x => x.LoadDataFromStaging(It.IsAny<DateTime>(), larsFile), Times.Once);
            frameworksImportService.Verify(x => x.LoadDataFromStaging(It.IsAny<DateTime>(), frameworkFile), Times.Once);
            standardsImportService.Verify(x => x.LoadDataFromStaging(It.IsAny<DateTime>()), Times.Once);
            indexBuilder.Verify(x => x.Build(), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_IndexBuilder_Is_Invoked(
            ImportDataCommand command,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<IIndexBuilder> indexBuilder,
            ImportDataCommandHandler handler)
        {
            SetupApplicationLock(mockCoursesDataContext);

            await handler.Handle(command, CancellationToken.None);

            indexBuilder.Verify(x => x.Build(), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Frameworks_Are_Not_Imported_Then_Frameworks_Are_Not_Loaded(
            ImportDataCommand command,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<IFrameworksImportService> frameworksImportService,
            ImportDataCommandHandler handler)
        {
            SetupApplicationLock(mockCoursesDataContext);

            frameworksImportService
                .Setup(s => s.ImportDataIntoStaging())
                .ReturnsAsync((false, null));

            await handler.Handle(command, CancellationToken.None);

            frameworksImportService.Verify(x => x.LoadDataFromStaging(It.IsAny<DateTime>(), It.IsAny<string>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_LarsData_Is_Not_Imported_Then_LarsData_Is_Not_Loaded(
            ImportDataCommand command,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<ILarsDataImportService> larsImportService,
            ImportDataCommandHandler handler)
        {
            SetupApplicationLock(mockCoursesDataContext);

            larsImportService
                .Setup(s => s.ImportDataIntoStaging())
                .ReturnsAsync((false, null));

            await handler.Handle(command, CancellationToken.None);

            larsImportService.Verify(x => x.LoadDataFromStaging(It.IsAny<DateTime>(), It.IsAny<string>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Standards_Are_Not_Imported_Then_Standards_Are_Not_Loaded_And_Cache_Is_Not_Cleared(
            ImportDataCommand command,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<IStandardsImportService> standardsImportService,
            [Frozen] Mock<ICoursesCacheService> coursesCacheService,
            ImportDataCommandHandler handler)
        {
            SetupApplicationLock(mockCoursesDataContext);

            standardsImportService
                .Setup(s => s.ImportDataIntoStaging())
                .ReturnsAsync((false, new List<string> { "validation failed" }));

            var result = await handler.Handle(command, CancellationToken.None);

            result.StandardsLoadedSuccessfully.Should().BeFalse();
            result.ValidationMessages.Should().ContainSingle().Which.Should().Be("validation failed");

            standardsImportService.Verify(x => x.LoadDataFromStaging(It.IsAny<DateTime>()), Times.Never);
            coursesCacheService.Verify(x => x.ClearCoursesCache(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Standards_Are_Loaded_Successfully_Then_Cache_Is_Cleared(
            ImportDataCommand command,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<IStandardsImportService> standardsImportService,
            [Frozen] Mock<ICoursesCacheService> coursesCacheService,
            ImportDataCommandHandler handler)
        {
            SetupApplicationLock(mockCoursesDataContext);

            standardsImportService
                .Setup(s => s.ImportDataIntoStaging())
                .ReturnsAsync((true, new List<string>()));

            await handler.Handle(command, CancellationToken.None);

            coursesCacheService.Verify(x => x.ClearCoursesCache(
                    "after successful standards load",
                    CancellationToken.None),
                Times.Once);
        }

        private static void SetupApplicationLock(
            Mock<ICoursesDataContext> mockCoursesDataContext)
        {
            mockCoursesDataContext
                .Setup(x => x.ExecuteWithApplicationLockAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<Task<ImportDataCommandResult>>>(),
                    It.IsAny<CancellationToken>()))
                .Returns<string, Func<Task<ImportDataCommandResult>>, CancellationToken>(
                    async (_, operation, _) => await operation());
        }
    }
}
