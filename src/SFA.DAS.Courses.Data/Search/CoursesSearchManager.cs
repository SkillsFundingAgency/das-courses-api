using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.TokenAttributes;
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

            var ngramSearchTerms = TokenizeQuery(new EdgeNGramAnalyzer(), searchTerm);

            var boolQuery = new BooleanQuery();
            
            //phrase
            boolQuery.Add(new PhraseQuery{new Term(SearchableStandard.TitlePhrase, searchTerm)}, Occur.SHOULD);
            boolQuery.Add(new PhraseQuery{new Term(SearchableStandard.TypicalJobTitlesPhrase, searchTerm)}, Occur.SHOULD);
            boolQuery.Add(new PhraseQuery{new Term(SearchableStandard.KeywordsPhrase, searchTerm)}, Occur.SHOULD);

            //term
            foreach (var term in searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                boolQuery.Add(new TermQuery(new Term(SearchableStandard.TitleTerm, term)), Occur.SHOULD);
                boolQuery.Add(new TermQuery(new Term(SearchableStandard.TypicalJobTitlesTerm, term)), Occur.SHOULD);
                boolQuery.Add(new TermQuery(new Term(SearchableStandard.KeywordsTerm, term)), Occur.SHOULD);
            }

            //ngram
            foreach (var term in ngramSearchTerms)
            {
                boolQuery.Add(new TermQuery(new Term(SearchableStandard.TitleNGram, term)), Occur.SHOULD);
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

        private IEnumerable<string> TokenizeQuery(Analyzer analyzer, string query)
        {
            var result = new List<string>();

            using TokenStream tokenStream = analyzer.GetTokenStream(null, new StringReader(query));
            tokenStream.Reset();
            while (tokenStream.IncrementToken())
            {
                result.Add(tokenStream.GetAttribute<ICharTermAttribute>().ToString());
            }

            return result;
        }
    }
}
