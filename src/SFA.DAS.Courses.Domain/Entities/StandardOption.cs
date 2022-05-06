using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardOption
    {
        public Guid OptionId { get; set; }
        public string Title { get; set; }
        public List<string> Knowledge => AllKsbs?.Where(x => x.Type == KsbType.Knowledge).Select(x => x.Detail).ToList();
        public List<string> Skills => AllKsbs?.Where(x => x.Type == KsbType.Skill).Select(x => x.Detail).ToList();
        public List<string> Behaviours => AllKsbs?.Where(x => x.Type == KsbType.Behaviour).Select(x => x.Detail).ToList();
        public List<Ksb> AllKsbs { get; set; }
    }

    public class Ksb
    {
        public static Ksb Knowledge(int index, string detail)
            => new Ksb { Type = KsbType.Knowledge, Key = $"K{index}", Detail = detail };
        
        public static Ksb Skill(int index, string detail)
            => new Ksb { Type = KsbType.Skill, Key = $"S{index}", Detail = detail };
        
        public static Ksb Behaviour(int index, string detail)
            => new Ksb { Type = KsbType.Behaviour, Key = $"B{index}", Detail = detail };

        public KsbType Type { get; set; }
        public string Key { get; set; }
        public string Detail { get; set; }
    }

    public enum KsbType
    {
        Knowledge = 1,
        Skill = 2,
        Behaviour = 3,
    }
}
