using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Search
{
    public class WhenConstructingSearchableStandard
    {
        [Test, RecursiveMoqAutoData]
        public void Then_Assigns_Matching_Fields(Standard source)
        {
            var searchable = new SearchableStandard(source);

            searchable.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());
        }
    }
}
