using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Data.Extensions
{
    public enum DistinctInMemoryFilterType
    {
        ByIfateReferenceNumber,
        ByLarsCode,
        ByIfateReferenceNumberAndLarsCode
    }

    public static class StandardFilterExtension
    {
        public static IQueryable<Standard> FilterCourseType(this IQueryable<Standard> standards, CourseType? courseType)
        {
            if (courseType.HasValue)
            {
                return standards.Where(s => s.CourseType == courseType);
            }

            return standards;
        }

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
                case StandardFilter.ClosedToNewStarts:
                    standards = standards.FilterClosedToNewStarts();
                    break;
                case StandardFilter.None:
                    break;
            }

            return standards;
        }

        public static IEnumerable<Standard> InMemoryFilterIsLatestVersion(
            this IEnumerable<Standard> standards,
            StandardFilter filter,
            DistinctInMemoryFilterType inMemoryFilterType)
        {
            switch (filter)
            {
                case StandardFilter.Active:
                case StandardFilter.ActiveAvailable:
                    {
                        var latestByIfateReferenceNumber = GetLatestVersionPerGroup(
                            standards,
                            s => s.IfateReferenceNumber);

                        // There are several exception cases where standards with same IfateReferenceNumber 
                        // have more than one record each, one in `retired` and another in `approved for delivery` status 
                        // where each version has a distinct LarsCode, some example are shown below

                        // RefNum  LarsCode    Version  Status
                        // ST0096  288         1.0      Retired
                        // ST0096  529         2.0      Approved for delivery
                        // ST0313  73          1.0      Retired
                        // ST0313  222         2.0      Approved for delivery

                        // To include all of the above we will create a list that is grouped by LarsCode (otherwise unnecessary)
                        // and union it with the `actual` list which is grouped by IFateReferenceNumber 
                        var latestByLarsCode = GetLatestVersionPerGroup(
                            standards,
                            s => s.LarsCode);

                        return inMemoryFilterType switch
                        {
                            DistinctInMemoryFilterType.ByIfateReferenceNumber => latestByIfateReferenceNumber,
                            DistinctInMemoryFilterType.ByLarsCode => latestByLarsCode,
                            DistinctInMemoryFilterType.ByIfateReferenceNumberAndLarsCode => latestByIfateReferenceNumber.Union(latestByLarsCode),
                            _ => standards
                        };
                    }

                default:
                    return standards;
            }
        }

        private static IEnumerable<Standard> GetLatestVersionPerGroup<TKey>(
            IEnumerable<Standard> standards,
            Func<Standard, TKey> keySelector)
        {
            return standards
                .GroupBy(keySelector)
                .Select(g => g
                    .OrderByDescending(x => x.VersionMajor)
                    .ThenByDescending(x => x.VersionMinor)
                    .FirstOrDefault())
                .Where(x => x != null)!;
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
                .IsValid()
                .StatusIsOneOf(Domain.Courses.Status.ApprovedForDelivery, Domain.Courses.Status.Retired);

            return filteredStandards;
        }

        private static IQueryable<Standard> FilterNotYetApproved(this IQueryable<Standard> standards)
        {
            var filteredStandards = standards
                .IsNotValid()
                .StatusIsOneOf(Domain.Courses.Status.ProposalInDevelopment, Domain.Courses.Status.InDevelopment);

            return filteredStandards;
        }

        private static IQueryable<Standard> FilterClosedToNewStarts(this IQueryable<Standard> standards)
        {
            var filteredStandards = standards
                .IsValid()
                .IsPastLastStartDate();

            return filteredStandards;
        }

        private static IQueryable<Standard> IsAvailableToStart(this IQueryable<Standard> standards)
        {
            var now = DateTime.UtcNow;

            return standards.Where(s =>
                (s.CourseType == CourseType.ShortCourse
                    && (s.ShortCourseDates.LastDateStarts == null || s.ShortCourseDates.LastDateStarts >= now)
                    && s.ShortCourseDates.LastDateStarts != s.ShortCourseDates.EffectiveFrom
                    && s.ShortCourseDates.EffectiveFrom <= now)
                ||
                (s.CourseType != CourseType.ShortCourse
                    && (s.LarsStandard.LastDateStarts == null || s.LarsStandard.LastDateStarts >= now)
                    && s.LarsStandard.LastDateStarts != s.LarsStandard.EffectiveFrom
                    && s.LarsStandard.EffectiveFrom <= now));
        }
        
        private static IQueryable<Standard> IsPastLastStartDate(this IQueryable<Standard> standards)
        {
            var now = DateTime.UtcNow;

            return standards.Where(s =>
                (s.CourseType == CourseType.ShortCourse
                    && s.ShortCourseDates.LastDateStarts != null
                    && s.ShortCourseDates.LastDateStarts < now)
                ||
                (s.CourseType != CourseType.ShortCourse
                    && s.LarsStandard.LastDateStarts != null
                    && s.LarsStandard.LastDateStarts < now));
        }

        private static IQueryable<Standard> IsNotValid(this IQueryable<Standard> standards)
        {
            return standards.Where(s =>
                (s.CourseType == CourseType.ShortCourse
                    && s.ShortCourseDates == null)
                ||
                (s.CourseType != CourseType.ShortCourse
                    && s.LarsStandard == null));
        }

        private static IQueryable<Standard> IsValid(this IQueryable<Standard> standards)
        {
            return standards.Where(s =>
                (s.CourseType == CourseType.ShortCourse
                    && s.ShortCourseDates != null)
                ||
                (s.CourseType != CourseType.ShortCourse
                    && s.LarsStandard != null));
        }

        private static IQueryable<Standard> StatusIsOneOf(this IQueryable<Standard> standards, params string[] statuses)
        {
            // database case insensitive so satisfies the translated SQL IN statement
            return standards.Where(ls => statuses.Contains(ls.Status));
        }
    }
}
