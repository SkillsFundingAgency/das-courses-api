using System.IO;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Util;

namespace SFA.DAS.Courses.Data.Search
{
    public sealed class PipeTokenizer: CharTokenizer
    {
        public PipeTokenizer(LuceneVersion matchVersion, TextReader input) : 
            base(matchVersion, input)
        {
        }

        public PipeTokenizer(LuceneVersion matchVersion, AttributeFactory factory, TextReader input) : 
            base(matchVersion, factory, input)
        {
        }

        protected override bool IsTokenChar(int c)
        {
            return !((char)c).Equals('|');
        }
    }
}
