using System;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public DataLoadController (
            IMediator mediator,
            ILogger<DataLoadController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Data import request received");
            
            var validationMessages = await _mediator.Send(new ImportDataCommand());
            if (validationMessages.Count > 0)
            {
                var combinedValidationMessage = string.Join(Environment.NewLine, validationMessages);

                _logger.LogWarning(
                    "Data import completed with {ValidationErrorCount} validation errors:{NewLine}{ValidationMessages}",
                    validationMessages.Count,
                    Environment.NewLine,
                    combinedValidationMessage);
            }
            else
            {
                _logger.LogInformation("Data import completed successfully");
            }

            return Ok(validationMessages);
        }
    }
}
