using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.UnitTests.Data;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromStandardApiTypeToStandardImport
    {
        [Test, AutoData]
        public void Then_Maps_The_Fields(ImportTypes.Standard standard)
        {
            standard.Version = "1.0";
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
                .Excluding(c => c.EqaProvider)
                .Excluding(c => c.TbMainContact)
                .Excluding(c => c.Change)
            );

            actual.LarsCode.Should().Be(standard.LarsCode);
            actual.StandardPageUrl.Should().Be(standard.StandardPageUrl.AbsoluteUri);
            actual.TypicalJobTitles.Should().Be(string.Join("|", standard.TypicalJobTitles));
            actual.EqaProviderWebLink.Should().Be(standard.EqaProvider.WebLink);
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
            actual.CoreDuties.Should().BeEquivalentTo(new List<string>{detail});
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
        public void Then_If_The_Version_Is_Null_It_Is_Set_To_DefaultVersion(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Version = null;            

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Version.Should().Be("1.0");
        }

        [Test, AutoData]
        public void Then_Major_and_Minor_versions_are_mapped(ImportTypes.Standard standard)
        {
            standard.Version = "1.2";

            var actual = (StandardImport)standard;

            actual.VersionMajor.Should().Be(1);
            actual.VersionMinor.Should().Be(2);
        }

        [Test, AutoData]
        public void Then_All_Knowledge_Is_Mapped_To_Correct_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Knowledge = KnowledgeBuilder.Create("k1", "k2", "k3", "k4");
            var options = new[]
            {
                new OptionBuilder().WithKnowledge(standard.Knowledge.Take(2)),
                new OptionBuilder().WithKnowledge(standard.Knowledge.Skip(2)),
            };
            standard.Options = options.Select(x => x.Build()).ToList();
            standard.Duties = new List<Duty>
            {
                new DutyBuilder().ForOptions(options[0]).Build(),
                new DutyBuilder().ForOptions(options[1]).Build(),
            };

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options2().Should().Contain(x => x.OptionId == options[0].OptionId)
                .Which.Knowledge.Should().BeEquivalentTo("k1", "k2");
            actual.Options2().Should().Contain(x => x.OptionId == options[1].OptionId)
                .Which.Knowledge.Should().BeEquivalentTo("k3", "k4");
        }

        [Test, AutoData]
        public void Then_All_Skills_Are_Mapped_To_Correct_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Skills = SkillsBuilder.Create("s1", "s2", "s3", "s4", "s5");
            var options = new[]
            {
                new OptionBuilder().WithSkills(standard.Skills.Take(2)),
                new OptionBuilder().WithSkills(standard.Skills.Skip(2)),
            };
            standard.Options = options.Select(x => x.Build()).ToList();
            standard.Duties = new List<Duty>
            {
                new DutyBuilder().ForOptions(options[0]).Build(),
                new DutyBuilder().ForOptions(options[1]).Build(),
            };

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options2().Should().Contain(x => x.OptionId == options[0].OptionId)
                .Which.Skills.Should().BeEquivalentTo("s1", "s2");
            actual.Options2().Should().Contain(x => x.OptionId == options[1].OptionId)
                .Which.Skills.Should().BeEquivalentTo("s3", "s4", "s5");
        }

        [Test, AutoData]
        public void Then_All_Behaviours_Are_Mapped_To_Correct_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Behaviours = BehavioursBuilder.Create("b1", "b2", "b3", "b4", "b5", "b6");
            var options = new[]
            {
                new OptionBuilder().WithBehaviours(standard.Behaviours.Take(1)),
                new OptionBuilder().WithBehaviours(standard.Behaviours.Skip(2).Take(3)),
                new OptionBuilder().WithBehaviours(standard.Behaviours.Skip(3)),
            };
            standard.Options = options.Select(x => x.Build()).ToList();
            standard.Duties = new List<Duty>
            {
                new DutyBuilder().ForOptions(options[0], options[1]).Build(),
                new DutyBuilder().ForOptions(options[2]).Build(),
            };

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options2().Should().Contain(x => x.OptionId == options[0].OptionId)
                .Which.Behaviours.Should().BeEquivalentTo("b1", "b3", "b4", "b5");
            actual.Options2().Should().Contain(x => x.OptionId == options[1].OptionId)
                .Which.Behaviours.Should().BeEquivalentTo("b1", "b3", "b4", "b5");
            actual.Options2().Should().Contain(x => x.OptionId == options[2].OptionId)
                .Which.Behaviours.Should().BeEquivalentTo("b4", "b5", "b6");
        }

        [Test, AutoData]
        public void Then_Null_MappedOptions_Are_OK(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Duties = new List<Duty>
            {
                new Duty
                {
                    DutyId = Guid.NewGuid(),
                    MappedOptions = null,
                }
            };

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options2().Should().Contain(x => x.OptionId == standard.Options.First().OptionId)
                .Which.Behaviours.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_Null_KSBs_For_Options_Are_OK(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Duties = new List<Duty>
            {
                new Duty
                {
                    DutyId = Guid.NewGuid(),
                    MappedOptions = standard.Options.Select(x => x.OptionId).ToList(),
                }
            };

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options2().Should().Contain(x => x.OptionId == standard.Options.First().OptionId)
                .Which.Behaviours.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_All_Skills_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = false;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Skills.Should().BeEquivalentTo(standard.Skills.Select(x => x.Detail).ToList());
        }

        [Test, AutoData]
        public void Then_All_Knowledge_Is_Mapped(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = false;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Knowledge.Should().BeEquivalentTo(standard.Knowledge.Select(c => c.Detail));
        }

        [Test, AutoData]
        public void Then_All_Behaviours_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = false;

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

        [Test]
        [InlineAutoData("Option")]
        [InlineAutoData(" Option")]
        [InlineAutoData("  Option")]
        [InlineAutoData("Option ")]
        public void And_Option_Contains_Whitespace_Then_Option_Is_Trimmed_Correctly(string optionTitle,ImportTypes.Standard standard)
        {
            //Arrange
            standard.Options[0].Title = optionTitle;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options[0].Should().Be("Option");
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

        [Test, AutoData]
        public void Then_If_Change_Is_Empty_Then_EPAChanged_Is_False(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Change = string.Empty;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.EPAChanged.Should().Be(false);
        }

        [Test, AutoData]
        public void Then_If_Change_Does_Not_Contain_Magic_Value_Then_EPAChanged_Is_False(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Change = "Approved for delivery";

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.EPAChanged.Should().Be(false);
        }

        [Test, AutoData]
        public void Then_If_Change_Equals_Magic_Value_Then_EPAChanged_Is_True(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Change = "End-point assessment plan revised";

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.EPAChanged.Should().Be(true);
        }

        [Test, AutoData]
        public void Then_If_Change_Contains_Magic_Value_Then_EPAChanged_Is_True(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Change = "Approved for delivery. End-point assessment plan revised";

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.EPAChanged.Should().Be(true);
        }

        [Test]
        [InlineAutoData("ST0001")]
        [InlineAutoData(" ST0001")]
        [InlineAutoData("  ST0001")]
        [InlineAutoData("ST0001 ")]
        public void Then_ifate_reference_is_trimmed_and_mapped(string ifateReference, ImportTypes.Standard standard)
        {
            standard.ReferenceNumber = ifateReference;

            var actual = (StandardImport)standard;

            actual.IfateReferenceNumber.Should().Be("ST0001");
        }

        [Test]
        [InlineAutoData("Live", "Live")]
        [InlineAutoData(" Approved for delivery", "Approved for delivery")]
        [InlineAutoData("  Retired ", "Retired")]
        [InlineAutoData("Withdrawn ", "Withdrawn")]
        public void Then_status_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.Status = source;

            var actual = (StandardImport)standard;

            actual.Status.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("trailblazer@contact.com", "trailblazer@contact.com")]
        [InlineAutoData(" trailblazer@contact.com", "trailblazer@contact.com")]
        [InlineAutoData("  trailblazer@contact.com ", "trailblazer@contact.com")]
        [InlineAutoData(null, null)]
        public void Then_trail_blazer_contact_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.TbMainContact = source;

            var actual = (StandardImport)standard;

            actual.TrailBlazerContact.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("Provider name", "Provider name")]
        [InlineAutoData(" Provider name", "Provider name")]
        [InlineAutoData("  Provider name ", "Provider name")]
        [InlineAutoData(null, null)]
        public void Then_provider_name_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.EqaProvider.ProviderName = source;

            var actual = (StandardImport)standard;

            actual.EqaProviderName.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("Contact name", "Contact name")]
        [InlineAutoData(" Contact name", "Contact name")]
        [InlineAutoData("  Contact name ", "Contact name")]
        [InlineAutoData(null, null)]
        public void Then_provider_contact_name_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.EqaProvider.ContactName = source;

            var actual = (StandardImport)standard;

            actual.EqaProviderContactName.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("contact@email.com", "contact@email.com")]
        [InlineAutoData(" contact@email.com", "contact@email.com")]
        [InlineAutoData("  contact@email.com ", "contact@email.com")]
        [InlineAutoData(null, null)]
        public void Then_provider_contact_email_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.EqaProvider.ContactEmail = source;

            var actual = (StandardImport)standard;

            actual.EqaProviderContactEmail.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("Regulated body", "Regulated body")]
        [InlineAutoData(" Regulated body", "Regulated body")]
        [InlineAutoData("  Regulated body ", "Regulated body")]
        [InlineAutoData(null, null)]
        public void Then_regulated_body_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.RegulatedBody = source;

            var actual = (StandardImport)standard;

            actual.RegulatedBody.Should().Be(expected);
        }
    }
}
