using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardSummary
{
    public class GetStandardSummaryQueryHandler : IRequestHandler<GetStandardSummaryQuery, GetStandardSummaryResult>
    {
        private readonly IVersioningStandardRepository versioningStandardRepository;

        public GetStandardSummaryQueryHandler(IVersioningStandardRepository versioningStandardRepository)
        {
            this.versioningStandardRepository = versioningStandardRepository;
        }

        public async Task<GetStandardSummaryResult> Handle(GetStandardSummaryQuery request, CancellationToken cancellationToken)
        {
            var standard = await versioningStandardRepository.GetStandardByUId(request.StandardUId);
            if (standard == null)
            {
                throw new ArgumentException("Invalid standardUId");
            }
            return new GetStandardSummaryResult(standard);
        }
    }
}
