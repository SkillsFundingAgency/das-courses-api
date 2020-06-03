using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
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

            response.Should().BeEquivalentTo(source);
        }
    }
}
