using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Api.UnitTests.Controllers.Standards;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.UnitTests.Models;
public class WhenCastingToGetStandardDetailResponseFromStandard
{
    [Test]
    public void And_Source_Is_Null_The_Returns_Null()
    {
        // Act
        var result = (GetStandardDetailResponse)null;

        // Assert
        result.Should().BeNull();
    }

    [Test, AutoData]
    public void Then_Maps_Fields_Appropriately(Standard standard)
    {
        // Arrange integer LarsCode
        standard.LarsCode = 1234.ToString();
        
        // Act
        var result = (GetStandardDetailResponse)standard;

        // Assert all properties except LarsCode
        result.Should().BeEquivalentTo(
                standard,
                options => StandardToGetStandardResponseOptions
                    .ExclusionsForGetStandardDetailResponse(options)
                    .Excluding(s => s.LarsCode)
            );

        // Assert options is a list of titles
        result.Options.Should().BeEquivalentTo(standard.Options.Select(p => p.Title));

        // Assert LarsCode is an integer
        result.LarsCode.Should().Be(int.Parse(standard.LarsCode));
    }
}
