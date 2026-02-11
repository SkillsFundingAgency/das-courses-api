using System.Collections.Generic;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetCoursesSearchResponse : CourseSearchResponseBase
    {
        public IEnumerable<GetCourseResponse> Standards { get; set; }
    }
}
