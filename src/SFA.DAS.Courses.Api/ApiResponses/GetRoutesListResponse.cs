using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetRoutesListResponse
    {
        public IEnumerable<GetRouteResponse> Routes { get; set; }
    }
}