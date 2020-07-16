using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Courses
{
    public class WhenCastingToFrameworkFromEntity
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(Framework framework)
        {
            var actual = (Domain.Courses.Framework) framework;
            
            actual.Should().BeEquivalentTo(framework, options=>
                options
                    .Excluding(c=>c.FundingPeriods)
                    .Excluding(c=>c.TypicalLengthFrom)
                    .Excluding(c=>c.TypicalLengthTo)
                    .Excluding(c=>c.TypicalLengthUnit)
                
                );
            actual.TypicalLength.From.Should().Be(framework.TypicalLengthFrom);
            actual.TypicalLength.To.Should().Be(framework.TypicalLengthTo);
            actual.TypicalLength.Unit.Should().Be(framework.TypicalLengthUnit);

            actual.FundingPeriods.Should()
                .BeEquivalentTo(framework.FundingPeriods, options => options
                    .Excluding(c => c.Framework)
                    .Excluding(c => c.FrameworkId)
                    .Excluding(c => c.Id)
                );
        }
    }
}