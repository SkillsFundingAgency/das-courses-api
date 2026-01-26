using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromFundingImportToApprenticeshipFunding
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(FundingImport fundingImport)
        {
            // Act
            ApprenticeshipFunding actual = fundingImport;

            // Assert
            actual.Should().BeEquivalentTo(fundingImport, options => options
                .Excluding(x => x.Id)
                .Excluding(x => x.LearnAimRef)
                .Excluding(x => x.RateWeighted)
                .Excluding(x => x.RateUnWeighted)
                .Excluding(x => x.WeightingFactor)
                .Excluding(x => x.FundedGuidedLearningHours)
                .Excluding(x => x.FundingCategory)
                .Excluding(x => x.AdultSkillsFundingBand)
            );

            actual.Id.Should().NotBeEmpty();
            actual.LarsCode.Should().Be(fundingImport.LearnAimRef);
            actual.MaxEmployerLevyCap.Should().Be(fundingImport.RateUnWeighted);
            actual.Duration.Should().Be(fundingImport.FundedGuidedLearningHours.GetValueOrDefault(0));
            actual.DurationUnits.Should().Be("Hours");
            actual.FundingStream.Should().Be(fundingImport.FundingCategory);
        }
    }
}
