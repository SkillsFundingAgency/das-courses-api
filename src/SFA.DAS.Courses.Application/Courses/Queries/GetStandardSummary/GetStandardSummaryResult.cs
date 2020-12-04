using SFA.DAS.Courses.Domain.Entities.Versioning;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardSummary
{
    public class GetStandardSummaryResult
    {
        public Standard Standard { get; internal set; }
        public GetStandardSummaryResult(Standard standard)
        {
            Standard = standard;
        }
    }
}
