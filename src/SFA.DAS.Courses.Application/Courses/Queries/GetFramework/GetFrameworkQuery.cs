using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetFramework
{
    public class GetFrameworkQuery : IRequest<GetFrameworkResult>
    {
        public string FrameworkId { get ; set ; }
    }
}