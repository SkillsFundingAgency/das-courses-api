using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardOption
    {
        [JsonProperty] public Guid OptionId { get; private set; }
        [JsonProperty] public string Title { get; private set; }
        public List<Ksb> Knowledge => KsbDetail(KsbType.Knowledge);
        public List<Ksb> Skills => KsbDetail(KsbType.Skill);
        public List<Ksb> Behaviours => KsbDetail(KsbType.Behaviour);
        public List<Ksb> TechnicalKnowledges => KsbDetail(KsbType.TechnicalKnowledge);
        public List<Ksb> TechnicalSkills => KsbDetail(KsbType.TechnicalSkill);
        public List<Ksb> EmployabilitySkillsAndBehaviours => KsbDetail(KsbType.EmployabilitySkillsAndBehaviour);

        [JsonProperty]
        public List<Ksb> Ksbs { get; private set; }

        private StandardOption() { }

        //[JsonConstructor]
        //internal StandardOption() { }

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
