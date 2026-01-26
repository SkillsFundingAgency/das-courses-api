using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Api.UnitTests.Controllers.Standards;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.UnitTests.Models
{
    public class WhenCastingToGetStandardResponseFromDomainType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            Standard source)
        {
            // Arrange integer LarsCode
            source.LarsCode = 1234.ToString();

            // Act
            var response = (GetStandardResponse)source;

            // Assert all properties except LarsCode
            response.Should().BeEquivalentTo(
                    source,
                    options => StandardToGetStandardResponseOptions
                        .ExclusionsGetStandardResponse(options)
                        .Excluding(s => s.LarsCode)
                );
            
            // Assert LarsCode is an integer
            response.LarsCode.Should().Be(int.Parse(source.LarsCode));
        }

        [Test, AutoData]
        public void Then_Maps_Unique_Skills(
            Standard source)
        {
            // Arrange integer LarsCode
            source.LarsCode = 1234.ToString();
            
            // Act
            var response = (GetStandardResponse)source;

            // Assert Skills are transferred from distinct options skills details
            response.Skills.Should().BeEquivalentTo(
                source.Options.SelectMany(x => x.Skills).Select(x => x.Detail).Distinct());
        }
    }
}
