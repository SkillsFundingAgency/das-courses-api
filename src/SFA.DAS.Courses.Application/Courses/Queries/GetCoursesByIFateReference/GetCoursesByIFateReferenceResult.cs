using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetCoursesByIFateReference
{
    public class GetCoursesByIFateReferenceResult
    {
        public IEnumerable<Course> Courses { get; set; }
    }
}
