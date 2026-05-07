using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Import
{
    public class WhenCallingImport
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_ImportStandardsCommand_And_Returns_OkResult(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DataLoadController controller)
        {
            // Arrange
            mockMediator
                .Setup(p => p.Send(It.IsAny<ImportDataCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ImportDataCommandResult { ValidationMessages = new List<string>() });

            // Act
            var controllerResult = await controller.Index(CancellationToken.None) as OkObjectResult;

            // Assert
            mockMediator.Verify(x=>x.Send(It.IsAny<ImportDataCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
