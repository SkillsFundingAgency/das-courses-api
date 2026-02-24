using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardsListResponse : MultipleResponse
    {
        public IEnumerable<GetStandardResponse> Standards { get; set; }
    }
}
