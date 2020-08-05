using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Courses
{
    public class WhenCastingToStandardDatesFromEntity
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Mapped_Correctly(LarsStandard larsStandard)
        {
            var actual = (StandardDates) larsStandard;
            
            actual.Should().BeEquivalentTo(larsStandard, options=> options
                .Excluding(c=>c.Id)
                .Excluding(c=>c.StandardId)
                .Excluding(c=>c.Version)
                .Excluding(c=>c.Standard)
                .Excluding(c=>c.SectorSubjectAreaTier2)
            );
        }
    }
}