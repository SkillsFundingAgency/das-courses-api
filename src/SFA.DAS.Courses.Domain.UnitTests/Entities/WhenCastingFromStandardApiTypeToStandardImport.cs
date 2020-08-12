using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using AutoFixture.NUnit3;
using Castle.Core.Internal;
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
            var actual = (StandardImport)standard;

            actual.Should().BeEquivalentTo(standard, options => options
                 .Excluding(c => c.Route)
                 .Excluding(c => c.Duties)
                 .Excluding(c => c.Keywords)
                 .Excluding(c => c.Skills)
                 .Excluding(c => c.JobRoles)
                 .Excluding(c => c.LarsCode)
                 .Excluding(c => c.Status)
                 .Excluding(c => c.StandardPageUrl)
                 .Excluding(c => c.TypicalJobTitles)
                 .Excluding(c => c.CoreAndOptions)
            );

            actual.Id.Should().Be(standard.LarsCode);
            actual.StandardPageUrl.Should().Be(standard.StandardPageUrl.AbsoluteUri);            
            actual.TypicalJobTitles.Should().Be(string.Join("|", standard.TypicalJobTitles));

        }

        [Test, AutoData]
        public void Then_All_Skills_Are_Mapped_If_The_CoreAndOptions_Is_False(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = false;            

            //Act
            var actual = (StandardImport)standard;

            //Assert - CoreSkillsCount should contain details for all skills in standard.Skills
            actual.CoreSkillsCount.Should().Be(string.Join("|", standard.Skills.Select(c => c.Detail)));
        }

        [Test, AutoData]
        public void Then_All_Skills_Are_Mapped_In_Correct_Order_If_The_CoreAndOptions_Is_False(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = false;            

            //Act
            var actual = (StandardImport)standard;

            //Assert - skill details should be in the same order as skills listed in standard.Skills
            Assert.AreEqual(standard.Skills.Select(s => s.Detail), actual.CoreSkillsCount.Split("|").ToList());
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
                    SkillId = skillId,
                }
            };
            duty.MappedSkills = new List<Guid> { skillId };
            duty.IsThisACoreDuty = 1;
            standard.Duties = new List<Duty>
            {
                duty
            };

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.CoreSkillsCount.Should().Be(detail);
        }

        [Test, AutoData]
        public void Then_All_Skills_Are_Mapped_In_Correct_Order_If_The_CoreAndOptions_Is_True(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;

            //Add skills randomly from the standard.Skills list to each duty.MappedSkills
            foreach (var skill in standard.Skills)
            {
                foreach (var duty in standard.Duties)
                {
                    var random = new Random();
                    if (random.Next(2) == 1)
                    {
                        duty.MappedSkills.Add(skill.SkillId);
                    }
                }
            }

            //Act
            var actual = (StandardImport)standard;
            var coreSkillsCountList = actual.CoreSkillsCount.IsNullOrEmpty() ? new List<string>() : actual.CoreSkillsCount.Split("|").ToList();

            //Assert - CoreSkillsCount should contain details for each skill mapped to a core duty's MappedSkills, in the order that they appear in the standard.Skills list
            //Remove skills not in core list from standard.Skills to compare list order
            standard.Skills.RemoveAll(s => !coreSkillsCountList.Contains(s.Detail));
            Assert.AreEqual(standard.Skills.Select(s => s.Detail), coreSkillsCountList);
        }

        [Test, AutoData]
        public void Then_No_Skills_Are_Mapped_If_No_Skill_Data_Available(ImportTypes.Standard standard)
        {
            // Arrange
            standard.Skills = new List<Skill> { };
            
            // Act
            var actual = (StandardImport)standard;

            // Assert
            actual.CoreSkillsCount.Should().Be(null);
        }
    }
}
