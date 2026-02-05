using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsByIFateReference
{
    public class GetStandardsByIFateReferenceResult
    {
        public IEnumerable<Standard> Standards { get; set; }
    }
}
