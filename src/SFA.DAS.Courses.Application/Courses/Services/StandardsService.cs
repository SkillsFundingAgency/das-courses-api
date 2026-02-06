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
            ApprenticeshipType? apprenticeshipType,
            CourseType? courseType)
        {
            var standards = await _standardsRepository.GetStandards(routeIds, levels, filter, includeAllProperties, apprenticeshipType, courseType);

            if (!string.IsNullOrEmpty(keyword))
            {
                standards = FindByKeyword(standards, keyword);
            }

            standards = _sortOrderService.OrderBy(standards, orderBy, keyword);

            return standards.Select(standard => (Standard)standard);
        }

        public async Task<IEnumerable<Standard>> GetAllVersionsOfAStandard(string iFateReferenceNumber, CourseType? courseType)
        {
            var standards = await _standardsRepository.GetStandards(iFateReferenceNumber, courseType);

            return standards.Select(standard => (Standard)standard);
        }

        public async Task<int> Count(StandardFilter filter = StandardFilter.None, CourseType? courseType = null)
        {
            var count = await _standardsRepository.Count(filter, courseType);
            return count;
        }

        public async Task<Standard> GetLatestActiveStandard(string larsCode, CourseType? courseType)
        {
            var standard = await _standardsRepository.GetLatestActiveStandard(larsCode, courseType);

            var result = (Standard)standard;
            if (result != null)
            {
                result.RelatedOccupations = await GetRelatedOccupations(standard, courseType);
            }
            return result;
        }

        public async Task<Standard> GetLatestActiveStandardByIfateReferenceNumber(string iFateReferenceNumber, CourseType? courseType)
        {
            var standard = await _standardsRepository.GetLatestActiveStandardByIfateReferenceNumber(iFateReferenceNumber, courseType);

            var result = (Standard)standard;
            if (result != null)
            {
                result.RelatedOccupations = await GetRelatedOccupations(standard, courseType);
            }
            return result;
        }

        public async Task<Standard> GetStandard(string standardUId, CourseType? courseType)
        {
            var standard = await _standardsRepository.Get(standardUId, courseType);

            var result = (Standard)standard;
            if (result != null)
            {
                result.RelatedOccupations = await GetRelatedOccupations(standard, courseType);
            }
            return result;
        }

        public async Task<Standard> GetStandardByAnyId(string id, CourseType? courseType)
        {
            if (IsLarsCode(id))
            {
                return await GetLatestActiveStandard(id, courseType);
            }
            else if (IsStandardReference(id))
            {
                return await GetLatestActiveStandardByIfateReferenceNumber(id, courseType);
            }
            else
            {
                return await GetStandard(id, courseType);
            }
        }

        private static bool IsStandardReference(string id)
            => IdentifierRegexes.StandardReferenceNumber.IsMatch(id) ||
               IdentifierRegexes.FoundationReferenceNumber.IsMatch(id) ||
               IdentifierRegexes.ShortCourseReferenceNumber.IsMatch(id);

        private static bool IsLarsCode(string id)
            => int.TryParse(id, out _)
            || IdentifierRegexes.ShortCourseLarsCode.IsMatch(id);
    
        private async Task<List<RelatedOccupation>> GetRelatedOccupations(Domain.Entities.Standard standard, CourseType? courseType)
        {
            if (standard != null && standard.ApprenticeshipType == ApprenticeshipType.FoundationApprenticeship)
            {
                var standards = await _standardsRepository.GetActiveStandardsByIfateReferenceNumbers(standard.RelatedOccupations, courseType);
                return standards.ConvertAll(s => (RelatedOccupation)s);
            }
            return [];
        }

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
