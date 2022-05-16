using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardOption
    {
        public Guid OptionId { get; private set; }
        public string Title { get; private set; }
        public List<string> Knowledge => KsbDetail(KsbType.Knowledge);
        public List<string> Skills => KsbDetail(KsbType.Skill);
        public List<string> Behaviours => KsbDetail(KsbType.Behaviour);
        public List<Ksb> Ksbs { get; private set; }

        public static StandardOption Create(string title)
            => new StandardOption { Title = title };

        public static StandardOption Create(Guid id, string title, List<Ksb> ksbs)
            => new StandardOption { OptionId = id, Title = title, Ksbs = ksbs, };

        public static StandardOption CreateCorePseudoOption(List<Ksb> ksbs)
            => new StandardOption { Title = Courses.StandardOption.CoreTitle, Ksbs = ksbs, };

        private List<string> KsbDetail(KsbType knowledge)
            => Ksbs?
            .Where(x => x.Type == knowledge)
            .Select(x => x.Detail)
            .ToList();
    }
}
