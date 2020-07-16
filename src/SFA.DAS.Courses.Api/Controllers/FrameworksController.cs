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
            try
            {
                var result = await _mediator.Send(new GetFrameworkQuery {FrameworkId = id});

                var response = (GetFrameworkResponse)result.Framework;

                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, $"Framework not found {id}");
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetFrameworksQuery());

                var response = new GetFrameworksResponse
                {
                    Frameworks = queryResult.Frameworks.Select(framework => (GetFrameworkResponse)framework),
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of standards");
                return BadRequest();
            }
        }
    }
}