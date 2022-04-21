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

        private DutyBuilder(OptionBuilder[] options, Knowledge[] knowledge)
            => (_options, _knowledge) = (options, knowledge);

        public DutyBuilder()
        {
        }

        public DutyBuilder ForOptions(params OptionBuilder[] options)
            => new DutyBuilder(options, _knowledge);

        internal DutyBuilder WithKnowledge(IEnumerable<Knowledge> knowledge)
            => new DutyBuilder(_options, knowledge.ToArray());

        internal Duty Build() => new Duty
        {
            DutyId = Guid.NewGuid(),
            MappedOptions = _options.Select(x => x.OptionId).ToList(),
            MappedKnowledge = _options.SelectMany(x => x.Knowledge).Select(x => x.KnowledgeId).ToList(),
            MappedSkills = _options.SelectMany(x => x.Skills).Select(x => Guid.Parse(x.SkillId)).ToList(),
        };
    }
}
