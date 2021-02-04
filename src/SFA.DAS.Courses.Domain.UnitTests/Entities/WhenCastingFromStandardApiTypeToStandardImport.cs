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
            var actual = (StandardImport)standard;

            actual.Should().BeEquivalentTo(standard, options => options
                .Excluding(c => c.Options)
                .Excluding(c => c.Route)
                .Excluding(c => c.Duties)
                .Excluding(c => c.Keywords)
                .Excluding(c => c.Skills)
                .Excluding(c => c.Knowledge)
                .Excluding(c => c.Behaviours)
                .Excluding(c => c.JobRoles)
                .Excluding(c => c.StandardPageUrl)
                .Excluding(c => c.TypicalJobTitles)
                .Excluding(c => c.CoreAndOptions)
                .Excluding(c => c.ReferenceNumber)
            );

            actual.LarsCode.Should().Be(standard.LarsCode);
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
        public void Then_All_Skills_That_Are_Mapped_To_A_Core_Duty_Are_Shown(ImportTypes.Standard standard, string detail, string skillId, Duty duty)
        {
            // Arrange	
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
            actual.CoreDuties.Should().Be(detail);
        }

        [Test, AutoData]
        public void Then_All_Skills_That_Are_Mapped_To_A_Core_Duty_Are_Mapped_In_Same_Order_As_Skills_List(ImportTypes.Standard standard)
        {
            //Arrange	
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
            standard.Skills.RemoveAll(s => !actual.CoreDuties.Contains(s.Detail));

            //Assert           	
            Assert.AreEqual(standard.Skills.Select(s => s.Detail), actual.CoreDuties);
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
            actual.Skills.Should().BeEquivalentTo(standard.Skills.Select(x => x.Detail).ToList());
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

        [Test, AutoData]
        public void Then_All_Duties_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Duties.Should().BeEquivalentTo(standard.Duties.Select(c => c.DutyDetail));
        }

        [Test, AutoData]
        public void Then_All_Options_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().BeEquivalentTo(standard.Options.Select(c => c.Title));
        }

        [Test, AutoData]
        public void Then_Options_Are_Mapped_To_Empty_List(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Options = null;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().BeEquivalentTo(new List<string>());
        }

        [Test, AutoData]
        public void Then_All_OptionsUnstructuredTemplate_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.OptionsUnstructuredTemplate.Should().BeEquivalentTo(standard.OptionsUnstructuredTemplate);
        }

        [Test, AutoData]
        public void Then_OptionsUnstructuredTemplate_Are_Mapped_To_Empty_List(ImportTypes.Standard standard)
        {
            //Arrange
            standard.OptionsUnstructuredTemplate = null;
            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.OptionsUnstructuredTemplate.Should().BeEquivalentTo(new List<string>());
        }

        [Test, AutoData]
        public void Then_Mappings_Cope_With_Null_Sources(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Knowledge = null;
            standard.Behaviours = null;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Knowledge.Should().BeEmpty();
            actual.Behaviours.Should().BeEmpty();
        }

        [Test]
        [InlineAutoData(3, "integrated degree", false)]
        [InlineAutoData(5, "integrated degree", false)]
        [InlineAutoData(6, "integrated degree", true)]
        [InlineAutoData(6, "INTEGRATED degree", true)]
        [InlineAutoData(6, "non integrated", false)]
        [InlineAutoData(6, "", false)]
        [InlineAutoData(6, "abc", false)]
        [InlineAutoData(7, "Integrated Degree", true)]
        public void Then_If_The_Standard_Is_Level_Six_Or_Above_The_Integrated_Degree_Field_Is_Used_To_Set_The_Standard_As_IntegratedApprenticeship(
            int level, string integratedDegreeValue, bool expected, ImportTypes.Standard standard)
        {
            //Arrange
            standard.Level = level;
            standard.IntegratedDegree = integratedDegreeValue;
            
            //Act
            var actual = (StandardImport) standard;

            //Assert
            actual.IntegratedApprenticeship.Should().Be(expected);
        }

        [Test]
        [InlineAutoData(3, true, true)]
        [InlineAutoData(4, false, false)]
        [InlineAutoData(5, true, true)]
        [InlineAutoData(6, true, false)]
        [InlineAutoData(6, false, false)]
        [InlineAutoData(6, null, false)]
        public void Then_If_The_Standard_Is_Level_Five_Or_Below_Then_The_IntegratedApprenticeship_Field_Is_Used_To_Set_The_Standard_As_IntegratedApprenticeship(
            int level, bool? integratedValue, bool expected, ImportTypes.Standard standard)
        {
            //Arrange
            standard.Level = level;
            standard.IntegratedApprenticeship = integratedValue;
            
            //Act
            var actual = (StandardImport) standard;

            //Assert
            actual.IntegratedApprenticeship.Should().Be(expected);
        }
        
    }
}
