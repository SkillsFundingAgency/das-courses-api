using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.StandardsImport.Handlers.ImportStandards;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.StandardsImport.Handlers
{
    public class WhenHandlingImportStandardsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_To_Import_Data(
            ImportStandardsCommand command,
            [Frozen] Mock<IStandardsImportService> service,
            ImportStandardsCommandHandler handler)
        {
            // Act
            await handler.Handle(command, new CancellationToken());
            
            //Assert
            service.Verify(x=>x.ImportStandards(), Times.Once);
        }
    }
}