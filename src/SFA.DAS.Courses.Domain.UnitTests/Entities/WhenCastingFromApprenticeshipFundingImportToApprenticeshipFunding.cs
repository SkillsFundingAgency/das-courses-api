using AutoFixture.NUnit3;
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
            var actual = (ApprenticeshipFunding) apprenticeshipFundingImport;
            
            actual.Should().BeEquivalentTo(apprenticeshipFundingImport, options=> options
                .Excluding(c=>c.Standard)
                .Excluding(c=>c.Id)
                .Excluding(c=>c.LarsCode)
            );
        }
    }
}