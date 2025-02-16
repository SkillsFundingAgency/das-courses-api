using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.UnitTests.ApiResponses.GetStandardResponseTests;

public class WhenConvertingFromStandard
{
    [Test, AutoData]
    public void ThenTransformsFromStandard(Standard standard)
    {
        GetStandardResponse sut = standard;

        sut.Should().BeEquivalentTo(standard, options => options.ExcludingMissingMembers());
    }
}
