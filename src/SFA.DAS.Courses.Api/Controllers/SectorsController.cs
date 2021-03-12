using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetSectors;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/courses/[controller]/")]
    public class SectorsController : ControllerBase
    {
        private readonly ILogger<SectorsController> _logger;
        private readonly IMediator _mediator;

        public SectorsController(ILogger<SectorsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetList()
        {
            var queryResult = await _mediator.Send(new GetSectorsListQuery());

            var response = new GetSectorsListResponse
            {
                Sectors = queryResult.Sectors.Select(c=>(GetSectorResponse)c)
            };

            return Ok(response);
        }
    }
}
