using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.TaskQueue;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards;

namespace SFA.DAS.Courses.Api.UnitTests.TaskQueue
{
    public class WhenUsingBackgroundTaskQueue
    {
        private BackgroundTaskQueue _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sut = new BackgroundTaskQueue();
        }

        [Test]
        public void Then_Queues_Request_When_RequestName_Is_Not_Already_Queued_Or_Running()
        {
            var result = _sut.QueueBackgroundRequest(
                new ImportDataCommand(),
                "data load",
                (_, _, _, _) => { });

            result.Queued.Should().BeTrue();
            result.RequestId.Should().NotBe(Guid.Empty);
            result.Reason.Should().BeNull();
        }

        [Test]
        public void Then_Does_Not_Queue_Request_When_Same_RequestName_Is_Already_Queued_Or_Running()
        {
            _sut.QueueBackgroundRequest(
                new ImportDataCommand(),
                "data load",
                (_, _, _, _) => { });

            var result = _sut.QueueBackgroundRequest(
                new ImportDataCommand(),
                "data load",
                (_, _, _, _) => { });

            result.Queued.Should().BeFalse();
            result.RequestId.Should().Be(Guid.Empty);
            result.Reason.Should().Be("A data load request is already queued or running");
        }

        [Test]
        public void Then_Queues_Request_When_Different_RequestName_Is_Already_Queued_Or_Running()
        {
            _sut.QueueBackgroundRequest(
                new ImportDataCommand(),
                "data load",
                (_, _, _, _) => { });

            var result = _sut.QueueBackgroundRequest(
                new ImportDataCommand(),
                "other load",
                (_, _, _, _) => { });

            result.Queued.Should().BeTrue();
            result.RequestId.Should().NotBe(Guid.Empty);
            result.Reason.Should().BeNull();
        }

        [Test]
        public void Then_Allows_Same_RequestName_After_Request_Is_Completed()
        {
            _sut.QueueBackgroundRequest(
                new ImportDataCommand(),
                "data load",
                (_, _, _, _) => { });

            _sut.Complete("data load");

            var result = _sut.QueueBackgroundRequest(
                new ImportDataCommand(),
                "data load",
                (_, _, _, _) => { });

            result.Queued.Should().BeTrue();
            result.RequestId.Should().NotBe(Guid.Empty);
            result.Reason.Should().BeNull();
        }

        [Test]
        public async Task Then_Dequeue_Returns_Queued_Request()
        {
            var queueResult = _sut.QueueBackgroundRequest(
                new ImportDataCommand(),
                "data load",
                (_, _, _, _) => { });

            var request = await _sut.DequeueAsync(CancellationToken.None);

            request.RequestName.Should().Be("data load");
            request.RequestId.Should().Be(queueResult.RequestId);
        }

        [Test]
        public async Task Then_Dequeue_Returns_Requests_In_The_Order_They_Were_Queued()
        {
            var firstQueueResult = _sut.QueueBackgroundRequest(
                new ImportDataCommand(),
                "first request",
                (_, _, _, _) => { });

            var secondQueueResult = _sut.QueueBackgroundRequest(
                new ImportDataCommand(),
                "second request",
                (_, _, _, _) => { });

            var firstRequest = await _sut.DequeueAsync(CancellationToken.None);
            var secondRequest = await _sut.DequeueAsync(CancellationToken.None);

            firstRequest.RequestName.Should().Be("first request");
            firstRequest.RequestId.Should().Be(firstQueueResult.RequestId);

            secondRequest.RequestName.Should().Be("second request");
            secondRequest.RequestId.Should().Be(secondQueueResult.RequestId);
        }

        [Test]
        public void Then_QueueBackgroundRequest_Throws_When_Request_Is_Null()
        {
            var action = () => _sut.QueueBackgroundRequest<ImportDataCommandResult>(
                null!,
                "data load",
                (_, _, _, _) => { });

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("request");
        }

        [Test]
        public void Then_QueueBackgroundRequest_Throws_When_RequestName_Is_Null()
        {
            var action = () => _sut.QueueBackgroundRequest(
                new ImportDataCommand(),
                null!,
                (_, _, _, _) => { });

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("requestName");
        }

        [Test]
        public void Then_QueueBackgroundRequest_Throws_When_Response_Is_Null()
        {
            var action = () => _sut.QueueBackgroundRequest<ImportDataCommandResult>(
                new ImportDataCommand(),
                "data load",
                null!);

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("response");
        }

        [Test]
        public void Then_Complete_Throws_When_RequestName_Is_Null()
        {
            var action = () => _sut.Complete(null!);

            action.Should().Throw<ArgumentNullException>()
                .WithParameterName("requestName");
        }

        [Test]
        public void Then_Complete_Does_Not_Throw_When_RequestName_Was_Not_Queued()
        {
            var action = () => _sut.Complete("data load");

            action.Should().NotThrow();
        }
    }
}
