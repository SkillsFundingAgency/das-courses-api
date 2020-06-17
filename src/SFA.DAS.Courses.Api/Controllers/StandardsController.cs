using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandard;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiController]
    [Route("api/courses/[controller]/")]
    public class StandardsController : ControllerBase
    {
        private readonly ILogger<StandardsController> _logger;
        private readonly IMediator _mediator;

        public StandardsController(ILogger<StandardsController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetStandardsListQuery());

                var response = new GetStandardsListResponse
                {
                    Standards = queryResult.Standards.Select(standard => (GetStandardResponse)standard)
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of standards");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetStandardQuery {StandardId = id});

                var response = (GetStandardResponse)result.Standard;

                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, $"Standard not found {id}");
                return NotFound();
            }
        }
    }
}
