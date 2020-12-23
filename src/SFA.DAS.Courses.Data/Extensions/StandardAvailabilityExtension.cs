using System;
using System.Linq;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Extensions
{
    public static class StandardAvailabilityExtension
    {
        public static IQueryable<Standard> FilterAvailableToStart(this IQueryable<Standard> standards, bool filterAvailableToStart)
        {
            if (!filterAvailableToStart)
            {
                return standards.Where(ls=>ls.LarsStandard != null);
            }

            var filteredStandards = standards.Where(ls => (ls.LarsStandard.LastDateStarts == null
                                                          || ls.LarsStandard.LastDateStarts >= DateTime.UtcNow)
                                                          && ls.LarsStandard.LastDateStarts != ls.LarsStandard.EffectiveFrom
                                                          && ls.LarsStandard.EffectiveFrom <= DateTime.UtcNow);
            
            return filteredStandards;
        }
    }
}