using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetCourse
{
    public class GetCourseByIdQuery : IRequest<GetCourseByIdQueryResult>
    {
        public string Id { get; set; }
    }
}
