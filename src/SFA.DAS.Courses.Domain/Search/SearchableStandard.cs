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

        private const string SoundexSuffix = "Soundex";
        private const string PhraseSuffix = "Phrase";

        public static string TitleSoundex => $"{nameof(Standard.Title)}-{SoundexSuffix}";
        public static string TypicalJobTitlesSoundex => $"{nameof(Standard.TypicalJobTitles)}-{SoundexSuffix}";
        public static string KeywordsSoundex => $"{nameof(Standard.Keywords)}-{SoundexSuffix}";

        public static string TitlePhrase => $"{nameof(Standard.Title)}-{PhraseSuffix}";
        public static string TypicalJobTitlesPhrase => $"{nameof(Standard.TypicalJobTitles)}-{PhraseSuffix}";
        public static string KeywordsPhrase => $"{nameof(Standard.Keywords)}-{PhraseSuffix}";

        public  IEnumerable<IIndexableField> GetFields()
        {
            return new Field[]
            {
                new Int32Field(nameof(Standard.Id), Id, Field.Store.YES),
                // phrase
                new TextField(TitlePhrase, Title ?? "", Field.Store.NO) {Boost = 32.0f},
                new TextField(TypicalJobTitlesPhrase, TypicalJobTitles ?? "", Field.Store.NO) {Boost = 16.0f},
                new TextField(KeywordsPhrase, Keywords ?? "", Field.Store.NO) {Boost = 1.5f},
                // soundex
                new TextField(TitleSoundex, Title ?? "", Field.Store.NO) {Boost = 6.0f},
                new TextField(TypicalJobTitlesSoundex, TypicalJobTitles ?? "", Field.Store.NO) {Boost = 3.0f},
                new TextField(KeywordsSoundex, Keywords ?? "", Field.Store.NO)
            };
        }
    }
}
