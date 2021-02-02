using Lucene.Net.Documents;

namespace SFA.DAS.Courses.Domain.Search
{
    public class StandardSearchResult
    {
        public StandardSearchResult() {}

        public StandardSearchResult(Document document,
            float score)
        {
            Id = document.GetField(nameof(Id)).GetInt32Value().GetValueOrDefault();
            Score = score;
        }

        public int Id { get; set; }
        public float Score { get; set; }
    }
}
