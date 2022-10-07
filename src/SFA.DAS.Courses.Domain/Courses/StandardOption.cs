using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class StandardOption
    {
        public const string CoreTitle = "core";

        public Guid OptionId { get; set; }
        public string Title { get; set; }
        public bool IsRealOption => Title != CoreTitle;
        public List<Ksb> Knowledge { get; set; }
        public List<Ksb> Skills { get; set; }
        public List<Ksb> Behaviours { get; set; }
        public List<Ksb> Ksbs { get; set; }

        public static explicit operator StandardOption(Entities.StandardOption source)
        {
            return new StandardOption
            {
                OptionId = source.OptionId,
                Title = source.Title,
                Knowledge = source.Knowledge?.Select(x => (Ksb)x).ToList(),
                Skills = source.Skills?.Select(x => (Ksb)x).ToList(),
                Behaviours = source.Behaviours?.Select(x => (Ksb)x).ToList(),
                Ksbs = source.Ksbs?.Select(x => (Ksb)x).ToList(),
            };
        }
    }
}
