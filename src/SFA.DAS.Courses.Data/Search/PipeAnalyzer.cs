using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Util;

namespace SFA.DAS.Courses.Data.Search
{
    public class PipeAnalyzer: Analyzer
    {
        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            var tokenizer = new PipeTokenizer(LuceneVersion.LUCENE_48, reader);
            var lowerCaseFilter = new LowerCaseFilter(LuceneVersion.LUCENE_48, tokenizer); 
            return new TokenStreamComponents(tokenizer, lowerCaseFilter);
        }
    }
}
