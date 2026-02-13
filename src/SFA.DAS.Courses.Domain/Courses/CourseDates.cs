using System;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class CourseDates
    {
        public DateTime? LastDateStarts { get ; set ; }

        public DateTime? EffectiveTo { get ; set ; }

        public DateTime EffectiveFrom { get ; set ; }

        public static explicit operator CourseDates(LarsStandard larsStandard)
        {
            if(larsStandard == null)
            {
                return null;
            }

            return new CourseDates
            {
                EffectiveFrom = larsStandard.EffectiveFrom,
                EffectiveTo = larsStandard.EffectiveTo,
                LastDateStarts = larsStandard.LastDateStarts
            };
        }
    }
}
