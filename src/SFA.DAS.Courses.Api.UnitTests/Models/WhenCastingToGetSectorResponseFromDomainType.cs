using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.UnitTests.Models
{
    public class WhenCastingToGetSectorResponseFromDomainType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            Sector sector)
        {
            var actual = (GetSectorResponse)sector;
            
            actual.Should().BeEquivalentTo(sector);
        }
    }
}