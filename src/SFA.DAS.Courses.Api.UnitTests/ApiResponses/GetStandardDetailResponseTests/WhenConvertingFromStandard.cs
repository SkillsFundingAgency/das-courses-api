using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.UnitTests.ApiResponses.GetStandardDetailResponseTests;

public class WhenConvertingFromStandard
{
    [Test, AutoData]
    public void ThenTransformsFromStandard(Standard standard)
    {
        GetStandardDetailResponse sut = standard;

        sut.Should().BeEquivalentTo(standard, options => options.ExcludingMissingMembers());
    }
}
