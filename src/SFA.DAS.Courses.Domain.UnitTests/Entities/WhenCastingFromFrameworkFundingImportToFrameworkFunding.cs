using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromFrameworkFundingImportToFrameworkFunding
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(FrameworkFundingImport frameworkFundingImport)
        {
            var actual = (FrameworkFunding) frameworkFundingImport;
            
            actual.Should().BeEquivalentTo(frameworkFundingImport, options=> options
                .Excluding(c=>c.Framework)
                .Excluding(c=>c.Id)
            );
        }
    }
}