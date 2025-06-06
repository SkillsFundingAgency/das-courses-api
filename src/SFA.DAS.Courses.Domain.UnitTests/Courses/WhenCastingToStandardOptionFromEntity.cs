using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.UnitTests.Courses;

public class WhenCastingToStandardOptionFromEntity
{
    [Test]
    public void Then_The_Fields_Are_Mapped_Correctly()
    {
        var source = StandardOption.CreateCorePseudoOption(new List<Ksb>
        {
            Ksb.Knowledge(Guid.NewGuid(), 1, "Knowledge Detail"),
            Ksb.Skill(Guid.NewGuid(), 2, "Skill Detail"),
            Ksb.Behaviour(Guid.NewGuid(), 3, "Behaviour Detail"),
            Ksb.TechnicalKnowledge(Guid.NewGuid(), 4, "Technical Knowledge Detail"),
            Ksb.TechnicalSkill(Guid.NewGuid(), 5, "Technical Skill Detail"),
            Ksb.EmployabilitySkillsAndBehaviour(Guid.NewGuid(), 6, "Employability Skills and Behaviour Detail")
        });

        var actual = (Domain.Courses.StandardOption)source;

        actual.Knowledge.Should().HaveCount(1);
        actual.Knowledge[0].Detail.Should().Be("Knowledge Detail");
        actual.Skills.Should().HaveCount(1);
        actual.Skills[0].Detail.Should().Be("Skill Detail");
        actual.Behaviours.Should().HaveCount(1);
        actual.Behaviours[0].Detail.Should().Be("Behaviour Detail");
        actual.TechnicalKnowledges.Should().HaveCount(1);
        actual.TechnicalKnowledges[0].Detail.Should().Be("Technical Knowledge Detail");
        actual.TechnicalSkills.Should().HaveCount(1);
        actual.TechnicalSkills[0].Detail.Should().Be("Technical Skill Detail");
        actual.EmployabilitySkillsAndBehaviours.Should().HaveCount(1);
        actual.EmployabilitySkillsAndBehaviours[0].Detail.Should().Be("Employability Skills and Behaviour Detail");
        actual.Title.Should().Be(Domain.Courses.StandardOption.CoreTitle);
        actual.Ksbs.Should().HaveCount(6);
    }
}
