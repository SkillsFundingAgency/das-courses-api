using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class GetStandardsListResult
    {
        public int Total { get; set; }
        public int TotalFiltered { get; set; }
        public IEnumerable<Standard> Standards { get; set; }
    }
}
