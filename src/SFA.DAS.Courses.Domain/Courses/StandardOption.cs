using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class StandardOption
    {
        public Guid OptionId { get; set; }
        public string Title { get; set; }
        public List<string> Knowledge { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Behaviours { get; set; }
        public List<Ksb> AllKsbs { get; set; }

        public static explicit operator StandardOption(Entities.StandardOption source)
        {
            return new StandardOption
            {
                OptionId = source.OptionId,
                Title = source.Title,
                Knowledge = source.Knowledge,
                Skills = source.Skills,
                Behaviours = source.Behaviours,
                AllKsbs = source.AllKsbs?.Select(x => (Ksb)x).ToList(),
            };
        }
    }

    public class Ksb
    {
        public KsbType Type { get; set; }
        public string Key { get; set; }
        public string Detail { get; set; }

        public static explicit operator Ksb(Entities.Ksb source)
        {
            return new Ksb
            {
                Type = (KsbType)source.Type,
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
