using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public class GetStandardByStandardUIdQueryHandler : IRequestHandler<GetStandardByStandardUIdQuery, GetStandardByStandardUIdResult>
    {
        private readonly IStandardsService _standardsService;

        public GetStandardByStandardUIdQueryHandler(IStandardsService standardsService)
        {
            _standardsService = standardsService;
        }
        public async Task<GetStandardByStandardUIdResult> Handle(GetStandardByStandardUIdQuery request, CancellationToken cancellationToken)
        {
            var standard = await _standardsService.GetStandard(request.StandardUId);

            return new GetStandardByStandardUIdResult { Standard = standard };
        }
    }
}
