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
                case StandardFilter.NotYetApproved:
                    standards = standards.FilterNotYetApproved();
                    break;
                case StandardFilter.None:
                    break;
            }

            return standards;
        }

        private static IQueryable<Standard> FilterActiveAvailableToStart(this IQueryable<Standard> standards)
        {
            var filteredStandards = standards
                .FilterActive()
                .IsAvailableToStart();

            return filteredStandards;
        }

        private static IQueryable<Standard> FilterActive(this IQueryable<Standard> standards)
        {
            var filteredStandards = standards
                .HasLarsStandard()
                .IsLatestVersion()
                .StatusIsOneOf("Approved for delivery");

            return filteredStandards;
        }

        private static IQueryable<Standard> FilterNotYetApproved(this IQueryable<Standard> standards)
        {
            var filteredStandards = standards.Where(ls => ls.LarsCode == 0)
                                    .StatusIsOneOf("Proposal in development", "In development");
                
            return filteredStandards;
        }

        private static IQueryable<Standard> IsAvailableToStart(this IQueryable<Standard> standards)
        {
            return standards.Where(ls => (ls.LarsStandard.LastDateStarts == null
                                                          || ls.LarsStandard.LastDateStarts >= DateTime.UtcNow)
                                                          && ls.LarsStandard.LastDateStarts != ls.LarsStandard.EffectiveFrom
                                                          && ls.LarsStandard.EffectiveFrom <= DateTime.UtcNow);
        }

        private static IQueryable<Standard> HasLarsStandard(this IQueryable<Standard> standards)
        {
            return standards.Where(ls => ls.LarsStandard != null);
        }

        private static IQueryable<Standard> StatusIsOneOf(this IQueryable<Standard> standards, params string[] statuses)
        {
            return standards.Where(ls => statuses.Contains(ls.Status, StringComparer.InvariantCultureIgnoreCase));
        }

        private static IQueryable<Standard> IsLatestVersion(this IQueryable<Standard> standards)
        {
            return standards.GroupBy(c => c.LarsCode).Select(c => c.OrderByDescending(x => x.Version).FirstOrDefault());
        }
    }
}
