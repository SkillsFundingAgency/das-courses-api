using System;
using System.Linq;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Data.Extensions
{
    public static class StandardFilterExtension
    {
        public static IQueryable<Standard> FilterStandards(this IQueryable<Standard> standards, StandardFilter filter)
        {
            switch (filter)
            {
                case StandardFilter.Active:
                    standards = standards.FilterActive();
                    break;
                case StandardFilter.ActiveAvailable:
                    standards = standards.FilterActiveAvailableToStart();
                    break;
                case StandardFilter.None:
                    break;
            }

            return standards;
        }

        private static IQueryable<Standard> FilterActiveAvailableToStart(this IQueryable<Standard> standards)
        {
            var filteredStandards = standards.Where(ls => (ls.LarsStandard.LastDateStarts == null
                                                          || ls.LarsStandard.LastDateStarts >= DateTime.UtcNow)
                                                          && ls.LarsStandard.LastDateStarts != ls.LarsStandard.EffectiveFrom
                                                          && ls.LarsStandard.EffectiveFrom <= DateTime.UtcNow);

            return filteredStandards;
        }

        private static IQueryable<Standard> FilterActive(this IQueryable<Standard> standards)
        {
            var filteredStandards = standards.Where(ls => ls.LarsStandard != null);

            return filteredStandards;
        }
    }
}
