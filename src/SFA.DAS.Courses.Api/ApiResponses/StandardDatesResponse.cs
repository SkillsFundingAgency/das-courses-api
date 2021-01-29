using System;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class StandardDatesResponse
    {
        public DateTime? LastDateStarts { get ; set ; }

        public DateTime? EffectiveTo { get ; set ; }

        public DateTime EffectiveFrom { get ; set ; }

        public static explicit operator StandardDatesResponse(StandardDates standardDates)
        {
            if(standardDates == null)
            {
                return null;
            }
            return new StandardDatesResponse
            {
                EffectiveFrom = standardDates.EffectiveFrom,
                EffectiveTo = standardDates.EffectiveTo,
                LastDateStarts = standardDates.LastDateStarts
            };
        }
    }
}
