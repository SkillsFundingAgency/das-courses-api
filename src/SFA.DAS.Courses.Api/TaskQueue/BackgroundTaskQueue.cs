using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Courses.Api.TaskQueue
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<IBackgroundRequest> _requests = new();
        private readonly SemaphoreSlim _signal = new(0);

        public void QueueBackgroundRequest<TResponse>(
            IRequest<TResponse> request,
            string requestName,
            Action<TResponse, TimeSpan, ILogger<TaskQueueHostedService>> response)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(response);

            _requests.Enqueue(new BackgroundRequest<TResponse>(
                request,
                requestName,
                response));

            _signal.Release();
        }

        public async Task<IBackgroundRequest> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);

            if (!_requests.TryDequeue(out var request))
            {
                throw new InvalidOperationException("No background request was available after the queue signal was received.");
            }

            return request;
        }
    }
}
