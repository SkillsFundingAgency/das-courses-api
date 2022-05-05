using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardOptionKsbs
{
    public class GetStandardOptionKsbsQuery : IRequest<GetStandardOptionKsbsResult>
    {
        public string StandardId { get; set; }
        public string StandardOption { get; set; }
    }

    public class GetStandardOptionKsbsQueryHandler : IRequestHandler<GetStandardOptionKsbsQuery, GetStandardOptionKsbsResult>
    {
        public async Task<GetStandardOptionKsbsResult> Handle(GetStandardOptionKsbsQuery request, CancellationToken cancellationToken)
        {
            return new GetStandardOptionKsbsResult
            {
                KSBs = new StandardOptionKsb[]
               {
                   new StandardOptionKsb
                   {
                       Type = KsbType.Knowledge,
                       Key = "k1",
                       Detail = "core_knowledge_1",
                   }
               }
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
