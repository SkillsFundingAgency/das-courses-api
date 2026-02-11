using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Application.Courses.Queries.GetCoursesSearch;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]/")]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string keyword,
            [FromQuery] IList<int> routeIds,
            [FromQuery] IList<int> levels,
            [FromQuery] ApprenticeshipType? learningType,
            [FromQuery] CourseType? courseType,
            [FromQuery] OrderBy orderBy = OrderBy.Score,
            [FromQuery] StandardFilter filter = StandardFilter.ActiveAvailable)
        {
            var queryResult = await _mediator.Send(new GetCoursesSearchQuery
            {
                Keyword = keyword,
                RouteIds = routeIds,
                Levels = levels,
                LearningType = learningType,
                CourseType = courseType,
                OrderBy = orderBy,
                Filter = filter,
                IncludeAllProperties = false
            });

            var response = new GetCoursesSearchResponse
            {
                Standards = queryResult.Standards.Select(course => (GetCourseResponse)course),
                Total = queryResult.Total,
                TotalFiltered = queryResult.TotalFiltered
            };

            return Ok(response);
        }
    }
}
