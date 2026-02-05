using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandard;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardOptionKsbs;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsByIFateReference;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiVersion("1.0")]
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
        public async Task<IActionResult> GetList(
            [FromQuery] string keyword,
            [FromQuery] IList<int> routeIds,
            [FromQuery] IList<int> levels,
            [FromQuery] ApprenticeshipType apprenticeshipType,
            [FromQuery] OrderBy orderBy = OrderBy.Score,
            [FromQuery] StandardFilter filter = StandardFilter.ActiveAvailable)
        {
            var queryResult = await _mediator.Send(new GetStandardsListQuery
            {
                Keyword = keyword,
                RouteIds = routeIds,
                Levels = levels,
                ApprenticeshipType = apprenticeshipType,
                OrderBy = orderBy,
                Filter = filter,
                IncludeAllProperties = false
            });

            var response = new GetStandardsListResponse
            {
                Standards = queryResult.Standards.Select(standard => (GetStandardResponse)standard),
                Total = queryResult.Total,
                TotalFiltered = queryResult.TotalFiltered
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _mediator.Send(new GetStandardByIdQuery { Id = id });

            if (result.Standard is null)
                return NotFound();

            return Ok((GetStandardDetailResponse)result.Standard);
        }

        [HttpGet("{id}/options/{option}/ksbs")]
        public async Task<IActionResult> GetOptionKsbs(string id, string option)
        {
            var queryResult = await _mediator.Send(new GetStandardOptionKsbsQuery
            {
                Id = id,
                Option = option
            });

            return Ok(queryResult);
        }

        [HttpGet("versions/{iFateReferenceNumber}")]
        public async Task<IActionResult> GetStandardsByIFateReferenceNumber(string iFateReferenceNumber)
        {
            var queryResult = await _mediator.Send(new GetStandardsByIFateReferenceQuery
            {
                IFateReferenceNumber = iFateReferenceNumber
            });

            if (!queryResult.Standards.Any())
                return NotFound();

            var response = new GetStandardVersionsListResponse
            {
                Standards = queryResult.Standards.Select(standard => (GetStandardDetailResponse)standard)
            };

            return Ok(response);
        }
    }
}
