using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Policies;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsByIFateReference
{
    public class GetStandardsByIFateReferenceQueryHandler : IRequestHandler<GetStandardsByIFateReferenceQuery, GetStandardsByIFateReferenceResult>
    {
        private readonly IStandardsService _standardsService;

        public GetStandardsByIFateReferenceQueryHandler(IStandardsService standardsService)
            => _standardsService = standardsService;

        public async Task<GetStandardsByIFateReferenceResult> Handle(GetStandardsByIFateReferenceQuery request, CancellationToken cancellationToken)
        {
            var standards = (await _standardsService.GetAllVersionsOfAStandard(request.IFateReferenceNumber))
                .Where(s => AllowedApprenticeshipTypesPolicy.IsStandard(s.ApprenticeshipType))
                .ToList();

            return new GetStandardsByIFateReferenceResult
            {
                Standards = standards
            };
        }
    }
}
