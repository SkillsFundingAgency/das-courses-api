﻿using System;
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
        public List<Ksb> Ksbs { get; set; }

        public static explicit operator StandardOption(Entities.StandardOption source)
        {
            return new StandardOption
            {
                OptionId = source.OptionId,
                Title = source.Title,
                Knowledge = source.Knowledge,
                Skills = source.Skills,
                Behaviours = source.Behaviours,
                Ksbs = source.Ksbs?.Select(x => (Ksb)x).ToList(),
            };
        }
    }
}
