using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public class GetStandardQuery : IRequest<GetStandardResult>
    {
        public int LarsCode { get ; set ; }
    }
}
