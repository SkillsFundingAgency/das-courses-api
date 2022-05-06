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
}
