using System;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Courses.Api.Infrastructure;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/ops/dataload/")]
    public class DataLoadController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DataLoadController> _logger;
        private readonly IOutputCacheStore _outputCacheStore;

        public DataLoadController(
            IMediator mediator,
            ILogger<DataLoadController> logger,
            IOutputCacheStore outputCacheStore)
        {
            _mediator = mediator;
            _logger = logger;
            _outputCacheStore = outputCacheStore;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Data import request received");

            var result = await _mediator.Send(new ImportDataCommand(), cancellationToken);

            if (result.ValidationMessages.Count > 0)
            {
                var combinedValidationMessage = string.Join(Environment.NewLine, result.ValidationMessages);

                _logger.LogWarning(
                    "Data import completed with {ValidationErrorCount} validation messages:{NewLine}{ValidationMessages}",
                    result.ValidationMessages.Count,
                    Environment.NewLine,
                    combinedValidationMessage);
            }
            else
            {
                _logger.LogInformation("Data import completed with no validation messages");
            }

            if (result.StandardsLoadedSuccessfully)
            {
                await ClearCacheInternal(cancellationToken, "after successful standards load");
            }
            else
            {
                _logger.LogInformation("Courses cache not cleared because standards were not loaded");
            }

            return Ok(result.ValidationMessages);
        }

        [HttpPost("clear-cache")]
        public async Task<IActionResult> ClearCache(CancellationToken cancellationToken)
        {
            await ClearCacheInternal(cancellationToken, "manually");
            return NoContent();
        }

        private async Task ClearCacheInternal(CancellationToken cancellationToken, string message)
        {
            await _outputCacheStore.EvictByTagAsync(CoursesOutputCachePolicy.CoursesTag, cancellationToken);
            _logger.LogInformation("Courses cache cleared {Message}", message);
        }
    }
}
