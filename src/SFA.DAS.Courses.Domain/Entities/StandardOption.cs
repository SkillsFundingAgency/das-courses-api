﻿using System;
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
        [JsonProperty]
        public List<Ksb> Ksbs { get; private set; }

        private StandardOption() { }

        public static StandardOption Create(string title)
            => new StandardOption { Title = title };

        public static StandardOption Create(Guid id, string title, List<Ksb> ksbs)
            => new StandardOption { OptionId = id, Title = title, Ksbs = ksbs, };

        public static StandardOption CreateCorePseudoOption(List<Ksb> ksbs)
            => new StandardOption { Title = Courses.StandardOption.CoreTitle, Ksbs = ksbs, };

        private List<Ksb> KsbDetail(KsbType knowledge)
            => Ksbs?
            .Where(x => x.Type == knowledge)
            .ToList();
    }
}
