using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromLarsStandardImportToLarsStandard
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(LarsStandardImport larsStandardImport)
        {
            var actual = (LarsStandard) larsStandardImport;
            
            actual.Should().BeEquivalentTo(larsStandardImport, options => options
                .Excluding(c=>c.LarsCode)
            );
        }
    }
}
