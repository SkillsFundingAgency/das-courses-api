using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetCourseOptionKsbs
{
    public class GetCourseOptionKsbsQuery : IRequest<GetCourseOptionKsbsResult>
    {
        public string Id { get; set; }
        public string Option { get; set; }
    }
}
