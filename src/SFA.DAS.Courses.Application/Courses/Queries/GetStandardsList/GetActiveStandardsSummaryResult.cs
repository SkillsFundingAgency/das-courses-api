using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Entities.Versioning;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList
{
    public class GetActiveStandardsSummaryResult
    {
        public IEnumerable<Standard> Standards { get; set; }
    }
}
