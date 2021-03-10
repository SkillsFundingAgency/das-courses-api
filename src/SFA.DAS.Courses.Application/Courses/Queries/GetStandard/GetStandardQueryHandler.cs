using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public class GetStandardQueryHandler : IRequestHandler<GetLatestActiveStandardQuery, GetLatestActiveStandardResult>
    {
        private readonly IStandardsService _standardsService;

        public GetStandardQueryHandler(IStandardsService standardsService)
        {
            _standardsService = standardsService;
        }
        public async Task<GetLatestActiveStandardResult> Handle(GetLatestActiveStandardQuery request, CancellationToken cancellationToken)
        {
            Domain.Courses.Standard standard = null;
            if (request.LarsCode > 0)
            {
                standard = await _standardsService.GetLatestActiveStandard(request.LarsCode);
            }
            else if (!string.IsNullOrWhiteSpace(request.IfateRefNumber))
            {
                standard = await _standardsService.GetLatestActiveStandard(request.IfateRefNumber);
            }
            else
            {
                throw new InvalidOperationException($"Must specify LarsCode or IFateRefNumber when retrieving active standard.");
            }

            return new GetLatestActiveStandardResult { Standard = standard };
        }
    }
}
