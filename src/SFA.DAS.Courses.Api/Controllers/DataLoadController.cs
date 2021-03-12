using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards;
using Microsoft.Extensions.Logging;

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
            await _mediator.Send(new ImportDataCommand());
            _logger.LogInformation("Data import completed successfully");
            return NoContent();
        }
    }
}
