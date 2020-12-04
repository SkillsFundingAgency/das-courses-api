using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Application.Courses.Queries.GetOptions;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardSummary;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiVersion("2.0")]
    [ApiController]
    [Route("api/courses/standards/")]
    public class StandardsV2Controller : ControllerBase
    {
        private readonly ILogger<StandardsController> _logger;
        private readonly IMediator _mediator;

        public StandardsV2Controller(ILogger<StandardsController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllActiveStandardsSummary()
        {
            var result = await _mediator.Send(new GetActiveStandardsSummaryQuery()); 
            return Ok(result);
        }

        [HttpGet]
        [Route("options")]
        public async Task<IActionResult> GetOptions(string standardUId)
        {
            try
            {
                var result = await _mediator.Send(new GetStandardOptionsQuery(standardUId));
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("summary")]
        public async Task<IActionResult> GetStandardSummary(string standardUId)
        {
            try
            {
                var result = await _mediator.Send(new GetStandardSummaryQuery(standardUId));
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound();
            }
        }
    }
}
