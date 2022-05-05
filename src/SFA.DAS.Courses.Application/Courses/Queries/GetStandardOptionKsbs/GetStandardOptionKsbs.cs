using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandard;
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
            var standard = await new GetStandardById(_standardsService).GetStandard(request.Id);

            var option = standard.Options.FirstOrDefault(x => x.Title == request.Option);

            return new GetStandardOptionKsbsResult
            {
                KSBs = option?.Knowledge.Select(x => new StandardOptionKsb
                {
                    Type = KsbType.Knowledge,
                    Key = "k1",
                    Detail = x
                }).ToArray()
            };
        }
    }

    public class GetStandardOptionKsbsResult
    {
        public StandardOptionKsb[] KSBs { get; set; }
    }

    public class StandardOptionKsb
    {
        public KsbType Type { get; set; }
        public string Key { get; set; }
        public string Detail { get; set; }
    }

    public enum KsbType
    {
        Knowledge = 1,
        Skill = 2,
        Behaviour = 3,
    }
}
