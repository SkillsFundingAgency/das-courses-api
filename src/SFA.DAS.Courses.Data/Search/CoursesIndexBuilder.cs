using System.Linq;
using J2N.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.NGram;
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
            var pipeAnalyzer = new PipeAnalyzer();
            var fieldAnalyzers = new Dictionary<string, Analyzer>
            {
                //phrase
                {SearchableStandard.TitlePhrase, pipeAnalyzer},
                {SearchableStandard.TypicalJobTitlesPhrase, pipeAnalyzer},
                {SearchableStandard.KeywordsPhrase, pipeAnalyzer},
                //term
                {SearchableStandard.TitleTerm, standardAnalyzer},
                {SearchableStandard.TypicalJobTitlesTerm, standardAnalyzer},
                {SearchableStandard.KeywordsTerm, standardAnalyzer},
                //ngram
                {SearchableStandard.TitleNGram, standardAnalyzer},
                {SearchableStandard.TypicalJobTitlesNGram, standardAnalyzer},
                {SearchableStandard.KeywordsNGram, standardAnalyzer},
                //soundex
                {SearchableStandard.TitleSoundex, standardAnalyzer}
            };
            var perFieldAnalyzerWrapper = new PerFieldAnalyzerWrapper(standardAnalyzer, fieldAnalyzers);

            var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, perFieldAnalyzerWrapper);
            var directory = _directoryFactory.GetDirectory();
            
            using (var writer = new IndexWriter(directory, config))
            {
                writer.DeleteAll();
                writer.Commit();

                foreach (var standard in _coursesDataContext.Standards)
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
