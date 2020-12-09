using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardDetail
{
    public class GetStandardDetailQueryHandler : IRequestHandler<GetStandardDetailQuery, GetStandardDetailResult>
    {
        private readonly IVersioningStandardRepository versioningStandardRepository;

        public GetStandardDetailQueryHandler(IVersioningStandardRepository versioningStandardRepository)
        {
            this.versioningStandardRepository = versioningStandardRepository;
        }

        public async Task<GetStandardDetailResult> Handle(GetStandardDetailQuery request, CancellationToken cancellationToken)
        {
            var standard = await versioningStandardRepository.GetStandardByUId(request.StandardUId);
            if (standard == null)
            {
                throw new ArgumentException("Invalid standardUId");
            }
            var detail = await versioningStandardRepository.GetStandardAdditionalInformation(request.StandardUId);
            return new GetStandardDetailResult(standard, detail);
        }
    }
}
