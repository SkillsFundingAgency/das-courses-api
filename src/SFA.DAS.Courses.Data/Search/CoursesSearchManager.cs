using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Index;
using Lucene.Net.Search;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Data.Search
{
    public class CoursesSearchManager : ISearchManager
    {
        private readonly IDirectoryFactory _directoryFactory;

        public CoursesSearchManager(IDirectoryFactory directoryFactory)
        {
            _directoryFactory = directoryFactory;
        }

        public StandardSearchResultsList Query(string searchTerm)
        {
            searchTerm = searchTerm.ToLowerInvariant();

            var directory = _directoryFactory.GetDirectory();
            var reader = DirectoryReader.Open(directory);
            var searcher = new IndexSearcher(reader);

            var titleQuery = new FuzzyQuery(new Term(nameof(Standard.Title), searchTerm));
            var jobTitlesQuery = new FuzzyQuery(new Term(nameof(Standard.TypicalJobTitles), searchTerm));
            var keywordsQuery = new FuzzyQuery(new Term(nameof(Standard.Keywords), searchTerm));
            
            var boolQuery = new BooleanQuery
            {
                {titleQuery, Occur.SHOULD},
                {jobTitlesQuery, Occur.SHOULD},
                {keywordsQuery, Occur.SHOULD}
            };

            var topDocs = searcher.Search(boolQuery, 1000);
            
            var results = new List<StandardSearchResult>();
            foreach (var scoreDoc in topDocs.ScoreDocs.OrderByDescending(doc => doc.Score))
            {
                var doc = searcher.Doc(scoreDoc.Doc);
                results.Add(new StandardSearchResult(doc, scoreDoc.Score));
            }

            return new StandardSearchResultsList
            {
                TotalCount = topDocs.TotalHits,
                Standards = results
            };
        }
    }
}
