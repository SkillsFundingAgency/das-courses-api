using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromStandardApiTypeToStandardImport
    {
        [Test, AutoData]
        public void Then_Maps_The_Fields(ImportTypes.Standard standard)
        {
            var actual = (StandardImport) standard;
            
            actual.Should().BeEquivalentTo(standard,options=>options
                .Excluding(c=>c.Route)
                .Excluding(c=>c.Duties)
                .Excluding(c=>c.Keywords)
                .Excluding(c=>c.Skills)
                .Excluding(c=>c.JobRoles)
                .Excluding(c=>c.LarsCode)
                .Excluding(c=>c.Status)
                .Excluding(c=>c.StandardPageUrl)
                .Excluding(c=>c.TypicalJobTitles)
            );

            actual.Id.Should().Be(standard.LarsCode);
            actual.StandardPageUrl.Should().Be(standard.StandardPageUrl.AbsoluteUri);
            actual.CoreSkillsCount.Should().Be(string.Join("|", standard.Duties.Where(c => c.IsThisACoreDuty.Equals(1))
                .Select(c => c.DutyDetail)));
            actual.TypicalJobTitles.Should().Be(string.Join("|",standard.TypicalJobTitles));

        }
    }
}