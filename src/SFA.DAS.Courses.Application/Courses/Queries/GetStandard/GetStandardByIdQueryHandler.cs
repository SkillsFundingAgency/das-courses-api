using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public class GetStandardByIdQueryHandler : IRequestHandler<GetStandardByIdQuery, GetStandardByIdResult>
    {
        private readonly IStandardsService _standardsService;

        public GetStandardByIdQueryHandler(IStandardsService standardsService)
        {
            _standardsService = standardsService;
        }
        public async Task<GetStandardByIdResult> Handle(GetStandardByIdQuery request, CancellationToken cancellationToken)
        {
            Standard standard;
            if (IsLarsCode(request, out var larsCode))
            {
                standard = await _standardsService.GetLatestActiveStandard(larsCode);
            }
            else if (IsIfateReference(request))
            {
                standard = await _standardsService.GetLatestActiveStandard(request.Id);
            }
            else
            {
                standard = await _standardsService.GetStandard(request.Id);
            }

            return new GetStandardByIdResult { Standard = standard };
        }

        private static bool IsIfateReference(GetStandardByIdQuery request)
        {
            return request.Id.Length == 6;
        }

        private static bool IsLarsCode(GetStandardByIdQuery request, out int larsCode)
        {
            return int.TryParse(request.Id, out larsCode);
        }
    }
}
