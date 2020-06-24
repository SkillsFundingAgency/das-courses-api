using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Lucene.Net.Documents;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Domain.UnitTests.Search
{
    public class WhenCallingGetFields
    {
        [Test, AutoData]
        public void Then_Indexes_Fields(Standard source)
        {
            var expectedFields = new List<Field>
            {
                new Int32Field(nameof(Standard.Id), source.Id, Field.Store.YES),
                new TextField(nameof(Standard.Title), source.Title, Field.Store.NO) {Boost = 4.0f},
                new TextField(nameof(Standard.TypicalJobTitles), source.TypicalJobTitles, Field.Store.NO) {Boost = 2.0f},
                new TextField(nameof(Standard.Keywords), source.Keywords, Field.Store.NO)
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
                new TextField(nameof(Standard.Title), "", Field.Store.NO) {Boost = 4.0f},
                new TextField(nameof(Standard.TypicalJobTitles), "", Field.Store.NO) {Boost = 2.0f},
                new TextField(nameof(Standard.Keywords), "", Field.Store.NO)
            };

            var searchable = new SearchableStandard(source);

            var fields = searchable.GetFields().ToList();

            fields.Should().BeEquivalentTo(expectedFields);
        }
    }
}
