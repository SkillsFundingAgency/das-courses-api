using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Api.ApiResponses;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.UnitTests.Models
{
    public class WhenCastingToGetRouteResponseFromDomainType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(Route source)
        {
            //Act
            var actual = (GetRouteResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}