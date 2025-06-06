using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public enum KsbType
    {
        Knowledge = 1,
        Skill = 2,
        Behaviour = 3,
        TechnicalKnowledge = 4,
        TechnicalSkill = 5,
        EmployabilitySkillsAndBehaviour = 6
    }

    public class Ksb
    {
        private Ksb()
        {
            /* Entity Framework */
        }

        public static Ksb Knowledge(Guid id, int index, string detail)
            => new Ksb(KsbType.Knowledge, id, index, detail);

        public static Ksb Skill(Guid id, int index, string detail)
            => new Ksb(KsbType.Skill, id, index, detail);

        public static Ksb Behaviour(Guid id, int index, string detail)
            => new Ksb(KsbType.Behaviour, id, index, detail);

        public static Ksb TechnicalKnowledge(Guid id, int index, string detail)
            => new Ksb(KsbType.TechnicalKnowledge, id, "TK", index, detail);

        public static Ksb TechnicalSkill(Guid id, int index, string detail)
                => new Ksb(KsbType.TechnicalSkill, id, "TS", index, detail);

        public static Ksb EmployabilitySkillsAndBehaviour(Guid id, int index, string detail)
            => new Ksb(KsbType.EmployabilitySkillsAndBehaviour, id, "ESB", index, detail);

        private Ksb(KsbType type, Guid id, int index, string detail)
        {
            Type = type;
            Id = id;
            Key = $"{type.ToString()[0]}{index}";
            Detail = detail;
        }

        private Ksb(KsbType type, Guid id, string keyPrefix, int index, string detail)
        {
            Type = type;
            Id = id;
            Key = $"{keyPrefix}{index}";
            Detail = detail;
        }

        public KsbType Type { get; set; }
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Detail { get; set; }

    }
}
