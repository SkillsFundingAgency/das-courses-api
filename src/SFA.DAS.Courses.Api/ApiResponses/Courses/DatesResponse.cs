using System;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class CourseDatesResponse
    {
        public DateTime? LastDateStarts { get ; set ; }

        public DateTime? EffectiveTo { get ; set ; }

        public DateTime EffectiveFrom { get ; set ; }

        public static explicit operator CourseDatesResponse(CourseDates standardDates)
        {
            if (standardDates == null)
            {
                return null;
            }

            return new CourseDatesResponse
            {
                EffectiveFrom = standardDates.EffectiveFrom,
                EffectiveTo = standardDates.EffectiveTo,
                LastDateStarts = standardDates.LastDateStarts
            };
        }
    }
}
