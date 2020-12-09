using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetOptions
{
    public class GetStandardOptionsQueryHandler : IRequestHandler<GetStandardOptionsQuery, GetStandardOptionsResult>
    {
        private readonly IVersioningStandardRepository versioningStandardRepository;

        public GetStandardOptionsQueryHandler(IVersioningStandardRepository versioningStandardRepository)
        {
            this.versioningStandardRepository = versioningStandardRepository;
        }

        public async Task<GetStandardOptionsResult> Handle(GetStandardOptionsQuery request, CancellationToken cancellationToken)
        {
            var result = new GetStandardOptionsResult() { StandardUId = request.StandardUId};
            var details = await versioningStandardRepository.GetStandardAdditionalInformation(request.StandardUId);
            result.Options = details.Options?.ToArray() ?? Array.Empty<string>(); ;
            return result;
        }
    }
}
