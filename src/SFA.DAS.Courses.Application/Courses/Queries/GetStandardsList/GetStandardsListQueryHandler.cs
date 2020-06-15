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
            var standards = await _standardsService.GetStandardsList();

            return new GetStandardsListResult
            {
                Standards = standards
            };
        }
    }
}
