using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardVersionsListResponse
    {
        public IEnumerable<GetStandardDetailResponse> Standards { get; set; }
    }
}
