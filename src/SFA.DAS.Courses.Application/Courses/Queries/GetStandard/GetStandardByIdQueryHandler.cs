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
            // Id can be Lars Code which is int
            // Ifate Reference Number which length is 6 -> ST0001
            // Otherwise should be StandardUId -> ST0001_1.1
            // If Lars Code or IFateRef -> Will return latest active version
            Standard standard;
            if (int.TryParse(request.Id, out var larsCode))
            {
                standard = await _standardsService.GetLatestActiveStandard(larsCode);
            }
            else if (request.Id.Length == 6)
            {
                standard = await _standardsService.GetLatestActiveStandard(request.Id);
            }
            else
            {
                standard = await _standardsService.GetStandard(request.Id);
            }

            return new GetStandardByIdResult { Standard = standard };
        }
    }
}
