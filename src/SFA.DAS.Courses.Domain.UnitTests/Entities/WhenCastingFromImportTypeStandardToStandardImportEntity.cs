using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using Standard = SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland.Standard;

namespace SFA.DAS.Courses.Domain.UnitTests.Entities;

public class WhenCastingFromImportTypeStandardToStandardImportEntity
{
    private Standard _source;

    [SetUp]
    public void SetUp()
    {
        _source = new()
        {
            ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship,
            ApprovedForDelivery = new Settable<DateTime?>(DateTime.UtcNow),
            AssessmentPlanUrl = new Settable<string>("http://example.com"),
            Change = new Settable<string>("Some change"),
            CreatedDate = new Settable<DateTime>(DateTime.UtcNow),
            EqaProvider = new Settable<EqaProvider>(new EqaProvider
            {
                ContactAddress = new Settable<string>("Address"),
                ContactEmail = new Settable<string>("email@example.com"),
                ContactName = new Settable<string>("Contact Name"),
                ProviderName = new Settable<string>("Provider"),
                WebLink = new Settable<string>("http://provider.com")
            }),
            TbMainContact = new Settable<string>(string.Empty),
            CoreAndOptions = new Settable<bool>(false),
            CoronationEmblem = new Settable<bool>(false),
            RouteCode = new Settable<int>(0),
            VersionEarliestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
            Keywords = new Settable<List<string>>(new List<string>()),
            LarsCode = new Settable<string>("12345"),
            VersionLatestEndDate = new Settable<DateTime?>(DateTime.UtcNow),
            VersionLatestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
            Level = new Settable<int>(5),
            ProposedMaxFunding = new Settable<int>(5000),
            OverviewOfRole = new Settable<string>("Overview"),
            PublishDate = new Settable<DateTime>(DateTime.UtcNow),
            ReferenceNumber = new Settable<string>("FA0001"),
            RegulatedBody = new Settable<string>("Regulator"),
            Regulated = new Settable<bool>(false),
            RegulationDetail = new Settable<List<RegulationDetail>>(new List<RegulationDetail>()),
            Route = new Settable<string>("Engineering"),
            Status = new Settable<string>("Approved for delivery"),
            Title = new Settable<string>("Title"),
            ProposedTypicalDuration = new Settable<int>(12),
            TypicalJobTitles = new Settable<List<string>>(new List<string>()),
            Version = new Settable<string>("1.0"),
            VersionNumber = new Settable<string>("1.0"),
            FoundationApprenticeshipUrl = new Settable<Uri>(new Uri("http://foundation.com")),
            TechnicalKnowledges = new Settable<List<IdDetailPair>>
            {
                Value = new List<IdDetailPair>
                {
                    new IdDetailPair { Id = Guid.NewGuid(), Detail = "Technical Knowledge 1" },
                    new IdDetailPair { Id = Guid.NewGuid(), Detail = "Technical Knowledge 2" }
                }
            },
            TechnicalSkills = new Settable<List<IdDetailPair>>
            {
                Value = new List<IdDetailPair>
                {
                    new IdDetailPair { Id = Guid.NewGuid(), Detail = "Technical Skill 1" },
                    new IdDetailPair { Id = Guid.NewGuid(), Detail = "Technical Skill 2" }
                }
            },
            EmployabilitySkillsAndBehaviours = new Settable<List<IdDetailPair>>
            {
                Value = new List<IdDetailPair>
                {
                    new IdDetailPair { Id = Guid.NewGuid(), Detail = "Employability Skill 1" },
                    new IdDetailPair { Id = Guid.NewGuid(), Detail = "Employability Skill 2" }
                }
            },
            RelatedOccupations = new Settable<List<RelatedOccupation>>(
            [
                new() { Name = Guid.NewGuid().ToString(), Reference="OCC0001"},
                new() { Name = Guid.NewGuid().ToString(), Reference="OCC0002A"},
                new() { Name = Guid.NewGuid().ToString(), Reference="OCC0002B"},
            ])
        };
    }

    [Test]
    public void Then_The_Ksbs_Are_Mapped_Correctly()
    {
        //Act
        StandardImport actual = _source;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Options[0].TechnicalKnowledges.Should().HaveCount(_source.TechnicalKnowledges.Value.Count);
            actual.Options[0].TechnicalSkills.Should().HaveCount(_source.TechnicalSkills.Value.Count);
            actual.Options[0].EmployabilitySkillsAndBehaviours.Should().HaveCount(_source.EmployabilitySkillsAndBehaviours.Value.Count);
            actual.Options[0].TechnicalKnowledges.Select(k => k.Detail).Should().BeEquivalentTo(_source.TechnicalKnowledges.Value.Select(k => k.Detail.Value));
            actual.Options[0].TechnicalSkills.Select(k => k.Detail).Should().BeEquivalentTo(_source.TechnicalSkills.Value.Select(k => k.Detail.Value));
            actual.Options[0].EmployabilitySkillsAndBehaviours.Select(k => k.Detail)
                .Should().BeEquivalentTo(_source.EmployabilitySkillsAndBehaviours.Value.Select(k => k.Detail.Value));
        }
    }

    [Test]
    public void Then_The_RelatedOccupations_Are_Mapped_Correctly()
    {
        // Act
        StandardImport actual = _source;
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.RelatedOccupations.Should().BeEquivalentTo("ST0001", "ST0002");
        }
    }
}
