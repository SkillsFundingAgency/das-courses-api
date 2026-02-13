using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Interfaces;

using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetCoursesSearch
{
    public class GetCoursesSearchQueryHandler : IRequestHandler<GetCoursesSearchQuery, GetCoursesSearchQueryResult>
    {
        private readonly ILogger<GetCoursesSearchQueryHandler> _logger;
        private readonly IStandardsService _standardsService;

        public GetCoursesSearchQueryHandler(
            ILogger<GetCoursesSearchQueryHandler> logger,
            IStandardsService standardsService)
        {
            _logger = logger;
            _standardsService = standardsService;
        }

        public async Task<GetCoursesSearchQueryResult> Handle(GetCoursesSearchQuery request, CancellationToken cancellationToken)
        {
            var courses = (await _standardsService.GetCoursesList(
                    request.Keyword,
                    request.RouteIds,
                    request.Levels,
                    request.OrderBy,
                    request.Filter,
                    request.IncludeAllProperties,
                    request.LearningType,
                    request.CourseType))
                .ToList();

            var total = await _standardsService.CountCourses(request.Filter, request.CourseType);

            if (courses.Count == 0 &&
                !string.IsNullOrWhiteSpace(request.Keyword) &&
                request.RouteIds.Count == 0 &&
                request.Levels.Count == 0)
            {
                _logger.LogInformation("Zero results for searching by keyword: {Keyword}", request.Keyword);
            }

            return new GetCoursesSearchQueryResult
            {
                Standards = courses,
                Total = total,
                TotalFiltered = courses.Count
            };
        }
    }
}
