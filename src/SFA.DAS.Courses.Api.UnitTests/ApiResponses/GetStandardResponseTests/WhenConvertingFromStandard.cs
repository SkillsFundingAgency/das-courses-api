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
        // Act
        standard.LarsCode = 12345.ToString();
        GetStandardResponse sut = standard;

        // Assert: everything except LarsCode
        sut.Should().BeEquivalentTo(
            standard,
            options => options
                .ExcludingMissingMembers()
                .Excluding(s => s.LarsCode)
        );

        // Assert: LarsCode separately (backwards-compat behaviour)
        sut.LarsCode.Should().Be(12345);
    }
}
