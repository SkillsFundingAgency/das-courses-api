using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
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

            // todo: 
            // use same analyser for the search term
            // use nlog to support fragments

            var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            var queryBuilder = new QueryBuilder(analyzer);

            var titleQuery = new FuzzyQuery(new Term(SearchableStandard.TitleSoundex, searchTerm));
            var jobTitlesQuery = new FuzzyQuery(new Term(SearchableStandard.TypicalJobTitlesSoundex, searchTerm));
            var keywordsQuery = new FuzzyQuery(new Term(SearchableStandard.KeywordsSoundex, searchTerm));
            
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
