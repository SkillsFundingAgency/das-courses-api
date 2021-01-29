using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Lucene.Net.Documents;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Search
{
    public class WhenCallingGetFields
    {
        [Test, RecursiveMoqAutoData]
        public void Then_Indexes_Fields(Standard source)
        {
            var expectedFields = new List<Field>
            {
                new Int32Field(nameof(SearchableStandard.Id), source.LarsCode, Field.Store.YES),
                // phrase
                new TextField(SearchableStandard.TitlePhrase, source.Title, Field.Store.NO) {Boost = 1000f},
                new TextField(SearchableStandard.TypicalJobTitlesPhrase, source.TypicalJobTitles, Field.Store.NO) {Boost = 500f},
                new TextField(SearchableStandard.KeywordsPhrase, source.Keywords, Field.Store.NO) {Boost = 100f},
                // term
                new TextField(SearchableStandard.TitleTerm, source.Title, Field.Store.NO) {Boost = 40f},
                new TextField(SearchableStandard.TypicalJobTitlesTerm, source.TypicalJobTitles, Field.Store.NO) {Boost = 20f},
                new TextField(SearchableStandard.KeywordsTerm, source.Keywords, Field.Store.NO) ,
                // ngram
                new TextField(SearchableStandard.TitleNGram, source.Title, Field.Store.NO) {Boost = 10f},
            };

            var searchable = new SearchableStandard(source);

            var fields = searchable.GetFields().ToList();

            fields.Should().BeEquivalentTo(expectedFields);
        }

        [Test, AutoData]
        public void Then_Indexes_Nulls_To_Empty_String(int id)
        {
            var source = new Standard { LarsCode = id}; 
            var expectedFields = new List<Field>
            {
                new Int32Field(nameof(SearchableStandard.Id), source.LarsCode, Field.Store.YES),
                // phrase
                new TextField(SearchableStandard.TitlePhrase, "", Field.Store.NO) {Boost = 1000f},
                new TextField(SearchableStandard.TypicalJobTitlesPhrase, "", Field.Store.NO) {Boost = 500f},
                new TextField(SearchableStandard.KeywordsPhrase, "", Field.Store.NO) {Boost = 100f},
                // term
                new TextField(SearchableStandard.TitleTerm, "", Field.Store.NO) {Boost = 40f},
                new TextField(SearchableStandard.TypicalJobTitlesTerm, "", Field.Store.NO) {Boost = 20f},
                new TextField(SearchableStandard.KeywordsTerm, "", Field.Store.NO),
                // soundex
                new TextField(SearchableStandard.TitleNGram, "", Field.Store.NO) {Boost = 10f}
            };

            var searchable = new SearchableStandard(source);

            var fields = searchable.GetFields().ToList();

            fields.Should().BeEquivalentTo(expectedFields);
        }
    }
}
