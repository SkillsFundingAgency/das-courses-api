using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.UnitTests.Models;
public class WhenCastingToGetStandardDetailResponseFromStandard
{
    [Test]
    public void And_Source_Is_Null_The_Returns_Null()
    {
        var result = (GetStandardDetailResponse)null;

        result.Should().BeNull();
    }

    [Test, AutoData]
    public void Then_Maps_Fields_Appropriately(Standard standard)
    {
        var result = (GetStandardDetailResponse)standard;

        result.Should().BeEquivalentTo(standard, ExcludeProperties);
    }

    private EquivalencyAssertionOptions<Standard> ExcludeProperties(EquivalencyAssertionOptions<Standard> options)
    {
        options.Excluding(t => t.ApprenticeshipFunding);
        options.Excluding(t => t.Options);
        return options;
    }
}
