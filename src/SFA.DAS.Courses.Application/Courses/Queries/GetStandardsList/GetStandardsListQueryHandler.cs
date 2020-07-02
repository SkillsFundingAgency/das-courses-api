using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetStandardsListQueryHandler : IRequestHandler<GetStandardsListQuery, GetStandardsListResult>
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

        public async Task<GetStandardsListResult> Handle(GetStandardsListQuery request, CancellationToken cancellationToken)
        {
            var standardsTask = _standardsService.GetStandardsList(request.Keyword, request.RouteIds, request.Levels);
            var totalTask = _standardsService.Count();

            await Task.WhenAll(standardsTask, totalTask);

            var standards = standardsTask.Result.ToList();

            if (standards.Count == 0 && !string.IsNullOrWhiteSpace(request.Keyword))
            {
                _logger.LogInformation($"Zero results for searching by keyword [{request.Keyword}]");
            }

            return new GetStandardsListResult
            {
                Standards = standards,
                Total = totalTask.Result,
                TotalFiltered = standards.Count
            };
        }
    }
}
