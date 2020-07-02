using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetSectorsListResponse
    {
        public IEnumerable<GetSectorResponse> Sectors { get; set; }
    }
}