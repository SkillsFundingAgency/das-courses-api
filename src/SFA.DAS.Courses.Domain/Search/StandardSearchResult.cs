using Lucene.Net.Documents;

namespace SFA.DAS.Courses.Domain.Search
{
    public class StandardSearchResult
    {
        public StandardSearchResult() {}

        public StandardSearchResult(Document document,
            float score)
        {
            StandardUId = document.GetField(nameof(StandardUId)).GetStringValue();
            Score = score;
        }

        public string StandardUId { get; set; }
        public float Score { get; set; }
    }
}
