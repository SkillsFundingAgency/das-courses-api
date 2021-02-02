using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Handlers
{
    public class WhenHandlingImportDataCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Services_Are_Called_To_Import_Data(
            ImportDataCommand command,
            [Frozen] Mock<IStandardsImportService> standardsImportService,
            [Frozen] Mock<ILarsImportService> larsImportService,
            [Frozen] Mock<IFrameworksImportService> frameworksImportService,
            ImportDataCommandHandler handler)
        {
            // Act
            await handler.Handle(command, new CancellationToken());

            //Assert

            larsImportService.Verify(x=>x.ImportDataIntoStaging(), Times.Once);
            frameworksImportService.Verify(x=>x.ImportDataIntoStaging(), Times.Once);
            standardsImportService.Verify(x=>x.ImportDataIntoStaging(), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Services_Are_Called_To_Load_Data_From_Staging(
            ImportDataCommand command,
            string frameworkFile,
            string larsFile,
            [Frozen] Mock<IStandardsImportService> standardsImportService,
            [Frozen] Mock<ILarsImportService> larsImportService,
            [Frozen] Mock<IFrameworksImportService> frameworksImportService,
            [Frozen] Mock<IIndexBuilder> indexBuilder,
            ImportDataCommandHandler handler
            )
        {
            frameworksImportService.Setup(s => s.ImportDataIntoStaging()).ReturnsAsync((true, frameworkFile));
            larsImportService.Setup(s => s.ImportDataIntoStaging()).ReturnsAsync((true, larsFile));
            // Act
            await handler.Handle(command, new CancellationToken());

            //Assert
            larsImportService.Verify(x => x.LoadDataFromStaging(It.IsAny<DateTime>(), larsFile), Times.Once);
            frameworksImportService.Verify(x => x.LoadDataFromStaging(It.IsAny<DateTime>(), frameworkFile), Times.Once);
            standardsImportService.Verify(x => x.LoadDataFromStaging(It.IsAny<DateTime>()), Times.Once);
            indexBuilder.Verify(x => x.Build(), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_IndexBuilder_Is_Invoked(
            ImportDataCommand command,
            [Frozen] Mock<IIndexBuilder> indexBuilder,
            ImportDataCommandHandler handler
            )
        {
            // Act
            await handler.Handle(command, new CancellationToken());

            //Assert
            indexBuilder.Verify(x => x.Build(), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Frameworks_Are_Not_Imported_Then_Frameworks_Are_Not_Loaded(
            ImportDataCommand command,
            [Frozen] Mock<IFrameworksImportService> frameworksImportService,
            ImportDataCommandHandler handler
            )
        {
            //Arrange
            frameworksImportService.Setup(s => s.ImportDataIntoStaging()).ReturnsAsync((false, null));

            // Act
            await handler.Handle(command, new CancellationToken());

            //Assert
            frameworksImportService.Verify(x => x.LoadDataFromStaging(It.IsAny<DateTime>(), It.IsAny<string>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_LarsData_Is_Not_Imported_Then_LarsData_Is_Not_Loaded(
            ImportDataCommand command,
            [Frozen] Mock<ILarsImportService> larsImportService,
            ImportDataCommandHandler handler
            )
        {
            //Arrange
            larsImportService.Setup(s => s.ImportDataIntoStaging()).ReturnsAsync((false, null));

            // Act
            await handler.Handle(command, new CancellationToken());

            //Assert
            larsImportService.Verify(x => x.LoadDataFromStaging(It.IsAny<DateTime>(), It.IsAny<string>()), Times.Never);
        }

    }
}
