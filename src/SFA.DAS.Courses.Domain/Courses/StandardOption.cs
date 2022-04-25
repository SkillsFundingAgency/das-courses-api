using System;
using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class StandardOption
    {
        public Guid OptionId { get; set; }
        public string Title { get; set; }
        public List<string> Knowledge { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Behaviours { get; set; }

        public static explicit operator StandardOption(Entities.StandardOption source)
        {
            return new StandardOption();
        }
    }

}
