using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public partial class WhenCastingFromSectorSubjectAreaTier2ImportToSectorSubjectAreaTier2
    {
        public class WhenCastingFromSectorSubjectAreaTier1ImportToSectorSubjectAreaTier1
        {
            [Test, AutoData]
            public void Then_The_Fields_Are_Correctly_Mapped(SectorSubjectAreaTier1Import source)
            {
                SectorSubjectAreaTier1 actual = source;

                actual.Should().BeEquivalentTo(source);
            }
        }
    }
}
