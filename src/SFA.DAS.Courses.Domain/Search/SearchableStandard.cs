using System.Collections.Generic;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Search
{
    public class SearchableStandard
    {
        public SearchableStandard(Standard standard)
        {
            Id = standard.Id;
            Title = standard.Title;
            TypicalJobTitles = standard.TypicalJobTitles;
            Keywords = standard.Keywords;
        }
        
        public int Id { get; }
        public string Title { get; }
        public string TypicalJobTitles { get; }
        public string Keywords { get; }

        
        private const string PhraseSuffix = "Phrase";
        private const string TermSuffix = "Term";
        private const string NGramSuffix = "NGram";

        // phrase
        public static string TitlePhrase => $"{nameof(Standard.Title)}-{PhraseSuffix}";
        public static string TypicalJobTitlesPhrase => $"{nameof(Standard.TypicalJobTitles)}-{PhraseSuffix}";
        public static string KeywordsPhrase => $"{nameof(Standard.Keywords)}-{PhraseSuffix}";
        // term
        public static string TitleTerm => $"{nameof(Standard.Title)}-{TermSuffix}";
        public static string TypicalJobTitlesTerm => $"{nameof(Standard.TypicalJobTitles)}-{TermSuffix}";
        public static string KeywordsTerm => $"{nameof(Standard.Keywords)}-{TermSuffix}";
        // n-gram
        public static string TitleNGram => $"{nameof(Standard.Title)}-{NGramSuffix}";


        public  IEnumerable<IIndexableField> GetFields()
        {
            return new Field[]
            {
                new Int32Field(nameof(Id), Id, Field.Store.YES),
                // phrase
                new TextField(TitlePhrase, Title ?? "", Field.Store.NO) {Boost = 1000f},
                new TextField(TypicalJobTitlesPhrase, TypicalJobTitles ?? "", Field.Store.NO) {Boost = 500f},
                new TextField(KeywordsPhrase, Keywords ?? "", Field.Store.NO) {Boost = 100f},
                // term
                new TextField(TitleTerm, Title ?? "", Field.Store.NO) {Boost = 40f},
                new TextField(TypicalJobTitlesTerm, TypicalJobTitles ?? "", Field.Store.NO) {Boost = 20f},
                new TextField(KeywordsTerm, Keywords ?? "", Field.Store.NO),
                // ngram
                new TextField(TitleNGram, Title ?? "", Field.Store.NO) {Boost = 10f}
            };
        }
    }
}
