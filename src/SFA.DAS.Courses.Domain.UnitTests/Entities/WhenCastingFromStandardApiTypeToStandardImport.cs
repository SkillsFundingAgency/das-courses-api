using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Configuration;
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
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;
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
                .Excluding(c => c.IntegratedApprenticeship)
                .Excluding(c => c.RegulationDetail)
                .Excluding(c => c.Regulated)
                .Excluding(c => c.ApprenticeshipType)
            );

            actual.LarsCode.Should().Be(standard.LarsCode);
            actual.ApprenticeshipType.Should().Be("Apprenticeship");
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
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Title.Should().Be(expectedTitle);
        }

        [Test, AutoData]
        public void Then_All_Skills_That_Are_Mapped_To_A_Core_Duty_Are_Shown(ImportTypes.Standard standard, string detail, Guid skillId, Duty duty)
        {
            // Arrange	
            standard.Skills.Add(new Skill
            {
                Detail = detail,
                SkillId = skillId,
            }
            );

            duty.MappedSkills = new List<Guid> { skillId };
            duty.IsThisACoreDuty = 1;
            standard.Duties.Add(duty);
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act	
            var actual = (StandardImport)standard;

            //Assert	
            actual.CoreDuties.Should().BeEquivalentTo(new List<string> { detail });
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
                        duty.MappedSkills.Add(skill.SkillId);
                    }
                }
            }

            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act	
            var actual = (StandardImport)standard;
            standard.Skills.RemoveAll(s => !actual.CoreDuties.Contains(s.Detail));

            //Assert           	
            standard.Skills.Select(s => s.Detail).Should().BeEquivalentTo(actual.CoreDuties);
        }

        [Test, AutoData]
        public void Then_If_The_Version_Is_Null_It_Is_Set_To_DefaultVersion(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Version = null;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Version.Should().Be("1.0");
        }

        [Test, AutoData]
        public void Then_Major_and_Minor_versions_are_mapped(ImportTypes.Standard standard)
        {
            standard.Version = "1.2";
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            var actual = (StandardImport)standard;

            actual.VersionMajor.Should().Be(1);
            actual.VersionMinor.Should().Be(2);
        }

        [Test, AutoData]
        public void Then_All_Knowledge_Is_Mapped_To_Correct_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Knowledge = KnowledgeBuilder.Create("k1", "k2", "k3", "k4");
            var options = new[]
            {
                new OptionBuilder().WithKnowledge(standard.Knowledge.Take(2)),
                new OptionBuilder().WithKnowledge(standard.Knowledge.Skip(2)),
            };
            standard.Options = options.Select(x => x.Build()).ToList();
            standard.Duties = new List<Duty>
            {
                new OptionDutyBuilder().ForOptions(options[0]).Build(),
                new OptionDutyBuilder().ForOptions(options[1]).Build(),
            };
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == options[0].OptionId)
                .Which.Knowledge.Should().BeEquivalentTo(new[]{
                    new { Id = standard.Knowledge[0].KnowledgeId, Detail = "k1" },
                    new { Id = standard.Knowledge[1].KnowledgeId, Detail = "k2" }});
            actual.Options.Should().Contain(x => x.OptionId == options[1].OptionId)
                .Which.Knowledge.Should().BeEquivalentTo(new[] {
                    new { Id = standard.Knowledge[2].KnowledgeId, Detail = "k3" },
                    new { Id = standard.Knowledge[3].KnowledgeId, Detail = "k4" }});
        }

        [Test, AutoData]
        public void Then_Core_KSBs_Are_Mapped_To_All_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Knowledge = KnowledgeBuilder.Create("k1-detail", "k2-detail", "k3-detail");
            standard.Skills = SkillsBuilder.Create("s1-detail", "s2-detail");
            standard.Behaviours = BehavioursBuilder.Create("b1-detail");
            var option = new OptionBuilder()
                .WithKnowledge(standard.Knowledge.Take(1))
                .WithSkills(standard.Skills.Take(1));
            standard.Options = new List<Option> { option.Build() };
            standard.Duties = new List<Duty>
            {
                new OptionDutyBuilder().ForOptions(option).Build(),
                new CoreDutyBuilder()
                    .WithKnowledge(standard.Knowledge.Skip(1))
                    .WithSkills(standard.Skills.Skip(1))
                    .WithBehaviour(standard.Behaviours)
                    .Build(),
            };
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            var mappedOption = actual.Options.Should().Contain(x => x.OptionId == option.OptionId).Which;
            mappedOption.Ksbs.Should().BeEquivalentTo(new[]
            {
                new { Type = KsbType.Knowledge, Key = "K1", Detail = "k1-detail" },
                new { Type = KsbType.Knowledge, Key = "K2", Detail = "k2-detail" },
                new { Type = KsbType.Knowledge, Key = "K3", Detail = "k3-detail" },
                new { Type = KsbType.Skill, Key = "S1", Detail = "s1-detail" },
                new { Type = KsbType.Skill, Key = "S2", Detail = "s2-detail" },
                new { Type = KsbType.Behaviour, Key = "B1", Detail = "b1-detail" },
            });
        }

        [Test, AutoData]
        public void Then_Core_KSBs_Are_Mapped_To_Standard_Without_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Knowledge = KnowledgeBuilder.Create("k1-detail", "k2-detail", "k3-detail");
            standard.Skills = SkillsBuilder.Create("s1-detail", "s2-detail");
            standard.Behaviours = BehavioursBuilder.Create("b1-detail");
            standard.CoreAndOptions = false;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            var mappedOption = actual.Options.Should().Contain(x => x.Title == "core").Which;
            mappedOption.Ksbs.Should().BeEquivalentTo(new[]
            {
                new { Type = KsbType.Knowledge, Key = "K1", Detail = "k1-detail" },
                new { Type = KsbType.Knowledge, Key = "K2", Detail = "k2-detail" },
                new { Type = KsbType.Knowledge, Key = "K3", Detail = "k3-detail" },
                new { Type = KsbType.Skill, Key = "S1", Detail = "s1-detail" },
                new { Type = KsbType.Skill, Key = "S2", Detail = "s2-detail" },
                new { Type = KsbType.Behaviour, Key = "B1", Detail = "b1-detail" },
            });
        }

        [Test, AutoData]
        public void Then_All_Skills_Are_Mapped_To_Correct_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Skills = SkillsBuilder.Create("s1", "s2", "s3", "s4", "s5");
            var options = new[]
            {
                new OptionBuilder().WithSkills(standard.Skills.Take(2)),
                new OptionBuilder().WithSkills(standard.Skills.Skip(2)),
            };
            standard.Options = options.Select(x => x.Build()).ToList();
            standard.Duties = new List<Duty>
            {
                new OptionDutyBuilder().ForOptions(options[0]).Build(),
                new OptionDutyBuilder().ForOptions(options[1]).Build(),
            };
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == options[0].OptionId)
                .Which.Skills.Should().BeEquivalentTo(new[]{
                    new { Id = standard.Skills[0].SkillId, Detail = "s1" },
                    new { Id = standard.Skills[1].SkillId, Detail = "s2" }});
            actual.Options.Should().Contain(x => x.OptionId == options[1].OptionId)
                .Which.Skills.Should().BeEquivalentTo(new[]{
                    new { Id = standard.Skills[2].SkillId, Detail = "s3" },
                    new { Id = standard.Skills[3].SkillId, Detail = "s4" },
                    new { Id = standard.Skills[4].SkillId, Detail = "s5" }});
        }

        [Test, AutoData]
        public void Then_All_Behaviours_Are_Mapped_To_Correct_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
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
                new OptionDutyBuilder().ForOptions(options[0], options[1]).Build(),
                new OptionDutyBuilder().ForOptions(options[2]).Build(),
            };
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == options[0].OptionId)
                .Which.Behaviours.Should().BeEquivalentTo(new[]{
                    new { Id = standard.Behaviours[0].BehaviourId, Detail = "b1" },
                    new { Id = standard.Behaviours[2].BehaviourId, Detail = "b3" },
                    new { Id = standard.Behaviours[3].BehaviourId, Detail = "b4" },
                    new { Id = standard.Behaviours[4].BehaviourId, Detail = "b5" }});
            actual.Options.Should().Contain(x => x.OptionId == options[1].OptionId)
                .Which.Behaviours.Should().BeEquivalentTo(new[]{
                    new { Id = standard.Behaviours[0].BehaviourId, Detail = "b1" },
                    new { Id = standard.Behaviours[2].BehaviourId, Detail = "b3" },
                    new { Id = standard.Behaviours[3].BehaviourId, Detail = "b4" },
                    new { Id = standard.Behaviours[4].BehaviourId, Detail = "b5" }});
            actual.Options.Should().Contain(x => x.OptionId == options[2].OptionId)
                .Which.Behaviours.Should().BeEquivalentTo(new[]{
                    new { Id = standard.Behaviours[3].BehaviourId, Detail = "b4" },
                    new { Id = standard.Behaviours[4].BehaviourId, Detail = "b5" },
                    new { Id = standard.Behaviours[5].BehaviourId, Detail = "b6" }});
        }

        [Test, AutoData]
        public void Then_KSBs_Are_Unique(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Knowledge = KnowledgeBuilder.Create("k1", "k2");
            standard.Skills = SkillsBuilder.Create("s1", "s2");
            standard.Behaviours = BehavioursBuilder.Create("b1");
            var options = new[]
            {
                new OptionBuilder().WithKnowledge(standard.Knowledge),
                new OptionBuilder().WithKnowledge(standard.Knowledge),
            };
            standard.Options = options.Select(x => x.Build()).ToList();
            standard.Duties = new List<Duty>
            {
                new OptionDutyBuilder().ForOptions(options).Build(),
                new CoreDutyBuilder().WithKnowledge(standard.Knowledge).Build(),
            };
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == options[0].OptionId)
                .Which.Knowledge.Should().OnlyHaveUniqueItems();
        }

        [Test, AutoData]
        public void Then_Null_MappedOptions_Are_OK(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Duties = new List<Duty>
            {
                new Duty
                {
                    MappedOptions = null,
                }
            };
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == standard.Options.First().OptionId)
                .Which.Behaviours.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_Null_KSBs_For_Options_Are_OK(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Duties = new List<Duty>
            {
                new Duty
                {
                    MappedOptions = standard.Options.Select(x => x.OptionId).ToList(),
                    MappedKnowledge = null,
                    MappedSkills = null,
                    MappedBehaviour = null,
                }
            };
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == standard.Options.First().OptionId)
                .Which.Behaviours.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_All_Duties_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Duties.Should().BeEquivalentTo(standard.Duties.Select(c => c.DutyDetail));
        }

        [Test, AutoData]
        public void Then_All_Options_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Select(c => c.Title).Should().BeEquivalentTo(standard.Options.Select(c => c.Title));
        }

        [Test]
        [InlineAutoData("Option")]
        [InlineAutoData(" Option")]
        [InlineAutoData("  Option")]
        [InlineAutoData("Option ")]
        public void And_Option_Contains_Whitespace_Then_Option_Is_Trimmed_Correctly(string optionTitle, ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Options[0].Title = optionTitle;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options[0].Should().NotBeNull()
                .And.BeAssignableTo<StandardOption>()
                .Which.Title.Should().Be("Option");
        }

        [Test, AutoData]
        public void Then_Options_Are_Mapped_To_Empty_List(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Options = null;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_All_OptionsUnstructuredTemplate_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

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
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.OptionsUnstructuredTemplate.Should().BeEquivalentTo(new List<string>());
        }

        [Test]
        [InlineAutoData(3, "integrated degree", false, false)]
        [InlineAutoData(3, "integrated degree", true, true)]
        [InlineAutoData(5, "integrated degree", false, false)]
        [InlineAutoData(5, "integrated degree", true, true)]
        [InlineAutoData(6, "integrated degree", false, true)]
        [InlineAutoData(6, "INTEGRATED degree", false, true)]
        [InlineAutoData(6, "non integrated", false, false)]
        [InlineAutoData(6, "", false, false)]
        [InlineAutoData(6, "abc", false, false)]
        [InlineAutoData(7, "Integrated Degree", false, true)]
        public void Then_If_The_Standard_Is_Level_Six_Or_Above_The_Integrated_Degree_Field_Is_Used_To_Set_The_Standard_As_IntegratedApprenticeship(
            int level, string integratedDegreeValue, bool integratedApprenticeship, bool expected, ImportTypes.Standard standard)
        {
            //Arrange
            standard.Level = level;
            standard.IntegratedDegree = integratedDegreeValue;
            standard.IntegratedApprenticeship = integratedApprenticeship;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

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
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.IntegratedApprenticeship.Should().Be(expected);
        }

        [Test, AutoData]
        public void Then_If_Change_Is_Empty_Then_EPAChanged_Is_False(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Change = string.Empty;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

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
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

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
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

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
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

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
            //Arrange
            standard.ReferenceNumber = ifateReference;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.IfateReferenceNumber.Should().Be("ST0001");
        }

        [Test]
        [InlineAutoData("Live", "Live")]
        [InlineAutoData(" Approved for delivery", "Approved for delivery")]
        [InlineAutoData("  Retired ", "Retired")]
        [InlineAutoData("Withdrawn ", "Withdrawn")]
        public void Then_status_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            //Arrange
            standard.Status = source;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Status.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("trailblazer@contact.com", "trailblazer@contact.com")]
        [InlineAutoData(" trailblazer@contact.com", "trailblazer@contact.com")]
        [InlineAutoData("  trailblazer@contact.com ", "trailblazer@contact.com")]
        [InlineAutoData(null, null)]
        public void Then_trail_blazer_contact_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            //Arrange
            standard.TbMainContact = source;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.TrailBlazerContact.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("Provider name", "Provider name")]
        [InlineAutoData(" Provider name", "Provider name")]
        [InlineAutoData("  Provider name ", "Provider name")]
        [InlineAutoData(null, null)]
        public void Then_provider_name_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            //Arrange
            standard.EqaProvider.ProviderName = source;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.EqaProviderName.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("Contact name", "Contact name")]
        [InlineAutoData(" Contact name", "Contact name")]
        [InlineAutoData("  Contact name ", "Contact name")]
        [InlineAutoData(null, null)]
        public void Then_provider_contact_name_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            //Arrange
            standard.EqaProvider.ContactName = source;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.EqaProviderContactName.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("contact@email.com", "contact@email.com")]
        [InlineAutoData(" contact@email.com", "contact@email.com")]
        [InlineAutoData("  contact@email.com ", "contact@email.com")]
        [InlineAutoData(null, null)]
        public void Then_provider_contact_email_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            //Arrange
            standard.EqaProvider.ContactEmail = source;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.EqaProviderContactEmail.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("Regulated body", "Regulated body")]
        [InlineAutoData(" Regulated body", "Regulated body")]
        [InlineAutoData("  Regulated body ", "Regulated body")]
        [InlineAutoData(null, null)]
        public void Then_regulated_body_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            //Arrange
            standard.RegulatedBody = source;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.RegulatedBody.Should().Be(expected);
        }

        [Test, AutoData]
        public void Then_fake_duties_are_omitted(ImportTypes.Standard standard)
        {
            // Arrange
            standard.Duties = new List<Duty>
            {
                new Duty
                {
                    DutyDetail = "."
                }
            };
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;

            //Act	
            var actual = (StandardImport)standard;

            // Assert
            actual.Duties.Should().BeEmpty();
        }

        [Test]
        [InlineAutoData(false, "regulator", true, false)]
        [InlineAutoData(true, "", true, false)]
        [InlineAutoData(true, "regulator", false, false)]
        [InlineAutoData(true, "regulator", true, true)]
        public void Then_Maps_IsRegulatedForProvider_And_IsRegulatedForEPAO_Correctly(bool regulated, string regulator,
            bool approved, bool expected, ImportTypes.Standard standard)
        {
            //Arrange
            standard.Regulated = regulated;
            standard.RegulatedBody = regulator;
            standard.RegulationDetail[0].Name = Constants.ProviderRegulationType;
            standard.RegulationDetail[1].Name = Constants.EPAORegulationType;
            standard.RegulationDetail[0].Approved = approved;
            standard.RegulationDetail[1].Approved = approved;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.IsRegulatedForProvider.Should().Be(expected);
            actual.IsRegulatedForEPAO.Should().Be(expected);
        }
    }
}
