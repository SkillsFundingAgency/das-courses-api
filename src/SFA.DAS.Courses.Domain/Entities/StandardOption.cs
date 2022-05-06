using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardOption
    {
        public Guid OptionId { get; set; }
        public string Title { get; set; }
        public List<string> Knowledge => KsbDetail(KsbType.Knowledge);
        public List<string> Skills => KsbDetail(KsbType.Skill);
        public List<string> Behaviours => KsbDetail(KsbType.Behaviour);
        public List<Ksb> Ksbs { get; set; }

        private List<string> KsbDetail(KsbType knowledge)
            => Ksbs?
            .Where(x => x.Type == knowledge)
            .Select(x => x.Detail)
            .ToList();
    }
}
