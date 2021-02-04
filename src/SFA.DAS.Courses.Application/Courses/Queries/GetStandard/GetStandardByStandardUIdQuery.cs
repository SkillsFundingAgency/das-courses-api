using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public class GetStandardByStandardUIdQuery : IRequest<GetStandardByStandardUIdResult>
    {
        public string StandardUId { get; set; }
    }
}
