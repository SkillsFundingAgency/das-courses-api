using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;

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
            bool isExport)
        {
            var standards = await _standardsRepository.GetStandards(routeIds, levels, filter, isExport);

            if (!string.IsNullOrEmpty(keyword))
            {
                standards = FindByKeyword(standards, keyword);
            }

            standards = _sortOrderService.OrderBy(standards, orderBy, keyword);

            return standards.Select(standard => (Standard)standard);
        }

        public async Task<IEnumerable<Standard>> GetAllVersionsOfAStandard(string iFateReferenceNumber)
        {
            var standards = await _standardsRepository.GetStandards(iFateReferenceNumber);

            return standards.Select(standard => (Standard)standard);
        }

        public async Task<int> Count(StandardFilter filter = StandardFilter.None)
        {
            var count = await _standardsRepository.Count(filter);
            return count;
        }

        public async Task<Standard> GetLatestActiveStandard(int larsCode)
        {
            var standard = await _standardsRepository.GetLatestActiveStandard(larsCode);

            return (Standard)standard;
        }

        public async Task<Standard> GetLatestActiveStandard(string ifateReferenceNumber)
        {
            var standard = await _standardsRepository.GetLatestActiveStandard(ifateReferenceNumber);

            return (Standard)standard;
        }

        public async Task<Standard> GetStandard(string standardUId)
        {
            var standard = await _standardsRepository.Get(standardUId);

            return (Standard)standard;
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
