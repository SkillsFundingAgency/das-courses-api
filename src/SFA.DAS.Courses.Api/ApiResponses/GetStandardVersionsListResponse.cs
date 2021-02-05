using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardVersionsListResponse
    {
        public IEnumerable<GetStandardResponse> Standards { get; set; }
    }
}
