using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardOptionsListResponse
    {
        public int Total { get; set; }
        public int TotalFiltered { get; set; }
        public IEnumerable<GetStandardOptionsResponse> Standards { get; set; }
    }
}
