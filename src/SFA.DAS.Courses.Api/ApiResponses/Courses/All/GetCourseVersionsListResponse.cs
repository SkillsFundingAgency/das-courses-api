using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses.Courses.All
{
    public class GetCourseVersionsListResponse
    {
        public IEnumerable<GetCourseDetailResponse> Courses { get; set; }
    }
}
