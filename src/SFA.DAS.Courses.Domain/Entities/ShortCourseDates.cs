using System;
using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class ShortCourseDates
    {
        public string LarsCode { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? LastDateStarts { get; set; }

        public virtual ICollection<Standard> Standards { get; set; }
    }

}
