using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using Framework = SFA.DAS.Courses.Domain.ImportTypes.Framework;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromFrameworkApiTypeToFrameworkImport
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(Framework framework)
        {
            var actual = (FrameworkImport) framework;
            
            actual.Should().BeEquivalentTo(framework, options=>options
                .Excluding(x=>x.FundingPeriods)
                .Excluding(x=>x.TypicalLength)
            );

            actual.TypicalLengthFrom.Should().Be(framework.TypicalLength.From);
            actual.TypicalLengthTo.Should().Be(framework.TypicalLength.To);
            actual.TypicalLengthUnit.Should().Be(framework.TypicalLength.Unit);
        }
    }
}