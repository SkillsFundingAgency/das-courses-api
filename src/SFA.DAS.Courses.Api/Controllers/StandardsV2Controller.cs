using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;

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
    }
}
