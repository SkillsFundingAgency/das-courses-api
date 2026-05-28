using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Courses.Api.TaskQueue
{
    public class BackgroundRequest<TResponse> : IBackgroundRequest
    {
        private readonly IRequest<TResponse> _request;
        private readonly Action<TResponse, TimeSpan, ILogger<TaskQueueHostedService>, Guid> _response;

        public BackgroundRequest(
            Guid requestId,
            IRequest<TResponse> request,
            string requestName,
            Action<TResponse, TimeSpan, ILogger<TaskQueueHostedService>, Guid> response)
        {
            RequestId = requestId;
            _request = request;
            RequestName = requestName;
            _response = response;
        }

        public Guid RequestId { get; }

        public string RequestName { get; }

        public async Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<TaskQueueHostedService>>();

            var started = DateTime.UtcNow;

            logger.LogInformation(
                "Started background request {RequestName} with request id {RequestId}",
                RequestName,
                RequestId);

            var result = await mediator.Send(_request, cancellationToken);

            var duration = DateTime.UtcNow - started;

            _response?.Invoke(result, duration, logger, RequestId);
        }
    }
}
