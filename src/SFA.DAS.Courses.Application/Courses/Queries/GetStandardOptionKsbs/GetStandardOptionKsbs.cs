using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandard;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Extensions;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardOptionKsbs
{
    public class GetStandardOptionKsbsQuery : IRequest<GetStandardOptionKsbsResult>
    {
        public string Id { get; set; }
        public string Option { get; set; }
    }

    public class GetStandardOptionKsbsQueryHandler : IRequestHandler<GetStandardOptionKsbsQuery, GetStandardOptionKsbsResult>
    {
        private readonly IStandardsService _standardsService;

        public GetStandardOptionKsbsQueryHandler(IStandardsService standardsService)
            => _standardsService = standardsService;

        public async Task<GetStandardOptionKsbsResult> Handle(GetStandardOptionKsbsQuery request, CancellationToken cancellationToken)
        {
            var standard = await new GetStandardByAnyId(_standardsService).GetStandard(request.Id);

            var ksbs = standard?.Options.FirstOrDefault(x => x.Title == request.Option)?.Ksbs;

            return new GetStandardOptionKsbsResult
            {
                Ksbs = ksbs.EmptyEnumerableIfNull().ToArray()
            };
        }
    }

    public class GetStandardOptionKsbsResult
    {
        public Ksb[] Ksbs { get; set; }
    }
}
