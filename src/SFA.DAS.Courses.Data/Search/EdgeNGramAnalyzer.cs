using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.NGram;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace SFA.DAS.Courses.Data.Search
{
    public class EdgeNGramAnalyzer : Analyzer
    {
        private const LuceneVersion MatchLuceneVersion = LuceneVersion.LUCENE_48;
        private const int MinGramSize = 3;
        private const int MaxGramSize = 25;

        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            var tokenizer = new StandardTokenizer(MatchLuceneVersion, reader);
            TokenStream tokenStream = new StandardFilter(MatchLuceneVersion, tokenizer);
            tokenStream = new LowerCaseFilter(MatchLuceneVersion, tokenStream);
            tokenStream = new StopFilter(MatchLuceneVersion, tokenStream, StopAnalyzer.ENGLISH_STOP_WORDS_SET);
            tokenStream = new EdgeNGramTokenFilter(MatchLuceneVersion, tokenStream, MinGramSize, MaxGramSize);

            return new TokenStreamComponents(tokenizer, tokenStream);
        }
    }
}
