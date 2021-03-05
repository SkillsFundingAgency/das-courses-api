using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromRouteImportToRoute
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(RouteImport source)
        {
            //Act
            var actual = (Route) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options=>options.Excluding(c=>c.Standards));
        }
    }
}