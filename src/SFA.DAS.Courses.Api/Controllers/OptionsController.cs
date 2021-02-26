using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/courses/standards/[controller]/")]
    public class OptionsController : ControllerBase
    {
        private readonly ILogger<OptionsController> _logger;
        private readonly IMediator _mediator;

        public OptionsController(ILogger<OptionsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetStandardOptionsList()
        {
            try
            {
                var queryResult = await _mediator.Send(new GetStandardsListQuery
                {
                    Filter = StandardFilter.Active
                });

                var response = new GetStandardOptionsListResponse
                {
                    Standards = queryResult.Standards.Select(standard => (GetStandardOptionsResponse)standard)
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of standards and options");
                return BadRequest();
            }
        }
    }
}
