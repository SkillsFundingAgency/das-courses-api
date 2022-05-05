using System;
using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardOption
    {
        public Guid OptionId { get; set; }
        public string Title { get; set; }
        public List<string> Knowledge { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Behaviours { get; set; }
        public List<Ksb> AllKsbs { get; set; }
    }

    public class Ksb
    {
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
