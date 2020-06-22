using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Search
{
    public class StandardSearchResultsList
    {
        public int TotalCount { get; set; }
        public IEnumerable<StandardSearchResult> Standards { get; set; } = new List<StandardSearchResult>();
    }
}
