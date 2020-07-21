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
            
            var titlePhraseQuery = new PhraseQuery{new Term(SearchableStandard.TitlePhrase, searchTerm)};
            var jobTitlesPhraseQuery = new PhraseQuery{new Term(SearchableStandard.TypicalJobTitlesPhrase, searchTerm)};
            var keywordsPhraseQuery = new PhraseQuery{new Term(SearchableStandard.KeywordsPhrase, searchTerm)};

            var titleSoundexQuery = new FuzzyQuery(new Term(SearchableStandard.TitleSoundex, searchTerm));
            var jobTitlesSoundexQuery = new FuzzyQuery(new Term(SearchableStandard.TypicalJobTitlesSoundex, searchTerm));
            var keywordsSoundexQuery = new FuzzyQuery(new Term(SearchableStandard.KeywordsSoundex, searchTerm));
            
            var boolQuery = new BooleanQuery
            {
                //phrase
                {titlePhraseQuery, Occur.SHOULD},
                {jobTitlesPhraseQuery, Occur.SHOULD},
                {keywordsPhraseQuery, Occur.SHOULD},
                //soundex
                {titleSoundexQuery, Occur.SHOULD},
                {jobTitlesSoundexQuery, Occur.SHOULD},
                {keywordsSoundexQuery, Occur.SHOULD}
            };

            var topDocs = searcher.Search(boolQuery, 1000);
            
            var results = new List<StandardSearchResult>();
            foreach (var scoreDoc in topDocs.ScoreDocs)
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
