using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public class GetStandardQueryHandler : IRequestHandler<GetStandardQuery, GetStandardResult>
    {
        private readonly IStandardsService _standardsService;

        public GetStandardQueryHandler (IStandardsService standardsService)
        {
            _standardsService = standardsService;
        }
        public async Task<GetStandardResult> Handle(GetStandardQuery request, CancellationToken cancellationToken)
        {
            var standard = await _standardsService.GetStandard(request.LarsCode);
            
            return new GetStandardResult { Standard = standard };
        }
    }
}
