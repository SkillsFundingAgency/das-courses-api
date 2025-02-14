using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Extensions;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.TestHelper.AutoFixture;
using SFA.DAS.Courses.Domain.UnitTests.Data;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities
{
    public class WhenCastingFromStandardApiTypeToStandardImport
    {
        [Test, StandardAutoData]
        public void Then_Maps_The_Fields(ImportTypes.Standard standard)
        {
            standard.Version = "1.0";
            var actual = (StandardImport)standard;

            actual.LarsCode.Should().Be(standard.LarsCode.Value);
            actual.Status.Should().Be(standard.Status);
            actual.VersionEarliestStartDate.Should().Be(standard.VersionEarliestStartDate.Value);
            actual.VersionLatestStartDate.Should().Be(standard.VersionLatestStartDate.Value);
            actual.VersionLatestEndDate.Should().Be(standard.VersionLatestEndDate.Value);
            actual.IntegratedDegree.Should().Be(standard.IntegratedDegree.Value);
            actual.Level.Should().Be(standard.Level.Value);
            actual.CoronationEmblem.Should().Be(standard.CoronationEmblem.Value);
            actual.ProposedTypicalDuration.Should().Be(standard.ProposedTypicalDuration.Value);
            actual.ProposedMaxFunding.Should().Be(standard.ProposedMaxFunding.Value);
            actual.OverviewOfRole.Should().Be(standard.OverviewOfRole.Value);
            actual.StandardPageUrl.Should().Be(standard.StandardPageUrl.Value.AbsoluteUri);
            actual.RouteCode.Should().Be(standard.RouteCode.Value);
            actual.AssessmentPlanUrl.Should().Be(standard.AssessmentPlanUrl.Value);
            actual.ApprovedForDelivery.Should().Be(standard.ApprovedForDelivery.Value);
            actual.EqaProviderName.Should().Be(standard.EqaProvider.Value?.ProviderName.Value?.Trim());
            actual.EqaProviderContactName.Should().Be(standard.EqaProvider.Value?.ContactName.Value?.Trim());
            actual.EqaProviderContactEmail.Should().Be(standard.EqaProvider.Value?.ContactEmail.Value?.Trim());
            actual.EqaProviderWebLink.Should().Be(standard.EqaProvider.Value.WebLink.Value);
            actual.Title.Should().Be(standard.Title.Value.Trim());
            actual.TypicalJobTitles.Should().Be(string.Join("|", standard.TypicalJobTitles.Value));
            actual.Version.Should().Be((standard.Version?.Value).ToBaselineVersion());
            actual.RegulatedBody.Should().Be(standard.RegulatedBody.Value?.Trim());
            actual.CreatedDate.Should().Be(standard.CreatedDate.Value);
            actual.PublishDate.Should().Be(standard.PublishDate.Value);
        }

        [Test, StandardAutoData]
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

        [Test, StandardAutoData]
        public void Then_All_Skills_That_Are_Mapped_To_A_Core_Duty_Are_Shown(ImportTypes.Standard standard, string detail, Guid skillId, Duty duty)
        {
            // Arrange	
            standard.Skills.Value.Add(new Skill
                {
                    Detail = detail,
                    SkillId = skillId,
                }
            );

            duty.MappedSkills = new List<Guid> { skillId };
            duty.IsThisACoreDuty = 1;
            standard.Duties.Value.Add(duty);

            //Act	
            var actual = (StandardImport)standard;

            //Assert	
            actual.CoreDuties.Should().BeEquivalentTo(new List<string>{detail});
        }

        [Test, StandardAutoData]
        public void Then_All_Skills_That_Are_Mapped_To_A_Core_Duty_Are_Mapped_In_Same_Order_As_Skills_List(ImportTypes.Standard standard)
        {
            //Arrange	
            foreach (var skill in standard.Skills.Value)
            {
                foreach (var duty in standard.Duties.Value)
                {
                    var random = new Random();
                    if (random.Next(2) == 1)
                    {
                        duty.IsThisACoreDuty = 1;
                        duty.MappedSkills.Value.Add(skill.SkillId.Value);
                    }
                }
            }

            //Act	
            var actual = (StandardImport)standard;
            standard.Skills.Value.RemoveAll(s => !actual.CoreDuties.Contains(s.Detail.Value));

            //Assert
            standard.Skills.Value.Select(s => s.Detail.Value).Should().BeEquivalentTo(actual.CoreDuties);
        }

        [Test, StandardAutoData]
        public void Then_If_The_Version_Is_Null_It_Is_Set_To_DefaultVersion(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Version = null;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Version.Should().Be("1.0");
        }

        [Test, StandardAutoData]
        public void Then_Major_and_Minor_versions_are_mapped(ImportTypes.Standard standard)
        {
            standard.Version = "1.2";

            var actual = (StandardImport)standard;

            actual.VersionMajor.Should().Be(1);
            actual.VersionMinor.Should().Be(2);
        }

        [Test, StandardAutoData]
        public void Then_All_Knowledge_Is_Mapped_To_Correct_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Knowledges = KnowledgeBuilder.Create("k1", "k2", "k3", "k4");
            var options = new[]
            {
                new OptionBuilder().WithKnowledge(standard.Knowledges.Value.Take(2)),
                new OptionBuilder().WithKnowledge(standard.Knowledges.Value.Skip(2)),
            };
            standard.Options = options.Select(x => x.Build()).ToList();
            standard.Duties = new List<Duty>
            {
                new OptionDutyBuilder().ForOptions(options[0]).Build(),
                new OptionDutyBuilder().ForOptions(options[1]).Build(),
            };

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == options[0].OptionId)
                .Which.Knowledge.Should().BeEquivalentTo(new []{
                    new { Id = standard.Knowledges.Value[0].KnowledgeId.Value, Detail = "k1" },
                    new { Id = standard.Knowledges.Value[1].KnowledgeId.Value, Detail = "k2" }});
            actual.Options.Should().Contain(x => x.OptionId == options[1].OptionId)
                .Which.Knowledge.Should().BeEquivalentTo( new [] {
                    new { Id = standard.Knowledges.Value[2].KnowledgeId.Value, Detail = "k3" },
                    new { Id = standard.Knowledges.Value[3].KnowledgeId.Value, Detail = "k4" }});
        }

        [Test, StandardAutoData]
        public void Then_Core_KSBs_Are_Mapped_To_All_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Knowledges = KnowledgeBuilder.Create("k1-detail", "k2-detail", "k3-detail");
            standard.Skills = SkillsBuilder.Create("s1-detail", "s2-detail");
            standard.Behaviours = BehavioursBuilder.Create("b1-detail");
            var option = new OptionBuilder()
                .WithKnowledge(standard.Knowledges.Value.Take(1))
                .WithSkills(standard.Skills.Value.Take(1));
            standard.Options = new List<Option> { option.Build() };
            standard.Duties = new List<Duty>
            {
                new OptionDutyBuilder().ForOptions(option).Build(),
                new CoreDutyBuilder()
                    .WithKnowledge(standard.Knowledges.Value.Skip(1))
                    .WithSkills(standard.Skills.Value.Skip(1))
                    .WithBehaviour(standard.Behaviours.Value)
                    .Build(),
            };

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

        [Test, StandardAutoData]
        public void Then_Core_KSBs_Are_Mapped_To_Standard_Without_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Knowledges = KnowledgeBuilder.Create("k1-detail", "k2-detail", "k3-detail");
            standard.Skills = SkillsBuilder.Create("s1-detail", "s2-detail");
            standard.Behaviours = BehavioursBuilder.Create("b1-detail");
            standard.CoreAndOptions = false;

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

        [Test, StandardAutoData]
        public void Then_All_Skills_Are_Mapped_To_Correct_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Skills = SkillsBuilder.Create("s1", "s2", "s3", "s4", "s5");
            var options = new[]
            {
                new OptionBuilder().WithSkills(standard.Skills.Value.Take(2)),
                new OptionBuilder().WithSkills(standard.Skills.Value.Skip(2)),
            };
            standard.Options = options.Select(x => x.Build()).ToList();
            standard.Duties = new List<Duty>
            {
                new OptionDutyBuilder().ForOptions(options[0]).Build(),
                new OptionDutyBuilder().ForOptions(options[1]).Build(),
            };

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == options[0].OptionId)
                .Which.Skills.Should().BeEquivalentTo(new []{
                    new { Id = standard.Skills.Value[0].SkillId.Value, Detail = "s1" },
                    new { Id = standard.Skills.Value[1].SkillId.Value, Detail = "s2" }});
            actual.Options.Should().Contain(x => x.OptionId == options[1].OptionId)
                .Which.Skills.Should().BeEquivalentTo( new []{
                    new { Id = standard.Skills.Value[2].SkillId.Value, Detail = "s3" },
                    new { Id = standard.Skills.Value[3].SkillId.Value, Detail = "s4" },
                    new { Id = standard.Skills.Value[4].SkillId.Value, Detail = "s5" }});
        }

        [Test, StandardAutoData]
        public void Then_All_Behaviours_Are_Mapped_To_Correct_Options(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Behaviours = BehavioursBuilder.Create("b1", "b2", "b3", "b4", "b5", "b6");
            var options = new[]
            {
                new OptionBuilder().WithBehaviours(standard.Behaviours.Value.Take(1)),
                new OptionBuilder().WithBehaviours(standard.Behaviours.Value.Skip(2).Take(3)),
                new OptionBuilder().WithBehaviours(standard.Behaviours.Value.Skip(3)),
            };
            standard.Options = options.Select(x => x.Build()).ToList();
            standard.Duties = new List<Duty>
            {
                new OptionDutyBuilder().ForOptions(options[0], options[1]).Build(),
                new OptionDutyBuilder().ForOptions(options[2]).Build(),
            };

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == options[0].OptionId)
                .Which.Behaviours.Should().BeEquivalentTo( new []{
                    new { Id = standard.Behaviours.Value[0].BehaviourId.Value, Detail = "b1" },
                    new { Id = standard.Behaviours.Value[2].BehaviourId.Value, Detail = "b3" },
                    new { Id = standard.Behaviours.Value[3].BehaviourId.Value, Detail = "b4" },
                    new { Id = standard.Behaviours.Value[4].BehaviourId.Value, Detail = "b5" }});
            actual.Options.Should().Contain(x => x.OptionId == options[1].OptionId)
                .Which.Behaviours.Should().BeEquivalentTo(new []{
                    new { Id = standard.Behaviours.Value[0].BehaviourId.Value, Detail = "b1" },
                    new { Id = standard.Behaviours.Value[2].BehaviourId.Value, Detail = "b3" },
                    new { Id = standard.Behaviours.Value[3].BehaviourId.Value, Detail = "b4" },
                    new { Id = standard.Behaviours.Value[4].BehaviourId.Value, Detail = "b5" }});
            actual.Options.Should().Contain(x => x.OptionId == options[2].OptionId)
                .Which.Behaviours.Should().BeEquivalentTo(new []{
                    new { Id = standard.Behaviours.Value[3].BehaviourId.Value, Detail = "b4" },
                    new { Id = standard.Behaviours.Value[4].BehaviourId.Value, Detail = "b5" },
                    new { Id = standard.Behaviours.Value[5].BehaviourId.Value, Detail = "b6" }});
        }

        [Test, StandardAutoData]
        public void Then_KSBs_Are_Unique(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Knowledges = KnowledgeBuilder.Create("k1", "k2");
            standard.Skills = SkillsBuilder.Create("s1", "s2");
            standard.Behaviours = BehavioursBuilder.Create("b1");
            var options = new[]
            {
                new OptionBuilder().WithKnowledge(standard.Knowledges.Value),
                new OptionBuilder().WithKnowledge(standard.Knowledges.Value),
            };
            standard.Options = options.Select(x => x.Build()).ToList();
            standard.Duties = new List<Duty>
            {
                new OptionDutyBuilder().ForOptions(options).Build(),
                new CoreDutyBuilder().WithKnowledge(standard.Knowledges.Value).Build(),
            };

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == options[0].OptionId)
                .Which.Knowledge.Should().OnlyHaveUniqueItems();
        }

        [Test, StandardAutoData]
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

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == standard.Options.Value.First().OptionId.Value)
                .Which.Behaviours.Should().BeEmpty();
        }

        [Test, StandardAutoData]
        public void Then_Null_KSBs_For_Options_Are_OK(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Duties = new List<Duty>
            {
                new Duty
                {
                    MappedOptions = standard.Options.Value.Select(x => x.OptionId.Value).ToList(),
                    MappedKnowledge = null,
                    MappedSkills = null,
                    MappedBehaviour = null,
                }
            };

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().Contain(x => x.OptionId == standard.Options.Value.First().OptionId.Value)
                .Which.Behaviours.Should().BeEmpty();
        }

        [Test, StandardAutoData]
        public void Then_All_Duties_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Duties.Should().BeEquivalentTo(standard.Duties.Value.Select(c => c.DutyDetail.Value));
        }

        [Test, StandardAutoData]
        public void Then_All_Options_Are_Mapped(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Select(c => c.Title).Should().BeEquivalentTo(standard.Options.Value.Select(c => c.Title.Value));
        }

        [Test]
        [StandardInlineAutoData("Option")]
        [StandardInlineAutoData(" Option")]
        [StandardInlineAutoData("  Option")]
        [StandardInlineAutoData("Option ")]
        public void And_Option_Contains_Whitespace_Then_Option_Is_Trimmed_Correctly(string optionTitle,ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Options.Value[0].Title = optionTitle;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options[0].Should().NotBeNull()
                .And.BeAssignableTo<StandardOption>()
                .Which.Title.Should().Be("Option");
        }

        [Test, StandardAutoData]
        public void Then_Options_Are_Mapped_To_Empty_List(ImportTypes.Standard standard)
        {
            //Arrange
            standard.CoreAndOptions = true;
            standard.Options = null;
            standard.OptionsUnstructuredTemplate = null;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.Options.Should().BeEmpty();
        }

        [Test]
        [StandardInlineAutoData(3, "integrated degree", false, false)]
        [StandardInlineAutoData(3, "integrated degree", true, true)]
        [StandardInlineAutoData(5, "integrated degree", false, false)]
        [StandardInlineAutoData(5, "integrated degree", true, true)]
        [StandardInlineAutoData(6, "integrated degree", false, true)]
        [StandardInlineAutoData(6, "INTEGRATED degree", false, true)]
        [StandardInlineAutoData(6, "non integrated", false, false)]
        [StandardInlineAutoData(6, "", false, false)]
        [StandardInlineAutoData(6, "abc", false, false)]
        [StandardInlineAutoData(7, "Integrated Degree", false, true)]
        public void Then_If_The_Standard_Is_Level_Six_Or_Above_The_Integrated_Degree_Field_Is_Used_To_Set_The_Standard_As_IntegratedApprenticeship(
            int level, string integratedDegreeValue, bool integratedApprenticeship, bool expected, ImportTypes.Standard standard)
        {
            //Arrange
            standard.Level = level;
            standard.IntegratedDegree = integratedDegreeValue;
            standard.IntegratedApprenticeship = integratedApprenticeship;

            //Act
            var actual = (StandardImport) standard;

            //Assert
            actual.IntegratedApprenticeship.Should().Be(expected);
        }

        [Test]
        [StandardInlineAutoData(3, true, true)]
        [StandardInlineAutoData(4, false, false)]
        [StandardInlineAutoData(5, true, true)]
        [StandardInlineAutoData(6, true, false)]
        [StandardInlineAutoData(6, false, false)]
        [StandardInlineAutoData(6, null, false)]
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

        [Test, StandardAutoData]
        public void Then_If_Change_Is_Empty_Then_EPAChanged_Is_False(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Change = string.Empty;

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.EPAChanged.Should().Be(false);
        }

        [Test, StandardAutoData]
        public void Then_If_Change_Does_Not_Contain_Magic_Value_Then_EPAChanged_Is_False(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Change = "Approved for delivery";

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.EPAChanged.Should().Be(false);
        }

        [Test, StandardAutoData]
        public void Then_If_Change_Equals_Magic_Value_Then_EPAChanged_Is_True(ImportTypes.Standard standard)
        {
            //Arrange
            standard.Change = "End-point assessment plan revised";

            //Act
            var actual = (StandardImport)standard;

            //Assert
            actual.EPAChanged.Should().Be(true);
        }

        [Test, StandardAutoData]
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
        [StandardInlineAutoData("ST0001")]
        [StandardInlineAutoData(" ST0001")]
        [StandardInlineAutoData("  ST0001")]
        [StandardInlineAutoData("ST0001 ")]
        public void Then_ifate_reference_is_trimmed_and_mapped(string ifateReference, ImportTypes.Standard standard)
        {
            standard.ReferenceNumber = ifateReference;

            var actual = (StandardImport)standard;

            actual.IfateReferenceNumber.Should().Be("ST0001");
        }

        [Test]
        [StandardInlineAutoData("Live", "Live")]
        [StandardInlineAutoData(" Approved for delivery", "Approved for delivery")]
        [StandardInlineAutoData("  Retired ", "Retired")]
        [StandardInlineAutoData("Withdrawn ", "Withdrawn")]
        public void Then_status_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.Status = source;

            var actual = (StandardImport)standard;

            actual.Status.Should().Be(expected);
        }

        [Test]
        [StandardInlineAutoData("trailblazer@contact.com", "trailblazer@contact.com")]
        [StandardInlineAutoData(" trailblazer@contact.com", "trailblazer@contact.com")]
        [StandardInlineAutoData("  trailblazer@contact.com ", "trailblazer@contact.com")]
        [StandardInlineAutoData(null, null)]
        public void Then_trail_blazer_contact_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.TbMainContact = source;

            var actual = (StandardImport)standard;

            actual.TrailBlazerContact.Should().Be(expected);
        }

        [Test]
        [StandardInlineAutoData("Provider name", "Provider name")]
        [StandardInlineAutoData(" Provider name", "Provider name")]
        [StandardInlineAutoData("  Provider name ", "Provider name")]
        [StandardInlineAutoData(null, null)]
        public void Then_provider_name_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.EqaProvider.Value.ProviderName = source;

            var actual = (StandardImport)standard;

            actual.EqaProviderName.Should().Be(expected);
        }

        [Test]
        [StandardInlineAutoData("Contact name", "Contact name")]
        [StandardInlineAutoData(" Contact name", "Contact name")]
        [StandardInlineAutoData("  Contact name ", "Contact name")]
        [StandardInlineAutoData(null, null)]
        public void Then_provider_contact_name_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.EqaProvider.Value.ContactName = source;

            var actual = (StandardImport)standard;

            actual.EqaProviderContactName.Should().Be(expected);
        }

        [Test]
        [StandardInlineAutoData("contact@email.com", "contact@email.com")]
        [StandardInlineAutoData(" contact@email.com", "contact@email.com")]
        [StandardInlineAutoData("  contact@email.com ", "contact@email.com")]
        [StandardInlineAutoData(null, null)]
        public void Then_provider_contact_email_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.EqaProvider.Value.ContactEmail = source;

            var actual = (StandardImport)standard;

            actual.EqaProviderContactEmail.Should().Be(expected);
        }

        [Test]
        [StandardInlineAutoData("Regulated body", "Regulated body")]
        [StandardInlineAutoData(" Regulated body", "Regulated body")]
        [StandardInlineAutoData("  Regulated body ", "Regulated body")]
        [StandardInlineAutoData(null, null)]
        public void Then_regulated_body_is_trimmed_and_mapped(string source, string expected, ImportTypes.Standard standard)
        {
            standard.RegulatedBody = source;

            var actual = (StandardImport)standard;

            actual.RegulatedBody.Should().Be(expected);
        }

        [Test, StandardAutoData]
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

            //Act	
            var actual = (StandardImport)standard;

            // Assert
            actual.Duties.Should().BeEmpty();
        }
    }
}
