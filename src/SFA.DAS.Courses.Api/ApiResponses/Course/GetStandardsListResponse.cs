using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardsListResponse : CourseSearchResponseBase
    {
        public IEnumerable<GetStandardResponse> Standards { get; set; }
    }
}
