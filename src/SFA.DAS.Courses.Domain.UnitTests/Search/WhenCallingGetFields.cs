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
                new Int32Field(nameof(SearchableStandard.Id), source.Id, Field.Store.YES),
                // phrase
                new TextField(SearchableStandard.TitlePhrase, source.Title, Field.Store.NO) {Boost = 25.0f},
                new TextField(SearchableStandard.TypicalJobTitlesPhrase, source.TypicalJobTitles, Field.Store.NO) {Boost = 20.0f},
                new TextField(SearchableStandard.KeywordsPhrase, source.Keywords, Field.Store.NO) {Boost = 1.6f},
                // term
                new TextField(SearchableStandard.TitleTerm, source.Title, Field.Store.NO) {Boost = 12.0f},
                new TextField(SearchableStandard.TypicalJobTitlesTerm, source.TypicalJobTitles, Field.Store.NO) {Boost = 10.0f},
                new TextField(SearchableStandard.KeywordsTerm, source.Keywords, Field.Store.NO) {Boost = 1.5f},
                // soundex
                new TextField(SearchableStandard.TitleSoundex, source.Title, Field.Store.NO) {Boost = 0.04f},
                new TextField(SearchableStandard.TypicalJobTitlesSoundex, source.TypicalJobTitles, Field.Store.NO) {Boost = 0.02f},
                new TextField(SearchableStandard.KeywordsSoundex, source.Keywords, Field.Store.NO) {Boost = 0.01f}
            };

            var searchable = new SearchableStandard(source);

            var fields = searchable.GetFields().ToList();

            fields.Should().BeEquivalentTo(expectedFields);
        }

        [Test, AutoData]
        public void Then_Indexes_Nulls_To_Empty_String(int id)
        {
            var source = new Standard {Id = id}; 
            var expectedFields = new List<Field>
            {
                new Int32Field(nameof(SearchableStandard.Id), source.Id, Field.Store.YES),
                // phrase
                new TextField(SearchableStandard.TitlePhrase, "", Field.Store.NO) {Boost = 25.0f},
                new TextField(SearchableStandard.TypicalJobTitlesPhrase, "", Field.Store.NO) {Boost = 20.0f},
                new TextField(SearchableStandard.KeywordsPhrase, "", Field.Store.NO) {Boost = 1.6f},
                // term
                new TextField(SearchableStandard.TitleTerm, "", Field.Store.NO) {Boost = 12.0f},
                new TextField(SearchableStandard.TypicalJobTitlesTerm, "", Field.Store.NO) {Boost = 10.0f},
                new TextField(SearchableStandard.KeywordsTerm, "", Field.Store.NO) {Boost = 1.5f},
                // soundex
                new TextField(SearchableStandard.TitleSoundex, "", Field.Store.NO) {Boost = 0.04f},
                new TextField(SearchableStandard.TypicalJobTitlesSoundex, "", Field.Store.NO) {Boost = 0.02f},
                new TextField(SearchableStandard.KeywordsSoundex, "", Field.Store.NO) {Boost = 0.01f}
            };

            var searchable = new SearchableStandard(source);

            var fields = searchable.GetFields().ToList();

            fields.Should().BeEquivalentTo(expectedFields);
        }
    }
}
