using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromApprenticeshipFundingImportToApprenticeshipFunding
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ApprenticeshipFundingImport apprenticeshipFundingImport)
        {
            // Act
            ApprenticeshipFunding actual = apprenticeshipFundingImport;

            // Assert
            actual.Should().BeEquivalentTo(apprenticeshipFundingImport, options => options
                .Excluding(x => x.Id)
                .Excluding(x => x.LarsCode)
            );

            actual.Id.Should().NotBeEmpty();
            actual.LarsCode.Should().Be(apprenticeshipFundingImport.LarsCode.ToString());
            actual.DurationUnits.Should().Be("Months");
            actual.FundingStream.Should().Be("Apprenticeship");
        }
    }
}
