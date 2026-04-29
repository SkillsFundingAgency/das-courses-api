using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetCoursesByIFateReference
{
    public class GetCoursesByIFateReferenceQuery : IRequest<GetCoursesByIFateReferenceResult>
    {
        public string IFateReferenceNumber { get; set; }
    }
}
