using System;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Api.TaskQueue;
using SFA.DAS.Courses.Application.CoursesImport.Extensions;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ClearCoursesCache;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/ops/dataload/")]
    public class DataLoadController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<DataLoadController> _logger;

        public DataLoadController(
            IMediator mediator,
            IBackgroundTaskQueue taskQueue,
            ILogger<DataLoadController> logger)
        {
            _mediator = mediator;
            _taskQueue = taskQueue;
            _logger = logger;
        }

        [HttpPost]
        [Route("")]
        public IActionResult Index()
        {
            const string requestName = "data load";

            return QueueBackgroundRequest(new ImportDataCommand(), requestName,
                (result, duration, log) =>
                {
                    if (result.ValidationMessages.Count > 0)
                    {
                        var combinedValidationMessage = string.Join(
                            Environment.NewLine,
                            result.ValidationMessages);

                        log.LogWarning(
                            "Completed request to {RequestName} in {Duration} with {ValidationErrorCount} validation messages:{NewLine}{ValidationMessages}",
                            requestName,
                            duration.ToReadableString(),
                            result.ValidationMessages.Count,
                            Environment.NewLine,
                            combinedValidationMessage);
                    }
                    else
                    {
                        log.LogInformation(
                            "Completed request to {RequestName} in {Duration} with no validation messages",
                            requestName,
                            duration.ToReadableString());
                    }
                });
        }

        [HttpPost("clear-cache")]
        public async Task<IActionResult> ClearCache(CancellationToken cancellationToken)
        {
            await _mediator.Send(new ClearCoursesCacheCommand(), cancellationToken);

            return NoContent();
        }

        protected IActionResult QueueBackgroundRequest<TResponse>(
            IRequest<TResponse> request,
            string requestName,
            Action<TResponse, TimeSpan, ILogger<TaskQueueHostedService>> result)
        {
            try
            {
                _logger.LogInformation("Received request to {RequestName}", requestName);

                _taskQueue.QueueBackgroundRequest(request, requestName, result);

                _logger.LogInformation("Queued request to {RequestName}", requestName);

                return StatusCode(StatusCodes.Status202Accepted, new
                {
                    Message = $"Request to {requestName} has been accepted for asynchronous processing",
                    QueuedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request to {RequestName} failed", requestName);

                return StatusCode(500);
            }
        }
    }
}
