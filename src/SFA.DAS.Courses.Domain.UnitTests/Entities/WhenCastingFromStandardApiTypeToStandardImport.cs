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
                 .Excluding(c => c.Knowledge)
                 .Excluding(c => c.Behaviours)
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
        public void Then_Leading_And_Trailing_Whitespace_Is_Removed_From_The_Course_Title(ImportTypes.Standard standard)
        {
            //Arrange
            var expectedTitle = standard.Title;
            standard.Title = $"  {expectedTitle} ";
            
            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Title.Should().Be(expectedTitle);
        }

        [Test, AutoData]
        public void Then_All_Skills_Are_Mapped_If_The_CoreAndOptions_Is_False(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = false;            

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.CoreSkillsCount.Should().Be(string.Join("|", standard.Skills.Select(c => c.Detail)));
        }

        [Test, AutoData]
        public void Then_All_Skills_Are_Mapped_In_Same_Order_As_Skills_List_If_The_CoreAndOptions_Is_False(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = false;            

            //Act
            var actual = (StandardImport)standard;

            //Assert
            Assert.AreEqual(standard.Skills.Select(s => s.Detail), actual.CoreSkillsCount.Split("|").ToList());
        }

        [Test, AutoData]
        public void Then_All_Skills_That_Are_Mapped_To_A_Core_Duty_Are_Shown_If_The_CoreAndOptions_Is_True(ImportTypes.Standard standard, string detail, string skillId, Duty duty)
        {
            // Arrange
            standard.CoreAndOptions = true;
            standard.Skills.Add(new Skill
                {
                    Detail = detail,
                    SkillId = skillId.Substring(7),
                }
            );

            duty.MappedSkills = new List<Guid> { Guid.Parse(skillId.Substring(7)) };
            duty.IsThisACoreDuty = 1;
            standard.Duties.Add(duty);      

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.CoreSkillsCount.Should().Be(detail);
        }

        [Test, AutoData]
        public void Then_All_Skills_That_Are_Mapped_To_A_Core_Duty_Are_Mapped_In_Same_Order_As_Skills_List_If_The_CoreAndOptions_Is_True(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            foreach (var skill in standard.Skills)
            {
                foreach (var duty in standard.Duties)
                {
                    var random = new Random();
                    if (random.Next(2) == 1)
                    {
                        duty.IsThisACoreDuty = 1;
                        duty.MappedSkills.Add(Guid.Parse(skill.SkillId.Substring(7)));
                    }
                }
            }

            //Act
            var actual = (StandardImport)standard;
            var coreSkillsCountList = actual.CoreSkillsCount.IsNullOrEmpty() ? new List<string>() : actual.CoreSkillsCount.Split("|").ToList();
            standard.Skills.RemoveAll(s => !coreSkillsCountList.Contains(s.Detail));

            //Assert           
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

        [Test, AutoData]
        public void Then_If_The_Version_Is_Null_It_Is_Set_As_Zero(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Version = null;            

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Version.Should().Be(0);
        }

        [Test, AutoData]
        public void Then_All_Skills_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Skills.Should().BeEquivalentTo(standard.Skills.Select(c => c.Detail));
        }

        [Test, AutoData]
        public void Then_All_Knowledge_Is_Mapped(ImportTypes.Standard standard)
        {
            //Arrange

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Knowledge.Should().BeEquivalentTo(standard.Knowledge.Select(c => c.Detail));
        }

        [Test, AutoData]
        public void Then_All_Behaviours_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Behaviours.Should().BeEquivalentTo(standard.Behaviours.Select(c => c.Detail));
        }
    }
}
