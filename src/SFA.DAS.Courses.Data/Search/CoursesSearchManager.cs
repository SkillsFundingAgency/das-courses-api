using System;
using System.Collections.Generic;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Util;
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
            searchTerm = searchTerm.ToLowerInvariant().Trim();
            var splitSearchTerm = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            var directory = _directoryFactory.GetDirectory();
            var reader = DirectoryReader.Open(directory);
            var searcher = new IndexSearcher(reader);
            
            //phrase
            var titlePhraseQuery = new PhraseQuery{new Term(SearchableStandard.TitlePhrase, searchTerm)};
            var jobTitlesPhraseQuery = new PhraseQuery{new Term(SearchableStandard.TypicalJobTitlesPhrase, searchTerm)};
            var keywordsPhraseQuery = new PhraseQuery{new Term(SearchableStandard.KeywordsPhrase, searchTerm)};

            var boolQuery = new BooleanQuery
            {
                //phrase
                {titlePhraseQuery, Occur.SHOULD},
                {jobTitlesPhraseQuery, Occur.SHOULD},
                {keywordsPhraseQuery, Occur.SHOULD}
            };

            //soundex
            foreach (var term in splitSearchTerm)
            {
                var titleSoundexQuery = new FuzzyQuery(new Term(SearchableStandard.TitleSoundex, term));
                var jobTitlesSoundexQuery = new FuzzyQuery(new Term(SearchableStandard.TypicalJobTitlesSoundex, term));
                var keywordsSoundexQuery = new FuzzyQuery(new Term(SearchableStandard.KeywordsSoundex, term));

                boolQuery.Add(titleSoundexQuery, Occur.SHOULD);
                boolQuery.Add(jobTitlesSoundexQuery, Occur.SHOULD);
                boolQuery.Add(keywordsSoundexQuery, Occur.SHOULD);
            }

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
