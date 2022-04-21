using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.UnitTests.Data
{
    public class OptionBuilder
    {
        public Guid OptionId { get; } = Guid.NewGuid();

        public Knowledge[] Knowledge { get; } = new Knowledge[0];
        public Skill[] Skills { get; } = new Skill[0];


        private OptionBuilder(Knowledge[] knowledges, Skill[] skills)
            => (Knowledge, Skills) = (knowledges, skills);

        public OptionBuilder()
        {
        }

        internal OptionBuilder WithKnowledge(IEnumerable<Knowledge> knowledge)
            => new OptionBuilder(knowledge.ToArray(), Skills);

        internal OptionBuilder WithSkills(IEnumerable<Skill> skills)
            => new OptionBuilder(Knowledge, skills.ToArray());

        internal Option Build() => new Option { OptionId = OptionId };
    }
}
