using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetLevelsListResponse
    {
        public IEnumerable<GetLevelResponse> Levels { get; set; }
    }
}
