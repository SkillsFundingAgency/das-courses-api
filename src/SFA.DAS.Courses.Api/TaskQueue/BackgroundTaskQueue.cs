using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        private readonly object _lock = new();
        private readonly HashSet<string> _queuedOrRunningRequestNames = new(StringComparer.OrdinalIgnoreCase);

        public QueueBackgroundRequestResult QueueBackgroundRequest<TResponse>(
            IRequest<TResponse> request,
            string requestName,
            Action<TResponse, TimeSpan, ILogger<TaskQueueHostedService>, Guid> response)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(requestName);
            ArgumentNullException.ThrowIfNull(response);

            lock (_lock)
            {
                if (_queuedOrRunningRequestNames.Contains(requestName))
                {
                    return new QueueBackgroundRequestResult
                    {
                        Queued = false,
                        Reason = $"A {requestName} request is already queued or running"
                    };
                }

                _queuedOrRunningRequestNames.Add(requestName);
            }

            var requestId = Guid.NewGuid();

            _requests.Enqueue(new BackgroundRequest<TResponse>(
                requestId,
                request,
                requestName,
                response));

            _signal.Release();

            return new QueueBackgroundRequestResult
            {
                Queued = true,
                RequestId = requestId
            };
        }

        public async Task<IBackgroundRequest> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);

            if (!_requests.TryDequeue(out var request))
            {
                throw new InvalidOperationException(
                    "No background request was available after the queue signal was received.");
            }

            return request;
        }

        public void Complete(string requestName)
        {
            ArgumentNullException.ThrowIfNull(requestName);

            lock (_lock)
            {
                _queuedOrRunningRequestNames.Remove(requestName);
            }
        }
    }
}
