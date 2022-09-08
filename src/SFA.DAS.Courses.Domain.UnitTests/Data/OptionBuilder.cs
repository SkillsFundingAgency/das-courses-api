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
        public Behaviour[] Behaviours { get; } = new Behaviour[0];


        private OptionBuilder(Knowledge[] knowledges, Skill[] skills, Behaviour[] behaviours)
            => (Knowledge, Skills, Behaviours) = (knowledges, skills, behaviours);

        public OptionBuilder()
        {
        }

        internal OptionBuilder WithKnowledge(IEnumerable<Knowledge> knowledge)
            => new OptionBuilder(knowledge.ToArray(), Skills, Behaviours);

        internal OptionBuilder WithSkills(IEnumerable<Skill> skills)
            => new OptionBuilder(Knowledge, skills.ToArray(), Behaviours);

        internal OptionBuilder WithBehaviours(IEnumerable<Behaviour> behaviours)
            => new OptionBuilder(Knowledge, Skills, behaviours.ToArray());

        internal Option Build() => new Option { OptionId = OptionId };
    }
}
