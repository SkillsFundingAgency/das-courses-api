using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromFrameworkImportToFramework
    {
        [Test, RecursiveMoqAutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(FrameworkImport frameworkImport)
        {
            var actual = (Framework) frameworkImport;
            
            actual.Should().BeEquivalentTo(frameworkImport, options=> options.Excluding(c=>c.FundingPeriods));
        }
    }
}