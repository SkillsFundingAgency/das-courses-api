using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.UnitTests.Data
{
    public class DutyBuilder
    {
        private readonly OptionBuilder[] _options = new OptionBuilder[0];
        private readonly Knowledge[] _knowledge = new Knowledge[0];
        private readonly Skill[] _skills = new Skill[0];
        private readonly Behaviour[] _behaviours = new Behaviour[0];

        private DutyBuilder(OptionBuilder[] options, Knowledge[] knowledge, Skill[] skills, Behaviour[] behaviours)
            => (_options, _knowledge, _skills, _behaviours) = (options, knowledge, skills, behaviours);

        public DutyBuilder()
        {
        }

        public DutyBuilder ForCore()
            => new DutyBuilder(new OptionBuilder[0], _knowledge, _skills, _behaviours);

        public DutyBuilder ForOptions(params OptionBuilder[] options)
            => new DutyBuilder(options, new Knowledge[0], new Skill[0], new Behaviour[0]);

        internal DutyBuilder WithKnowledge(IEnumerable<Knowledge> knowledge)
            => new DutyBuilder(new OptionBuilder[0], knowledge.ToArray(), _skills, _behaviours);

        internal DutyBuilder WithSkills(IEnumerable<Skill> skills)
            => new DutyBuilder(new OptionBuilder[0], _knowledge, skills.ToArray(), _behaviours);

        internal DutyBuilder WithBehaviour(IEnumerable<Behaviour> behaviours)
            => new DutyBuilder(new OptionBuilder[0], _knowledge, _skills, behaviours.ToArray());

        internal Duty Build() => new Duty
        {
            DutyId = Guid.NewGuid(),
            IsThisACoreDuty = _options.Any() ? 0 : 1,
            MappedOptions = _options.Select(x => x.OptionId).ToList(),
            MappedKnowledge = _options.SelectMany(x => x.Knowledge).Union(_knowledge).Select(x => x.KnowledgeId).ToList(),
            MappedSkills = _options.SelectMany(x => x.Skills).Union(_skills).Select(x => x.SkillId).ToList(),
            MappedBehaviour = _options.SelectMany(x => x.Behaviours).Union(_behaviours).Select(x => x.BehaviourId).ToList(),
        };
    }
}
