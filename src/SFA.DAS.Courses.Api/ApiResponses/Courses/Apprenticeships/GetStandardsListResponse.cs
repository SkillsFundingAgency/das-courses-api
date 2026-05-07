using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardsListResponse : TotalResponse
    {
        public IEnumerable<GetStandardResponse> Standards { get; set; }
    }
}
