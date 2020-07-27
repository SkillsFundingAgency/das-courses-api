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
        private const string SoundexSuffix = "Soundex";

        // phrase
        public static string TitlePhrase => $"{nameof(Standard.Title)}-{PhraseSuffix}";
        public static string TypicalJobTitlesPhrase => $"{nameof(Standard.TypicalJobTitles)}-{PhraseSuffix}";
        public static string KeywordsPhrase => $"{nameof(Standard.Keywords)}-{PhraseSuffix}";
        // term
        public static string TitleTerm => $"{nameof(Standard.Title)}-{TermSuffix}";
        public static string TypicalJobTitlesTerm => $"{nameof(Standard.TypicalJobTitles)}-{TermSuffix}";
        public static string KeywordsTerm => $"{nameof(Standard.Keywords)}-{TermSuffix}";
        // soundex
        public static string TitleSoundex => $"{nameof(Standard.Title)}-{SoundexSuffix}";
        public static string TypicalJobTitlesSoundex => $"{nameof(Standard.TypicalJobTitles)}-{SoundexSuffix}";
        public static string KeywordsSoundex => $"{nameof(Standard.Keywords)}-{SoundexSuffix}";

        public  IEnumerable<IIndexableField> GetFields()
        {
            return new Field[]
            {
                new Int32Field(nameof(Id), Id, Field.Store.YES),
                // phrase
                new TextField(TitlePhrase, Title ?? "", Field.Store.NO) {Boost = 25.0f},
                new TextField(TypicalJobTitlesPhrase, TypicalJobTitles ?? "", Field.Store.NO) {Boost = 20.0f},
                new TextField(KeywordsPhrase, Keywords ?? "", Field.Store.NO) {Boost = 1.6f},
                // term
                new TextField(TitleTerm, Title ?? "", Field.Store.NO) {Boost = 12.0f},
                new TextField(TypicalJobTitlesTerm, TypicalJobTitles ?? "", Field.Store.NO) {Boost = 10.0f},
                new TextField(KeywordsTerm, Keywords ?? "", Field.Store.NO) {Boost = 1.5f},
                // soundex
                new TextField(TitleSoundex, Title ?? "", Field.Store.NO) {Boost = 0.04f},
                new TextField(TypicalJobTitlesSoundex, TypicalJobTitles ?? "", Field.Store.NO) {Boost = 0.02f},
                new TextField(KeywordsSoundex, Keywords ?? "", Field.Store.NO) {Boost = 0.01f}
            };
        }
    }
}
