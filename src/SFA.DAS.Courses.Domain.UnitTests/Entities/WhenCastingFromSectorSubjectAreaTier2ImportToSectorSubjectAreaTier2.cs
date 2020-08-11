using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromSectorSubjectAreaTier2ImportToSectorSubjectAreaTier2
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(SectorSubjectAreaTier2Import source)
        {
            var actual = (SectorSubjectAreaTier2) source;
            
            actual.Should().BeEquivalentTo(source);
        }
    }
}