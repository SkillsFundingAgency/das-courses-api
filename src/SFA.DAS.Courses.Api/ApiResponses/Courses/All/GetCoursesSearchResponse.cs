using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetCoursesSearchResponse : MultipleResponse
    {
        public IEnumerable<GetCourseResponse> Standards { get; set; }
    }
}
