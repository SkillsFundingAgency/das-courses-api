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
            larsImportService.Verify(x=>x.ImportData(), Times.Once);
            frameworksImportService.Verify(x=>x.ImportData(), Times.Once);
            standardsImportService.Verify(x=>x.ImportStandards(), Times.Once);
        }
    }
}