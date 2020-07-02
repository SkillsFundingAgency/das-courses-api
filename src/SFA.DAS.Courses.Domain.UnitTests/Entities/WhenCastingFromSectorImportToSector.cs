using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromSectorImportToSector
    {
        [Test, RecursiveMoqAutoData]
        public void Then_Maps_The_Fields(SectorImport sectorImport)
        {
            var actual = (Sector)sectorImport;
            
            actual.Should().BeEquivalentTo(sectorImport, options => options.Excluding(c=>c.Standards));
        }
    }
}