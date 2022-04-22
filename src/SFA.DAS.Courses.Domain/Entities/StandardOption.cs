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
    }
}
