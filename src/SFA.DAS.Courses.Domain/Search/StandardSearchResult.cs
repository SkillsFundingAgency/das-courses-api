using Lucene.Net.Documents;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Search
{
    public class StandardSearchResult
    {
        public StandardSearchResult()
        {
            
        }

        public StandardSearchResult(Document document,
            float score)
        {
            Id = document.GetField(nameof(Standard.Id)).GetInt32Value().GetValueOrDefault();
            Score = score;
        }

        public int Id { get; set; }
        public float Score { get; set; }
    }
}
