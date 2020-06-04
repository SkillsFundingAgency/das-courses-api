using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiController]
    [Route("api/courses/[controller]/")]
    public class StandardsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StandardsController(IMediator mediator)
        {
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
                //todo: log e
                return BadRequest();
            }
        }
    }
}
