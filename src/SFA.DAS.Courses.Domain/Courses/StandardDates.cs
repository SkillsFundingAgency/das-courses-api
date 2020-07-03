using System;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class StandardDates
    {
        public DateTime? LastDateStarts { get ; set ; }

        public DateTime? EffectiveTo { get ; set ; }

        public DateTime EffectiveFrom { get ; set ; }

        public static implicit operator StandardDates(LarsStandard larsStandard)
        {
            return new StandardDates
            {
                EffectiveFrom = larsStandard.EffectiveFrom,
                EffectiveTo = larsStandard.EffectiveTo,
                LastDateStarts = larsStandard.LastDateStarts
            };
        }
    }
}