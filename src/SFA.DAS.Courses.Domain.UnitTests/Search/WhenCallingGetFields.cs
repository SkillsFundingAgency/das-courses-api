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
                new Int32Field(nameof(Standard.Id), source.Id, Field.Store.YES),
                new TextField(SearchableStandard.TitlePhrase, source.Title, Field.Store.NO) {Boost = 32.0f},
                new TextField(SearchableStandard.TypicalJobTitlesPhrase, source.TypicalJobTitles, Field.Store.NO) {Boost = 16.0f},
                new TextField(SearchableStandard.KeywordsPhrase, source.Keywords, Field.Store.NO) {Boost = 8.0f},
                new TextField(SearchableStandard.TitleSoundex, source.Title, Field.Store.NO) {Boost = 4.0f},
                new TextField(SearchableStandard.TypicalJobTitlesSoundex, source.TypicalJobTitles, Field.Store.NO) {Boost = 2.0f},
                new TextField(SearchableStandard.KeywordsSoundex, source.Keywords, Field.Store.NO)
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
                new Int32Field(nameof(Standard.Id), source.Id, Field.Store.YES),
                new TextField(SearchableStandard.TitlePhrase, "", Field.Store.NO) {Boost = 32.0f},
                new TextField(SearchableStandard.TypicalJobTitlesPhrase, "", Field.Store.NO) {Boost = 16.0f},
                new TextField(SearchableStandard.KeywordsPhrase, "", Field.Store.NO) {Boost = 8.0f},
                new TextField(SearchableStandard.TitleSoundex, "", Field.Store.NO) {Boost = 4.0f},
                new TextField(SearchableStandard.TypicalJobTitlesSoundex, "", Field.Store.NO) {Boost = 2.0f},
                new TextField(SearchableStandard.KeywordsSoundex, "", Field.Store.NO)
            };

            var searchable = new SearchableStandard(source);

            var fields = searchable.GetFields().ToList();

            fields.Should().BeEquivalentTo(expectedFields);
        }
    }
}
