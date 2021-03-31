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
    [Route("/ops/export")]
    public class ExportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var queryResult = await _mediator.Send(new GetStandardsListQuery
            {
                Filter = StandardFilter.None
            });

            var response = new GetStandardsExportResponse
            {
                Standards = queryResult.Standards.Select(standard => (GetStandardDetailResponse)standard)
            };

            return Ok(response);
        }
    }
}
