using MediatR;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardSummary
{
    public struct GetStandardSummaryQuery : IRequest<GetStandardSummaryResult>
    {
        public string StandardUId { get; internal set; }
        public GetStandardSummaryQuery(string standardUId)
        {
            StandardUId = standardUId;
        }
    }
}
