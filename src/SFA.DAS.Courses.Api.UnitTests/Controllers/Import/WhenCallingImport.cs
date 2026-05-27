using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Api.TaskQueue;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ClearCoursesCache;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Controllers.Import
{
    public class WhenCallingImport
    {
        [Test, MoqAutoData]
        public void Then_Queues_ImportDataCommand_And_Returns_AcceptedResult(
            [Frozen] Mock<IBackgroundTaskQueue> mockTaskQueue,
            [Greedy] DataLoadController _sut)
        {
            var result = _sut.Index();

            mockTaskQueue.Verify(x => x.QueueBackgroundRequest(
                    It.Is<IRequest<ImportDataCommandResult>>(request => request is ImportDataCommand),
                    "data load",
                    It.IsAny<Action<ImportDataCommandResult, TimeSpan, ILogger<TaskQueueHostedService>>>()),
                Times.Once);

            result.Should().BeOfType<ObjectResult>();

            var acceptedResult = result as ObjectResult;
            acceptedResult!.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
        }

        [Test, MoqAutoData]
        public void Then_When_Queueing_Fails_Returns_InternalServerError(
            [Frozen] Mock<IBackgroundTaskQueue> mockTaskQueue,
            [Greedy] DataLoadController _sut)
        {
            mockTaskQueue
                .Setup(x => x.QueueBackgroundRequest(
                    It.IsAny<IRequest<ImportDataCommandResult>>(),
                    It.IsAny<string>(),
                    It.IsAny<Action<ImportDataCommandResult, TimeSpan, ILogger<TaskQueueHostedService>>>()))
                .Throws(new Exception("Queue failed"));

            var result = _sut.Index();

            result.Should().BeOfType<StatusCodeResult>();

            var objectResult = result as StatusCodeResult;
            objectResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test, MoqAutoData]
        public async Task Then_ClearCache_Sends_ClearCoursesCacheCommand_And_Returns_NoContent(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DataLoadController _sut)
        {
            var result = await _sut.ClearCache(CancellationToken.None);

            mockMediator.Verify(x => x.Send(
                    It.IsAny<ClearCoursesCacheCommand>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            result.Should().BeOfType<NoContentResult>();

            var noContentResult = result as NoContentResult;
            noContentResult!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }
    }
}
