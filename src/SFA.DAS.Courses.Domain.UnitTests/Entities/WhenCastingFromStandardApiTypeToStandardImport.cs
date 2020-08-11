using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;

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

        [Test, AutoData]
        public void Then_All_Skills_Are_Mapped_If_The_CoreAndOptions_Is_False(ImportTypes.Standard standard, string detail, Guid skillId, Duty duty)
        {
            //Arrange
            standard.CoreAndOptions = false;
            duty.MappedSkills = new List<Guid>{skillId};
            
            //Act
            var actual = (StandardImport) standard;
            
            //Assert
            actual.CoreSkillsCount.Should().Be(string.Join("|", standard.Skills.Select(c=>c.Detail)));
        }
        
        
        [Test, AutoData]
        public void Then_All_Skills_That_Are_Mapped_To_A_Core_Duty_Are_Shown_If_The_CoreAndOptions_Is_True(ImportTypes.Standard standard, string detail, Guid skillId, Duty duty)
        {
            // Arrange
            standard.CoreAndOptions = true;
            standard.Skills = new List<Skill>
            {
                new Skill
                {
                    Detail = detail,
                    SkillId = skillId.ToString()
                }
            };
            duty.MappedSkills = new List<Guid>{skillId};
            standard.Duties = new List<Duty>
            {
                duty
            };
            
            //Act
            var actual = (StandardImport) standard;
            
            //Assert
            actual.CoreSkillsCount.Should().Be(detail);
        }
    }
}