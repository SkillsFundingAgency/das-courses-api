using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Application.StandardsImport.Handlers.ImportStandards;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Import
{
    public class WhenCallingImport
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_ImportStandardsCommand_And_Returns_NoContent_Result(
            [Frozen] Mock<IMediator> mockMediator,
            ImportController controller)
        {
            var controllerResult = await controller.Index() as NoContentResult;

            mockMediator.Verify(x=>x.Send(It.IsAny<ImportStandardsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        
    }
}