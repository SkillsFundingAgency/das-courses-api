using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardOptionsListResponse
    {
        public IEnumerable<GetStandardOptionsResponse> Standards { get; set; }
    }
}
