using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetCourse
{
    public class GetCourseByIdQuery : IRequest<GetCourseByIdResult>
    {
        public string Id { get; set; }
    }
}
