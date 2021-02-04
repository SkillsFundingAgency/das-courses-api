using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetStandardsByIFateReferenceResult
    {
        public int Total { get; set; }
        public int TotalFiltered { get; set; }
        public IEnumerable<Standard> Standards { get; set; }
    }
}
