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

            var boolQuery = new BooleanQuery();
            
            //phrase
            boolQuery.Add(new PhraseQuery{new Term(SearchableStandard.TitlePhrase, searchTerm)}, Occur.SHOULD);
            boolQuery.Add(new PhraseQuery{new Term(SearchableStandard.TypicalJobTitlesPhrase, searchTerm)}, Occur.SHOULD);
            boolQuery.Add(new PhraseQuery{new Term(SearchableStandard.KeywordsPhrase, searchTerm)}, Occur.SHOULD);

            foreach (var term in searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                //term
                boolQuery.Add(new TermQuery(new Term(SearchableStandard.TitleSoundex, term)), Occur.SHOULD);
                boolQuery.Add(new TermQuery(new Term(SearchableStandard.TypicalJobTitlesSoundex, term)), Occur.SHOULD);
                boolQuery.Add(new TermQuery(new Term(SearchableStandard.KeywordsSoundex, term)), Occur.SHOULD);
                //soundex
                boolQuery.Add(new FuzzyQuery(new Term(SearchableStandard.TitleSoundex, term)), Occur.SHOULD);
                boolQuery.Add(new FuzzyQuery(new Term(SearchableStandard.TypicalJobTitlesSoundex, term)), Occur.SHOULD);
                boolQuery.Add(new FuzzyQuery(new Term(SearchableStandard.KeywordsSoundex, term)), Occur.SHOULD);
            }

            var directory = _directoryFactory.GetDirectory();
            var reader = DirectoryReader.Open(directory);
            var searcher = new IndexSearcher(reader);

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
