using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetFramework;
using SFA.DAS.Courses.Application.Courses.Queries.GetFrameworks;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/courses/[controller]/")]
    public class FrameworksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FrameworksController> _logger;

        public FrameworksController (IMediator mediator, ILogger<FrameworksController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
            
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _mediator.Send(new GetFrameworkQuery {FrameworkId = id});

            if (result.Framework == null) return NotFound();

            var response = (GetFrameworkResponse)result.Framework;

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var queryResult = await _mediator.Send(new GetFrameworksQuery());

            var response = new GetFrameworksResponse
            {
                Frameworks = queryResult.Frameworks.Select(framework => (GetFrameworkResponse)framework),
            };

            return Ok(response);
        }
    }
}
