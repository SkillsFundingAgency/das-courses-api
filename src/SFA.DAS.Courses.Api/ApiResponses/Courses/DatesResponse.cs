using System;
using OpenTelemetry.Resources;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class CourseDatesResponse
    {
        public DateTime? LastDateStarts { get ; set ; }

        public DateTime? EffectiveTo { get ; set ; }

        public DateTime EffectiveFrom { get ; set ; }

        public static explicit operator CourseDatesResponse(CourseDates source)
        {
            if (source == null)
                return null;

            return new CourseDatesResponse
            {
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                LastDateStarts = source.LastDateStarts
            };
        }
    }
}
