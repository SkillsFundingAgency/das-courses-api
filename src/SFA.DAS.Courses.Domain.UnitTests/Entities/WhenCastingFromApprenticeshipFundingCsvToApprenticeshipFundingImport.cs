using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromApprenticeshipFundingCsvToApprenticeshipFundingImport
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_Correctly(ApprenticeshipFundingCsv apprenticeshipFundingCsv)
        {
            var actual = (ApprenticeshipFundingImport) apprenticeshipFundingCsv;
            
            actual.Should().BeEquivalentTo(apprenticeshipFundingCsv, options => options.Excluding(c=>c.ApprenticeshipCode));
            actual.StandardId.Should().Be(apprenticeshipFundingCsv.ApprenticeshipCode);
        }
    }
}