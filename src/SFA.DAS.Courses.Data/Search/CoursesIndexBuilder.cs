using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Util;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Data.Search
{
    public class CoursesIndexBuilder : IIndexBuilder
    {
        private readonly ICoursesDataContext _coursesDataContext;
        private readonly IDirectoryFactory _directoryFactory;

        public CoursesIndexBuilder(
            ICoursesDataContext coursesDataContext,
            IDirectoryFactory directoryFactory)
        {
            _coursesDataContext = coursesDataContext;
            _directoryFactory = directoryFactory;
        }

        public void Build()
        {
            var standardAnalyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, standardAnalyzer);
            var directory = _directoryFactory.GetDirectory();
            
            using (var writer = new IndexWriter(directory, config))
            {
                foreach (var standard in _coursesDataContext.Standards.OrderBy(standard => standard.Title))
                {
                    var doc = new Document();
                    var searchable = new SearchableStandard(standard);

                    foreach (var indexableField in searchable.GetFields())
                    {
                        doc.Add(indexableField);
                    }

                    writer.AddDocument(doc);
                    writer.Commit();
                }
            }
        }
    }
}
