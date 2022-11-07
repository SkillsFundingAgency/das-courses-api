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
            var response = (GetStandardResponse)source;

            response.Should().BeEquivalentTo(source, StandardToGetStandardResponseOptions.Exclusions);
        }

        [Test, AutoData]
        public void Then_Maps_KSBs_Uniques(
            Standard source)
        {
            source.Options.First().Skills.AddRange(source.Options.First().Skills);

            var response = (GetStandardResponse)source;

            response.Skills.Should().BeEquivalentTo(
                source.Options.SelectMany(x => x.Skills).Select(x => x.Detail).Distinct());
        }
    }
}
