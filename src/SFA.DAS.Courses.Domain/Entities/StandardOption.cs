using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardOption
    {
        [JsonInclude] public Guid OptionId { get; private set; }
        [JsonInclude] public string Title { get; private set; }
        public List<Ksb> Knowledge => KsbDetail(KsbType.Knowledge);
        public List<Ksb> Skills => KsbDetail(KsbType.Skill);
        public List<Ksb> Behaviours => KsbDetail(KsbType.Behaviour);
        public List<Ksb> TechnicalKnowledges => KsbDetail(KsbType.TechnicalKnowledge);
        public List<Ksb> TechnicalSkills => KsbDetail(KsbType.TechnicalSkill);
        public List<Ksb> EmployabilitySkillsAndBehaviours => KsbDetail(KsbType.EmployabilitySkillsAndBehaviour);

        [JsonInclude]
        public List<Ksb> Ksbs { get; private set; }

        [JsonConstructor]
        private StandardOption() { }

        public static StandardOption Create(string title)
            => new StandardOption { Title = title };

        public static StandardOption Create(Guid id, string title, List<Ksb> ksbs)
            => new StandardOption { OptionId = id, Title = title, Ksbs = ksbs, };

        public static StandardOption CreateCorePseudoOption(List<Ksb> ksbs)
            => new StandardOption { Title = Courses.StandardOption.CoreTitle, Ksbs = ksbs, };

        private List<Ksb> KsbDetail(KsbType ksbType)
            => Ksbs?
            .Where(x => x.Type == ksbType)
            .ToList();
    }
}
