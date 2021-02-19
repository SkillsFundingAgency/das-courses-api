using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public static IEnumerable<Standard> InMemoryFilterIsLatestVersion(this IEnumerable<Standard> standards, StandardFilter filter)
        {
            switch (filter)
            {
                case StandardFilter.Active:
                case StandardFilter.ActiveAvailable:
                    {
                        // There are two exception cases where standard with same IfateRefNum 
                        // has two records each, one in `retired` and other in `approved for delivery` status. 
                        // However each version has a distinct LarsCode as seen below.
                        // RefNum  LarsCode    Version  Status
                        // ST0096  288         1.0      Retired
                        // ST0096  529         2.0      Approved for delivery
                        // ST0313  73          1.0      Retired
                        // ST0313  222         2.0      Approved for delivery
                        // Going forward this should never happen.
                        // All versions of standards should have the same LarsCode.
                        // To include all of the above we need to create a list that is grouped by LarsCode (otherwise unnecessary)
                        // and union it with the `actual` list which is grouped by IFateReferenceNumber 

                        var standardsByLarsCode = standards.GroupBy(s => s.LarsCode).Select(c => c.OrderByDescending(x => x.Version).FirstOrDefault());

                        var standardsByIfateReferenceNumber = standards.GroupBy(s => s.IfateReferenceNumber).Select(c => c.OrderByDescending(x => x.Version).FirstOrDefault());

                        var union = standardsByLarsCode.Union(standardsByIfateReferenceNumber).ToList();
                        return union;
                    }
                default:
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
                .StatusIsOneOf("Approved for delivery", "Retired");

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
            //Database case insensitive so satisfies the translated SQL IN statement
            return standards.Where(ls => statuses.Contains(ls.Status));
        }
    }
}
