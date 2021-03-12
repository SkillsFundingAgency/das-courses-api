using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetRoutes;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/courses/[controller]/")]
    public class RoutesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RoutesController> _logger;

        public RoutesController (IMediator mediator, ILogger<RoutesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetList()
        {
            var queryResult = await _mediator.Send(new GetRoutesQuery());
                
            var response = new GetRoutesListResponse
            {
                Routes = queryResult.Routes.Select(c=>(GetRouteResponse)c)
            };
                
            return Ok(response);
        }
    }
}
