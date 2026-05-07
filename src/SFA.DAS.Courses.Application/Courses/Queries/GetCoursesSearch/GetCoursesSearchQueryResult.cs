using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetCoursesSearch;

public class GetCoursesSearchQueryResult
{
    public int Total { get; set; }
    public int TotalFiltered { get; set; }
    public IEnumerable<Course> Courses { get; set; }
}
