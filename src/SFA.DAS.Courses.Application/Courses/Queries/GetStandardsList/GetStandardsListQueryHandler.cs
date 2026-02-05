using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Policies;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetStandardsListQueryHandler : IRequestHandler<GetStandardsListQuery, GetStandardsListQueryResult>
    {
        private readonly ILogger<GetStandardsListQueryHandler> _logger;
        private readonly IStandardsService _standardsService;

        public GetStandardsListQueryHandler(
            ILogger<GetStandardsListQueryHandler> logger,
            IStandardsService standardsService)
        {
            _logger = logger;
            _standardsService = standardsService;
        }

        public async Task<GetStandardsListQueryResult> Handle(GetStandardsListQuery request, CancellationToken cancellationToken)
        {
            var standards = (await _standardsService.GetStandardsList(
                    request.Keyword,
                    request.RouteIds,
                    request.Levels,
                    request.OrderBy,
                    request.Filter,
                    request.IncludeAllProperties,
                    request.ApprenticeshipType))
                .Where(s => AllowedApprenticeshipTypesPolicy.IsStandard(s.ApprenticeshipType))
                .ToList();

            var total = await _standardsService.Count(request.Filter);

            if (standards.Count == 0 &&
                !string.IsNullOrWhiteSpace(request.Keyword) &&
                request.RouteIds.Count == 0 &&
                request.Levels.Count == 0)
            {
                _logger.LogInformation("Zero results for searching by keyword: {Keyword}", request.Keyword);
            }

            return new GetStandardsListQueryResult
            {
                Standards = standards,
                Total = total,
                TotalFiltered = standards.Count
            };
        }
    }
}
