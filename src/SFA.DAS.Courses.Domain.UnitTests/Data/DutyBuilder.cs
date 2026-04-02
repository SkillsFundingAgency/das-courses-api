using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Domain.UnitTests.Data
{
    public class OptionDutyBuilder
    {
        private readonly OptionBuilder[] _options = new OptionBuilder[0];

        private OptionDutyBuilder(OptionBuilder[] options)
            => _options = options;

        public OptionDutyBuilder()
        {
        }

        public OptionDutyBuilder ForOptions(params OptionBuilder[] options)
            => new OptionDutyBuilder(options);

        internal Duty Build() => new Duty
        {
            DutyId = Guid.NewGuid(),
            IsThisACoreDuty = 0,
            MappedOptions = _options.Select(x => x.OptionId).ToList(),
            MappedKnowledge = _options.SelectMany(x => x.Knowledge).Select(x => x.KnowledgeId.Value).ToList(),
            MappedSkills = _options.SelectMany(x => x.Skills).Select(x => x.SkillId.Value).ToList(),
            MappedBehaviour = _options.SelectMany(x => x.Behaviours).Select(x => x.BehaviourId.Value).ToList(),
        };
    }

    public class CoreDutyBuilder
    {
        private readonly Knowledge[] _knowledge = new Knowledge[0];
        private readonly Skill[] _skills = new Skill[0];
        private readonly Behaviour[] _behaviours = new Behaviour[0];

        private CoreDutyBuilder(Knowledge[] knowledge, Skill[] skills, Behaviour[] behaviours)
            => (_knowledge, _skills, _behaviours) = (knowledge, skills, behaviours);

        public CoreDutyBuilder()
        {
        }

        internal CoreDutyBuilder WithKnowledge(IEnumerable<Knowledge> knowledge)
            => new CoreDutyBuilder(knowledge.ToArray(), _skills, _behaviours);

        internal CoreDutyBuilder WithSkills(IEnumerable<Skill> skills)
            => new CoreDutyBuilder(_knowledge, skills.ToArray(), _behaviours);

        internal CoreDutyBuilder WithBehaviour(IEnumerable<Behaviour> behaviours)
            => new CoreDutyBuilder(_knowledge, _skills, behaviours.ToArray());

        internal Duty Build() => new Duty
        {
            DutyId = Guid.NewGuid(),
            IsThisACoreDuty = 1,
            MappedOptions = null,
            MappedKnowledge = _knowledge.Select(x => x.KnowledgeId.Value).ToList(),
            MappedSkills = _skills.Select(x => x.SkillId.Value).ToList(),
            MappedBehaviour = _behaviours.Select(x => x.BehaviourId.Value).ToList(),
        };
    }
}
