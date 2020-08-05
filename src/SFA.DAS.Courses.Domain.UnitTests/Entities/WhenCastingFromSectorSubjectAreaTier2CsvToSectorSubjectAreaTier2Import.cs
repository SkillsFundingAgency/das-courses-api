using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromSectorSubjectAreaTier2CsvToSectorSubjectAreaTier2Import
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(SectorSubjectAreaTier2Csv source)
        {
            var actual = (SectorSubjectAreaTier2Import) source;
            
            actual.Should().BeEquivalentTo(source);
        }
    }
}