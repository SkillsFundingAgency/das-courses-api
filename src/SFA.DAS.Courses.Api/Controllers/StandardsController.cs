using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
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
        public async Task<IActionResult> GetList(
            [FromQuery] string keyword,
            [FromQuery] IList<Guid> routeIds, 
            [FromQuery] IList<int> levels,
            [FromQuery] OrderBy orderBy = OrderBy.Score,
            [FromQuery] StandardFilter filter = StandardFilter.None)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get list of standards");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{larsCode:int}")]
        public async Task<IActionResult> Get(int larsCode)
        {
            try
            {
                var result = await _mediator.Send(new GetStandardQuery {LarsCode = larsCode});

                var response = (GetStandardResponse)result.Standard;

                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, $"Standard not found {larsCode}");
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{standardUId}")]
        public async Task<IActionResult> Get(string standardUId)
        {
            try
            {
                var result = await _mediator.Send(new GetStandardByStandardUIdQuery { StandardUId = standardUId });

                var response = (GetStandardDetailResponse)result.Standard;

                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, $"Standard not found for StandardUId: {standardUId}");
                return NotFound();
            }
        }

        [HttpGet]
        [Route("versions/{iFateReferenceNumber}")]
        public async Task<IActionResult> GetStandardsByIFateReferenceNumber(string iFateReferenceNumber)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetStandardsByIFateReferenceQuery { IFateReferenceNumber = iFateReferenceNumber });

                var response = new GetStandardVersionsListResponse
                {
                    Standards = queryResult.Standards.Select(standard => (GetStandardDetailResponse)standard)
                };

                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e, "Error attempting to get list of standards");
                return BadRequest();
            }
        }
    }
}
