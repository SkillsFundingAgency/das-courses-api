using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetLevels
{
    public class GetLevelsListResult
    {
        public IEnumerable<Level> Levels { get; set; }
    }
}
