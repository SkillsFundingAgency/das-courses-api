using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetStandardsListQueryHandler : IRequestHandler<GetStandardsListQuery, GetStandardsListResult>
    {
        private readonly IStandardsService _standardsService;

        public GetStandardsListQueryHandler(IStandardsService standardsService)
        {
            _standardsService = standardsService;
        }

        public async Task<GetStandardsListResult> Handle(GetStandardsListQuery request, CancellationToken cancellationToken)
        {
            var standardsTask = _standardsService.GetStandardsList(request.Keyword, request.RouteIds);
            var totalTask = _standardsService.Count();

            await Task.WhenAll(standardsTask, totalTask);

            var standards = standardsTask.Result.ToList();

            return new GetStandardsListResult
            {
                Standards = standards,
                Total = totalTask.Result,
                TotalFiltered = standards.Count
            };
        }
    }
}
