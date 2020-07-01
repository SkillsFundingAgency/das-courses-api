using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromApprenticeshipFundingImportToApprenticeshipFunding
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(ApprenticeshipFundingImport apprenticeshipFundingImport)
        {
            var actual = (ApprenticeshipFunding) apprenticeshipFundingImport;
            
            actual.Should().BeEquivalentTo(apprenticeshipFundingImport);
        }
    }
}