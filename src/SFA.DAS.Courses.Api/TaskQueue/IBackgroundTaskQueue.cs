using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Courses.Api.TaskQueue
{
    public interface IBackgroundTaskQueue
    {
        QueueBackgroundRequestResult QueueBackgroundRequest<TResponse>(
            IRequest<TResponse> request,
            string requestName,
            Action<TResponse, TimeSpan, ILogger<TaskQueueHostedService>, Guid> response);

        Task<IBackgroundRequest> DequeueAsync(CancellationToken cancellationToken);

        void Complete(string requestName);
    }
}
