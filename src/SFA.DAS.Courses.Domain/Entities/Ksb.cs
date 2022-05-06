namespace SFA.DAS.Courses.Domain.Entities
{
    public enum KsbType
    {
        Knowledge = 1,
        Skill = 2,
        Behaviour = 3,
    }

    public class Ksb
    {
        public KsbType Type { get; }
        public string Key { get; }
        public string Detail { get; }

        private Ksb()
        {
            /* Entity Framework */
        }

        public static Ksb Knowledge(int index, string detail)
            => new Ksb(KsbType.Knowledge, index, detail);

        public static Ksb Skill(int index, string detail)
            => new Ksb(KsbType.Skill, index, detail);

        public static Ksb Behaviour(int index, string detail)
            => new Ksb(KsbType.Behaviour, index, detail);

        private Ksb(KsbType type, int index, string detail)
            => (Type, Key, Detail) = (type, $"{type.ToString()[0]}{index}", detail);
    }
}
