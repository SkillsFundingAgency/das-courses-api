using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Api.Infrastructure;
using SFA.DAS.Courses.Application.Courses.Queries.GetCourse;
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
        [OutputCache(PolicyName = CoursesOutputCachePolicy.CoursesDataLoad)]
        public async Task<IActionResult> Search(
            [FromQuery] string keyword,
            [FromQuery] IList<int> routeIds,
            [FromQuery] IList<int> levels,
            [FromQuery] IList<ApprenticeshipType> learningTypes,
            [FromQuery] CourseType? courseType,
            [FromQuery] OrderBy orderBy = OrderBy.Score,
            [FromQuery] StandardFilter filter = StandardFilter.ActiveAvailable)
        {
            var queryResult = await _mediator.Send(new GetCoursesSearchQuery
            {
                Keyword = keyword,
                RouteIds = routeIds,
                Levels = levels,
                LearningTypes = learningTypes,
                CourseType = courseType,
                OrderBy = orderBy,
                Filter = filter,
                IncludeAllProperties = false
            });

            var response = new GetCoursesSearchResponse
            {
                Courses = queryResult.Standards.Select(course => (GetCourseResponse)course),
                Total = queryResult.Total,
                TotalFiltered = queryResult.TotalFiltered
            };

            return Ok(response);
        }

        [HttpGet("lookup/{id}")]
        [OutputCache(PolicyName = CoursesOutputCachePolicy.CoursesDataLoad)]
        public async Task<IActionResult> Lookup(string id)
        {
            var result = await _mediator.Send(new GetCourseByIdQuery { Id = id });

            if (result.Course is null)
                return NotFound();

            return Ok((GetCourseDetailResponse)result.Course);
        }
    }
}
