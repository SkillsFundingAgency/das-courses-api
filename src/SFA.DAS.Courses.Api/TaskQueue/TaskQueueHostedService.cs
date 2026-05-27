using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Courses.Api.TaskQueue
{
    public class TaskQueueHostedService : BackgroundService
    {
        private readonly ILogger<TaskQueueHostedService> _logger;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceProvider _serviceProvider;

        public TaskQueueHostedService(
            IBackgroundTaskQueue taskQueue,
            ILogger<TaskQueueHostedService> logger,
            IServiceProvider serviceProvider)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task Queue Hosted Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                IBackgroundRequest backgroundRequest;

                try
                {
                    backgroundRequest = await _taskQueue.DequeueAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                using var scope = _serviceProvider.CreateScope();

                try
                {
                    await backgroundRequest.ExecuteAsync(scope, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error occurred executing {RequestName}.",
                        backgroundRequest.RequestName);
                }
            }

            _logger.LogInformation("Task Queue Hosted Service is stopping.");
        }
    }
}
