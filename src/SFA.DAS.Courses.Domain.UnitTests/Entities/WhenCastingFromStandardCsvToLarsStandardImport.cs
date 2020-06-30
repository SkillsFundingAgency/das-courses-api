using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromStandardCsvToLarsStandardImport
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_Correctly(StandardCsv standardCsv)
        {
            var actual = (LarsStandardImport) standardCsv;
            
            actual.Should().BeEquivalentTo(standardCsv, options=>options.Excluding(c=>c.StandardCode));
            actual.StandardId.Should().Be(standardCsv.StandardCode);
        }
    }
}