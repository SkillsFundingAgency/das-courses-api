using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Courses
{
    public class WhenCastingToSectorFromEntity
    {
        [Test, RecursiveMoqAutoData]
        public void Then_Maps_Fields_Appropriately(
            Domain.Entities.Sector sector)
        {
            var actual = (Sector)sector;
            
            actual.Should().BeEquivalentTo(sector, options => options.Excluding(c=>c.Standards));
        }
    }
}