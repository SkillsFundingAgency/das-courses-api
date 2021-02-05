using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardsListResponse
    {
        public int Total { get; set; }
        public int TotalFiltered { get; set; }
        public IEnumerable<GetStandardResponse> Standards { get; set; }
    }
}
