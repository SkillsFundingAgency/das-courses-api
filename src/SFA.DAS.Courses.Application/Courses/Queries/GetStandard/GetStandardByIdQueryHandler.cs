using System.Threading;
using System.Threading.Tasks;
using MediatR;
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
            var standard = await new GetStandardByAnyId(_standardsService).GetStandard(request.Id);

            return new GetStandardByIdResult { Standard = standard };
        }
    }
}
