using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetFrameworks
{
    public class GetFrameworksResult
    {
        public IEnumerable<Framework> Frameworks { get; set; }
    }
}