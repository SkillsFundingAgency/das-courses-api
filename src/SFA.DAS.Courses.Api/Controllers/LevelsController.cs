using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetLevels;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/courses/[controller]")]
    public class LevelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LevelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetList()
        {
            var queryResult = await _mediator.Send(new GetLevelsListQuery());

            var response = new GetLevelsListResponse
            {
                Levels = queryResult.Levels.Select(level => (GetLevelResponse)level)
            };

            return Ok(response);
        }
    }
}
