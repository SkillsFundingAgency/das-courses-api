using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenGettingStandardsList
    {
        [Test, RecursiveMoqAutoData]
        public async Task And_No_Keyword_Then_Gets_Standards_From_Repository(
            List<Standard> standardsFromRepo,
            OrderBy orderBy,
            StandardFilter filter,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(new List<int>(), new List<int>(), filter, false))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            var result = (await service.GetStandardsList("", new List<int>(), new List<int>(), orderBy, filter, false)).ToList();

            result.Should().BeEquivalentTo(standardsFromRepo, StandardEquivalencyAssertionOptions.ExcludingFields);

            foreach (var standard in result)
            {
                standard.Route.Should().Be(standardsFromRepo.Single(c => c.LarsCode.Equals(standard.LarsCode)).Route.Name);
            }
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_No_Keyword_And_Filtering_By_Active_Then_Gets_Standards_From_Repository(
            List<Standard> standardsFromRepo,
            OrderBy orderBy,
            StandardFilter filter,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            filter = StandardFilter.Active;
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(new List<int>(), new List<int>(), filter, false))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            var result = (await service.GetStandardsList("", new List<int>(), new List<int>(), orderBy, filter, false)).ToList();

            result.Should().BeEquivalentTo(standardsFromRepo, StandardEquivalencyAssertionOptions.ExcludingFields);

            foreach (var standard in result)
            {
                standard.Route.Should().Be(standardsFromRepo.Single(c => c.LarsCode.Equals(standard.LarsCode)).Route.Name);
            }
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Keyword_Then_Gets_Standards_From_SearchManager(
            string keyword,
            OrderBy orderBy,
            StandardFilter filter,
            List<Standard> standardsFromRepo,
            StandardSearchResultsList searchResult,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<ISearchManager> mockSearchManager,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            searchResult.Standards = new List<StandardSearchResult>
            {
                new StandardSearchResult{StandardUId = standardsFromRepo[0].StandardUId}
            };
            var standardsFoundInSearch = standardsFromRepo
                .Where(standard => searchResult.Standards.Select(result => result.StandardUId).Contains(standard.StandardUId))
                .ToList();
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(new List<int>(), new List<int>(), filter, false))
                .ReturnsAsync(standardsFromRepo);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFoundInSearch, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFoundInSearch.OrderBy(standard => standard.SearchScore));

            var standards = await service.GetStandardsList(keyword, new List<int>(), new List<int>(), orderBy, filter, false);

            standards.Should().BeEquivalentTo(standardsFoundInSearch, StandardEquivalencyAssertionOptions.ExcludingFields);
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Keyword_And_Sectors_Then_Gets_Standards_From_SearchManager_And_Filters(
            string keyword,
            OrderBy orderBy,
            StandardFilter filter,
            List<int> routeIds,
            List<Standard> standardsFromRepo,
            StandardSearchResultsList searchResult,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<ISearchManager> mockSearchManager,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            searchResult.Standards = new List<StandardSearchResult>
            {
                new StandardSearchResult{StandardUId = standardsFromRepo[0].StandardUId}
            };
            var standardsFoundInSearch = standardsFromRepo
                .Where(standard => searchResult.Standards.Select(result => result.StandardUId).Contains(standard.StandardUId))
                .ToList();
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(routeIds, new List<int>(), filter,true))
                .ReturnsAsync(standardsFromRepo);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFoundInSearch, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFoundInSearch.OrderBy(standard => standard.SearchScore));

            var getStandardsListResult = await service.GetStandardsList(keyword, routeIds, new List<int>(), orderBy, filter,true);

            getStandardsListResult.Should().BeEquivalentTo(standardsFoundInSearch, StandardEquivalencyAssertionOptions.ExcludingFields);
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Keyword_And_Sectors_And_Filter_Active_Then_Gets_Standards_From_SearchManager_And_Filters(
            string keyword,
            OrderBy orderBy,
            StandardFilter filter,
            List<int> routeIds,
            List<Standard> standardsFromRepo,
            StandardSearchResultsList searchResult,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<ISearchManager> mockSearchManager,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            searchResult.Standards = new List<StandardSearchResult>
            {
                new StandardSearchResult{StandardUId = standardsFromRepo[0].StandardUId}
            };
            filter = StandardFilter.Active;
            var standardsFoundInSearch = standardsFromRepo
                .Where(standard => searchResult.Standards.Select(result => result.StandardUId).Contains(standard.StandardUId))
                .ToList();
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(routeIds, new List<int>(), filter, false))
                .ReturnsAsync(standardsFromRepo);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFoundInSearch, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFoundInSearch.OrderBy(standard => standard.SearchScore));

            var getStandardsListResult = await service.GetStandardsList(keyword, routeIds, new List<int>(), orderBy, filter, false);

            getStandardsListResult.Should().BeEquivalentTo(standardsFoundInSearch, StandardEquivalencyAssertionOptions.ExcludingFields);
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Levels_Then_Gets_Standards_From_Filters(
            List<int> levelCodes,
            OrderBy orderBy,
            StandardFilter filter,
            List<Standard> standardsFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(new List<int>(), levelCodes, filter, true))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            var getStandardsListResult = await service.GetStandardsList("", new List<int>(), levelCodes, orderBy, filter, true);

            getStandardsListResult.Should().BeEquivalentTo(standardsFromRepo, StandardEquivalencyAssertionOptions.ExcludingFields);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Standards_Are_Ordered_By_SortOrderService(
            List<int> levelCodes,
            OrderBy orderBy,
            StandardFilter filter,
            List<Standard> standardsFromRepo,
            List<Standard> standardsFromSortService,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetStandards(new List<int>(), levelCodes, filter, false))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, orderBy, ""))
                .Returns(standardsFromSortService.OrderBy(standard => standard.SearchScore));

            var getStandardsListResult = await service.GetStandardsList("", new List<int>(), levelCodes, orderBy, filter, false);

            getStandardsListResult
                .Should().BeEquivalentTo(standardsFromSortService.OrderBy(standard => standard.SearchScore), StandardEquivalencyAssertionOptions.ExcludingFieldsWithStrictOrdering);
        }
    }
}
