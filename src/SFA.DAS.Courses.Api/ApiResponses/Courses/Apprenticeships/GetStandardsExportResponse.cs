using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardsExportResponse
    {
        public IEnumerable<GetStandardDetailResponse> Standards { get; set; }
    }
}
