using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Identifiers;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;

using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;
using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.Courses.Services
{
    public class StandardsService : IStandardsService
    {
        private readonly IStandardRepository _standardsRepository;
        private readonly ISearchManager _searchManager;
        private readonly IStandardsSortOrderService _sortOrderService;

        public StandardsService(
            IStandardRepository standardsRepository,
            ISearchManager searchManager,
            IStandardsSortOrderService sortOrderService)
        {
            _standardsRepository = standardsRepository;
            _searchManager = searchManager;
            _sortOrderService = sortOrderService;
        }

        public async Task<IEnumerable<Standard>> GetStandardsList(
            string keyword,
            IList<int> routeIds,
            IList<int> levels,
            OrderBy orderBy,
            StandardFilter filter,
            bool includeAllProperties,
            ApprenticeshipType? apprenticeshipType)
        {
            var standards = await GetList(keyword, routeIds, levels, orderBy, filter, includeAllProperties, apprenticeshipType, CourseType.Apprenticeship);
            return standards.Select(standard => (Standard)standard);
        }

        public async Task<IEnumerable<Course>> GetCoursesList(
            string keyword,
            IList<int> routeIds,
            IList<int> levels,
            OrderBy orderBy,
            StandardFilter filter,
            bool includeAllProperties,
            ApprenticeshipType? apprenticeshipType,
            CourseType? courseType = null)
        {
            var standards = await GetList(keyword, routeIds, levels, orderBy, filter, includeAllProperties, apprenticeshipType, courseType);
            var courses = standards.Select(standard => (Course)standard);

            foreach(var course in courses)
            {
                if (course != null && course.CourseDates == null && course.CourseType == CourseType.ShortCourse)
                {
                    course.CourseDates = await GetCourseDates(course.LarsCode, courseType);
                }
            }

            return courses;
        }

        public async Task<IEnumerable<Standard>> GetAllVersionsOfAStandard(string iFateReferenceNumber, CourseType? courseType)
        {
            var standards = await _standardsRepository.GetStandards(iFateReferenceNumber, courseType);

            return standards.Select(standard => (Standard)standard);
        }

        public async Task<int> CountStandards(StandardFilter filter = StandardFilter.None)
        {
            var count = await _standardsRepository.Count(filter, CourseType.Apprenticeship);
            return count;
        }

        public async Task<int> CountCourses(StandardFilter filter = StandardFilter.None, CourseType? courseType = null)
        {
            var count = await _standardsRepository.Count(filter, courseType);
            return count;
        }

        public async Task<Standard> GetStandardByAnyId(string id)
        {
            if (IsLarsCode(id))
            {
                return await GetLatestActiveStandard(id);
            }
            else if (IsStandardReference(id))
            {
                return await GetLatestActiveStandardByIfateReferenceNumber(id);
            }
            else
            {
                return await GetStandard(id);
            }
        }

        public async Task<Course> GetCourseByAnyId(string id)
        {
            if (IsLarsCode(id))
            {
                return await GetLatestActiveCourse(id, null);
            }
            else if (IsStandardReference(id))
            {
                return await GetLatestActiveCourseByIfateReferenceNumber(id, null);
            }
            else
            {
                return await GetCourse(id, null);
            }
        }

        private async Task<IEnumerable<Domain.Entities.Standard>> GetList(
            string keyword,
            IList<int> routeIds,
            IList<int> levels,
            OrderBy orderBy,
            StandardFilter filter,
            bool includeAllProperties,
            ApprenticeshipType? apprenticeshipType,
            CourseType? courseType = null)
        {
            var standards = await _standardsRepository.GetStandards(routeIds, levels, filter, includeAllProperties, apprenticeshipType, courseType);

            if (!string.IsNullOrEmpty(keyword))
            {
                standards = FindByKeyword(standards, keyword);
            }

            standards = _sortOrderService.OrderBy(standards, orderBy, keyword);

            return standards;
        }

        private async Task<Standard> GetLatestActiveStandard(string larsCode)
        {
            var standard = await _standardsRepository.GetLatestActiveStandard(larsCode, CourseType.Apprenticeship);
            return await StandardWithRelatedOccupations(standard);
        }

        private async Task<Standard> GetLatestActiveStandardByIfateReferenceNumber(string iFateReferenceNumber)
        {
            var standard = await _standardsRepository.GetLatestActiveStandardByIfateReferenceNumber(iFateReferenceNumber, CourseType.Apprenticeship);
            return await StandardWithRelatedOccupations(standard);
        }

        private async Task<Standard> GetStandard(string standardUId)
        {
            var standard = await _standardsRepository.Get(standardUId, CourseType.Apprenticeship);
            return await StandardWithRelatedOccupations(standard);
        }

        private async Task<Course> GetLatestActiveCourse(string larsCode, CourseType? courseType)
        {
            var latestActiveStandard = await _standardsRepository.GetLatestActiveStandard(larsCode, courseType);
            var course = await CourseWithRelatedOccupations(latestActiveStandard);

            if (course != null && course.CourseDates == null)
            {
                course.CourseDates = await GetCourseDates(latestActiveStandard, courseType);
            }

            return course;
        }

        private async Task<Course> GetLatestActiveCourseByIfateReferenceNumber(string iFateReferenceNumber, CourseType? courseType)
        {
            var latestActiveStandard = await _standardsRepository.GetLatestActiveStandardByIfateReferenceNumber(iFateReferenceNumber, courseType);
            var course = await CourseWithRelatedOccupations(latestActiveStandard);

            if (course != null && course.CourseDates == null)
            {
                course.CourseDates = await GetCourseDates(latestActiveStandard, courseType);
            }

            return course;
        }

        public async Task<Course> GetCourse(string standardUId, CourseType? courseType)
        {
            var standard = await _standardsRepository.Get(standardUId, courseType);
            var course = await CourseWithRelatedOccupations(standard);

            if (course != null && course.CourseDates == null)
            {
                course.CourseDates = await GetCourseDates(standard.LarsCode, courseType);
            }

            return course;
        }

        private async Task<CourseDates> GetCourseDates(string larsCode, CourseType? courseType)
        {
            var latestActiveStandard = await _standardsRepository.GetLatestActiveStandard(larsCode, courseType);
            return await GetCourseDates(latestActiveStandard, courseType);
        }

        private async Task<CourseDates> GetCourseDates(Domain.Entities.Standard latestActiveStandard, CourseType? courseType)
        {
            var earliestActiveStandard = await _standardsRepository.GetEarliestActiveStandard(latestActiveStandard.LarsCode, courseType);
            return new CourseDates
            {
                EffectiveFrom = earliestActiveStandard.ApprovedForDelivery.GetValueOrDefault(DateTime.MinValue),
                EffectiveTo = latestActiveStandard.VersionLatestStartDate,
                LastDateStarts = latestActiveStandard.VersionLatestStartDate
            };
        }


        private async Task<Course> CourseWithRelatedOccupations(Domain.Entities.Standard standard)
        {
            var course = (Course)standard;
            if (course != null)
            {
                course.RelatedOccupations = await GetRelatedOccupations(standard, null);
            }

            return course;
        }

        private async Task<Standard> StandardWithRelatedOccupations(Domain.Entities.Standard standardEntity)
        {
            var standard = (Standard)standardEntity;
            if (standard != null)
            {
                standard.RelatedOccupations = await GetRelatedOccupations(standardEntity, CourseType.Apprenticeship);
            }

            return standard;
        }

        private async Task<List<RelatedOccupation>> GetRelatedOccupations(Domain.Entities.Standard standard, CourseType? courseType)
        {
            if (standard != null && standard.ApprenticeshipType == ApprenticeshipType.FoundationApprenticeship)
            {
                var standards = await _standardsRepository.GetActiveStandardsByIfateReferenceNumbers(standard.RelatedOccupations, courseType);
                return standards.ConvertAll(s => (RelatedOccupation)s);
            }
            return [];
        }

        private static bool IsStandardReference(string id)
            => IdentifierRegexes.StandardReferenceNumber.IsMatch(id) ||
               IdentifierRegexes.FoundationReferenceNumber.IsMatch(id) ||
               IdentifierRegexes.ShortCourseReferenceNumber.IsMatch(id);

        private static bool IsLarsCode(string id)
            => int.TryParse(id, out _)
            || IdentifierRegexes.ShortCourseLarsCode.IsMatch(id);

        private IEnumerable<Domain.Entities.Standard> FindByKeyword(IEnumerable<Domain.Entities.Standard> standards, string keyword)
        {
            var queryResult = _searchManager.Query(keyword);

            var tempStandards = standards
                .Join(queryResult.Standards,
                    standard => standard.StandardUId,
                    searchStandard => searchStandard.StandardUId,
                    (standard, searchStandard) => new { standard, searchStandard })
                .ToList();

            foreach (var tempStandard in tempStandards)
            {
                tempStandard.standard.SearchScore = tempStandard.searchStandard.Score;
            }

            standards = tempStandards
                .Select(arg => arg.standard);

            return standards;
        }
    }
}
