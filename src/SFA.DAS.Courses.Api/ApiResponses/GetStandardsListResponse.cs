using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardResponse> Standards { get; set; }
    }
}
