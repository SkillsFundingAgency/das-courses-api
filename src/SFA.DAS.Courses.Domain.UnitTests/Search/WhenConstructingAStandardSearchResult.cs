using AutoFixture.NUnit3;
using FluentAssertions;
using Lucene.Net.Documents;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Domain.UnitTests.Search
{
    public class WhenConstructingAStandardSearchResult
    {
        [Test, AutoData]
        public void Then_Assigns_Properties(
            SearchableStandard searchableStandard,
            float score)
        {
            var document = new Document();
            foreach (var field in searchableStandard.GetFields())
            {
                document.Add(field);
            }

            var result = new StandardSearchResult(document, score);

            result.Id.Should().Be(document.GetField(nameof(Standard.Id)).GetInt32Value().GetValueOrDefault());
            result.Score.Should().Be(score);
        }
    }
}
