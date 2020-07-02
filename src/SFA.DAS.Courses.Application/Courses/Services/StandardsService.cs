using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Services
{
    public class StandardsService : IStandardsService
    {
        private readonly IStandardRepository _standardsRepository;
        private readonly ISearchManager _searchManager;

        public StandardsService(
            IStandardRepository standardsRepository,
            ISearchManager searchManager)
        {
            _standardsRepository = standardsRepository;
            _searchManager = searchManager;
        }

        public async Task<IEnumerable<Standard>> GetStandardsList(string keyword, IList<Guid> routeIds)
        {
            var standards = routeIds != null && routeIds.Any()  ?
                await _standardsRepository.GetFilteredStandards(routeIds.ToList()) :
                await _standardsRepository.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                standards = FindByKeyword(standards, keyword);
            }

            return standards.Select(standard => (Standard)standard);
        }

        public async Task<int> Count()
        {
            var count = await _standardsRepository.Count();
            return count;
        }

        public async Task<Standard> GetStandard(int standardId)
        {
            var standard = await _standardsRepository.Get(standardId);

            return standard;
        }

        private IEnumerable<Domain.Entities.Standard> FindByKeyword(IEnumerable<Domain.Entities.Standard> standards, string keyword)
        {
            var queryResult = _searchManager.Query(keyword);

            var tempStandards = standards
                .Join(queryResult.Standards,
                    standard => standard.Id,
                    searchStandard => searchStandard.Id,
                    (standard, searchStandard) => new {standard, searchStandard})
                .ToList();

            foreach (var tempStandard in tempStandards)
            {
                tempStandard.standard.SearchScore = tempStandard.searchStandard.Score;
            }

            standards = tempStandards
                .Select(arg => arg.standard)
                .OrderByDescending(standard => standard.SearchScore);

            return standards;
        }
    }
}
