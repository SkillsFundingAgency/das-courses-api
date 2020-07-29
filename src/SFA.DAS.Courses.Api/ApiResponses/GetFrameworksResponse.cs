using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetFrameworksResponse
    {
        public IEnumerable<GetFrameworkResponse> Frameworks { get; set; }
    }
}