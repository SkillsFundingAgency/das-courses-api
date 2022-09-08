using System;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class Ksb
    {
        public KsbType Type { get; set; }
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Detail { get; set; }

        public static explicit operator Ksb(Entities.Ksb source)
        {
            return new Ksb
            {
                Type = (KsbType)source.Type,
                Id = source.Id,
                Key = source.Key,
                Detail = source.Detail,
            };
        }
    }

    public enum KsbType
    {
        Knowledge = 1,
        Skill = 2,
        Behaviour = 3,
    }
}
