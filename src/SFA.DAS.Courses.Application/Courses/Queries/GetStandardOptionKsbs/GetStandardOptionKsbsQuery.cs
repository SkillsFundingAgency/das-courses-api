using MediatR;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardOptionKsbs
{
    public class GetStandardOptionKsbsQuery : IRequest<GetStandardOptionKsbsResult>
    {
        public string Id { get; set; }
        public string Option { get; set; }
    }
}
