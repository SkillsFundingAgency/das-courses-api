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
            var result = await _standardsService.GetStandardsList(request.Keyword);
            var total = await _standardsService.Count();

            return new GetStandardsListResult
            {
                Standards = result.Standards,
                Total = total,
                TotalFiltered = result.Standards.Count()
            };
        }
    }
}
