using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Courses
{
    public class WhenCastingToRouteFromEntity
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Mapped(Domain.Entities.Route source)
        {
            //Act
            var actual = (Route) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options=> options.Excluding(c=>c.Standards));
        }
    }
}