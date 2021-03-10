using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public class GetLatestActiveStandardQuery : IRequest<GetLatestActiveStandardResult>
    {
        public int LarsCode { get ; set ; }
        public string IfateRefNumber { get; set; }
    }
}
