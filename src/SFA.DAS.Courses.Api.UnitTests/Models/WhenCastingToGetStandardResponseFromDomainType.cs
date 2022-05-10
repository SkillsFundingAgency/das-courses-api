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
            source.OptionsIncludingCore.First().Knowledge.AddRange(source.OptionsIncludingCore.First().Knowledge);

            var response = (GetStandardResponse)source;

            response.Knowledge.Should().BeEquivalentTo(source.OptionsIncludingCore.SelectMany(x => x.Knowledge).Distinct());
        }
    }
}
