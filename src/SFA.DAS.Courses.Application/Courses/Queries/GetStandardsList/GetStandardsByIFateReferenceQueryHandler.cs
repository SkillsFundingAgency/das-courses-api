using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetStandardsByIFateReferenceHandler : IRequestHandler<GetStandardsByIFateReferenceQuery, GetStandardsByIFateReferenceResult>
    {
        private readonly ILogger<GetStandardsByIFateReferenceHandler> _logger;
        private readonly IStandardsService _standardsService;

        public GetStandardsByIFateReferenceHandler(
            ILogger<GetStandardsByIFateReferenceHandler> logger,
            IStandardsService standardsService)
        {
            _logger = logger;
            _standardsService = standardsService;
        }

        public async Task<GetStandardsByIFateReferenceResult> Handle(GetStandardsByIFateReferenceQuery request, CancellationToken cancellationToken)
        {
            var standards = (await _standardsService.GetAllVersionsOfAStandard(request.IFateReferenceNumber)).ToList();
            var total = await _standardsService.Count();

            return new GetStandardsByIFateReferenceResult
            {
                Standards = standards,
                Total = total,
                TotalFiltered = standards.Count
            };
        }
    }
}
