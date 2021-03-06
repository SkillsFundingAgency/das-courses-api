﻿using FluentAssertions;
using Lucene.Net.Documents;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Search
{
    public class WhenConstructingAStandardSearchResult
    {
        [Test, RecursiveMoqAutoData]
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

            result.StandardUId.Should().Be(document.GetField(nameof(StandardSearchResult.StandardUId)).GetStringValue());
            result.Score.Should().Be(score);
        }
    }
}
