using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandard;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;
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
        [Route("")]
        public async Task<IActionResult> GetList(
            [FromQuery] string keyword,
            [FromQuery] IList<int> routeIds,
            [FromQuery] IList<int> levels,
            [FromQuery] OrderBy orderBy = OrderBy.Score,
            [FromQuery] StandardFilter filter = StandardFilter.ActiveAvailable)
        {
            var queryResult = await _mediator.Send(new GetStandardsListQuery
            {
                Keyword = keyword,
                RouteIds = routeIds,
                Levels = levels,
                OrderBy = orderBy,
                Filter = filter
            });

            var response = new GetStandardsListResponse
            {
                Standards = queryResult.Standards.Select(standard => (GetStandardResponse)standard),
                Total = queryResult.Total,
                TotalFiltered = queryResult.TotalFiltered
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _mediator.Send(new GetStandardByIdQuery { Id = id });

            if (result.Standard == null)
            {
                return NotFound();
            }

            return Ok((GetStandardDetailResponse)result.Standard);
        }

        [HttpGet]
        [Route("{id}/options/{option}/ksbs")]
        public async Task<IActionResult> GetOptionKsbs(string id, string option)
        {
            return Ok(new GetStandardOptionsResponse
            {
                KSBs = new StandardOptionKsb[]
                {
                   new StandardOptionKsb
                   {
                       Type = KsbType.Knowledge,
                       Key = "k1",
                       Detail = "core_knowledge_1",
                   }
                }
            });
        }

        [HttpGet]
        [Route("versions/{iFateReferenceNumber}")]
        public async Task<IActionResult> GetStandardsByIFateReferenceNumber(string iFateReferenceNumber)
        {
            var queryResult = await _mediator.Send(new GetStandardsByIFateReferenceQuery
            {
                IFateReferenceNumber = iFateReferenceNumber
            });

            if (queryResult.Standards.Any() == false)
            {
                return NotFound();
            }

            var response = new GetStandardVersionsListResponse
            {
                Standards = queryResult.Standards.Select(standard => (GetStandardDetailResponse)standard)
            };

            return Ok(response);
        }
    }
}
