using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Entities.Versioning;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetActiveStandardsSummaryQueryHandler : IRequestHandler<GetActiveStandardsSummaryQuery, GetActiveStandardsSummaryResult>
    {
        private readonly IVersioningStandardRepository versioningStandardRepository;

        public GetActiveStandardsSummaryQueryHandler(IVersioningStandardRepository versioningStandardRepository)
        {
            this.versioningStandardRepository = versioningStandardRepository;
        }

        public async Task<GetActiveStandardsSummaryResult> Handle(GetActiveStandardsSummaryQuery request, CancellationToken cancellationToken)
        {
            var result = new GetActiveStandardsSummaryResult();

            var allStandards = await versioningStandardRepository.GetAllActiveStandardsSummary();

            result.Standards = allStandards
                .Where(c => c.LarsCode > 0 && c.Status.Equals("Approved for Delivery", StringComparison.CurrentCultureIgnoreCase))
                .GroupBy(c => c.LarsCode)
                .Select(c => c.OrderByDescending(x => x.Version).FirstOrDefault())
                .Select(c => (Standard)c);

            return result;
        }
    }
}
