using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMediator _mediator;

        public OptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetStandardOptionsList()
        {
            var queryResult = await _mediator.Send(new GetStandardsListQuery
            {
                Filter = StandardFilter.Active
            });

            var response = new GetStandardOptionsListResponse
            {
                StandardOptions = queryResult.Standards.Select(standard => (GetStandardOptionsResponse)standard)
            };

            return Ok(response);
        }
    }
}
