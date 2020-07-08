using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromFrameworkFundingApiTypeToFrameworkFundingImport
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(FundingPeriod fundingPeriod, string frameworkId)
        {
            
            var actual = new FrameworkFundingImport().Map(fundingPeriod,frameworkId);
            
            actual.Should().BeEquivalentTo(fundingPeriod);
            actual.FrameworkId.Should().Be(frameworkId);
        }
    }
}