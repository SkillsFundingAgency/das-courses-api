using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetCoursesSearchResponse : TotalResponse
    {
        public IEnumerable<GetCourseResponse> Courses { get; set; }
    }
}
