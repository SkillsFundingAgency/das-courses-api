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
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetAll(true))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            var result = (await service.GetStandardsList("", new List<Guid>(), new List<int>(), orderBy)).ToList();

            result.Should().BeEquivalentTo(standardsFromRepo, 
                config => config
                    .Excluding(standard => standard.LarsStandard)
                    .Excluding(standard => standard.ApprenticeshipFunding)
                    .Excluding(standard => standard.SearchScore)
                    .Excluding(standard => standard.Sector)
                    .Excluding(standard => standard.RouteId)
                    .Excluding(standard => standard.RegulatedBody)
                    .Excluding(standard => standard.CoreDuties)
                );
            
            foreach (var standard in result)
            {
                standard.Route.Should().Be(standardsFromRepo.Single(c => c.Id.Equals(standard.Id)).Sector.Route);
            }
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_No_Keyword_And_Not_Filtering_By_Available_To_Start_Then_Gets_All_From_Repository(
            List<Standard> standardsFromRepo,
            OrderBy orderBy,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetAll(false))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            var result = (await service.GetStandardsList("", new List<Guid>(), new List<int>(), orderBy, false)).ToList();

            result.Should().BeEquivalentTo(standardsFromRepo, 
                config => config
                    .Excluding(standard => standard.LarsStandard)
                    .Excluding(standard => standard.ApprenticeshipFunding)
                    .Excluding(standard => standard.SearchScore)
                    .Excluding(standard => standard.Sector)
                    .Excluding(standard => standard.RouteId)
                    .Excluding(standard => standard.RegulatedBody)
                    .Excluding(standard => standard.CoreDuties)
            );
            
            foreach (var standard in result)
            {
                standard.Route.Should().Be(standardsFromRepo.Single(c => c.Id.Equals(standard.Id)).Sector.Route);
            }
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Keyword_Then_Gets_Standards_From_SearchManager(
            string keyword,
            OrderBy orderBy,
            List<Standard> standardsFromRepo,
            StandardSearchResultsList searchResult,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<ISearchManager> mockSearchManager,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            searchResult.Standards = new List<StandardSearchResult>
            {
                new StandardSearchResult{Id = standardsFromRepo[0].Id}
            };
            var standardsFoundInSearch = standardsFromRepo
                .Where(standard => searchResult.Standards.Select(result => result.Id).Contains(standard.Id))
                .ToList();
            mockStandardsRepository
                .Setup(repository => repository.GetAll(true))
                .ReturnsAsync(standardsFromRepo);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFoundInSearch, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFoundInSearch.OrderBy(standard => standard.SearchScore));

            var standards = await service.GetStandardsList(keyword, new List<Guid>(), new List<int>(), orderBy);

            standards.Should().BeEquivalentTo(standardsFoundInSearch,
                config => config
                    .Excluding(standard => standard.LarsStandard)
                    .Excluding(standard => standard.ApprenticeshipFunding)
                    .Excluding(standard => standard.SearchScore)
                    .Excluding(standard => standard.Sector)
                    .Excluding(standard => standard.RouteId)
                    .Excluding(standard => standard.RegulatedBody)
                    .Excluding(standard => standard.CoreDuties));
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Keyword_And_Sectors_Then_Gets_Standards_From_SearchManager_And_Filters(
            string keyword,
            OrderBy orderBy,
            List<Guid> routeIds,
            List<Standard> standardsFromRepo,
            StandardSearchResultsList searchResult,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<ISearchManager> mockSearchManager,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            searchResult.Standards = new List<StandardSearchResult>
            {
                new StandardSearchResult{Id = standardsFromRepo[0].Id}
            };
            var standardsFoundInSearch = standardsFromRepo
                .Where(standard => searchResult.Standards.Select(result => result.Id).Contains(standard.Id))
                .ToList();
            mockStandardsRepository
                .Setup(repository => repository.GetFilteredStandards(routeIds, new List<int>()))
                .ReturnsAsync(standardsFromRepo);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFoundInSearch, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFoundInSearch.OrderBy(standard => standard.SearchScore));

            var getStandardsListResult = await service.GetStandardsList(keyword, routeIds, new List<int>(), orderBy);

            getStandardsListResult.Should().BeEquivalentTo(standardsFoundInSearch,
                config => config
                    .Excluding(standard => standard.SearchScore)
                    .Excluding(standard => standard.ApprenticeshipFunding)
                    .Excluding(standard => standard.LarsStandard)
                    .Excluding(standard => standard.Sector)
                    .Excluding(standard => standard.RouteId)
                    .Excluding(standard => standard.RegulatedBody)
                    .Excluding(standard => standard.CoreDuties));
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Levels_Then_Gets_Standards_From_Filters(
            List<int> levelCodes,
            OrderBy orderBy,
            List<Standard> standardsFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetFilteredStandards(new List<Guid>(), levelCodes))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, It.IsAny<OrderBy>(), It.IsAny<string>()))
                .Returns(standardsFromRepo.OrderBy(standard => standard.SearchScore));

            var getStandardsListResult = await service.GetStandardsList("", new List<Guid>(), levelCodes, orderBy);

            getStandardsListResult.Should().BeEquivalentTo(standardsFromRepo,
                config => config
                    .Excluding(standard => standard.SearchScore)
                    .Excluding(standard => standard.ApprenticeshipFunding)
                    .Excluding(standard => standard.LarsStandard)
                    .Excluding(standard => standard.Sector)
                    .Excluding(standard => standard.RouteId)
                    .Excluding(standard => standard.RegulatedBody)
                    .Excluding(standard => standard.CoreDuties));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Standards_Are_Ordered_By_SortOrderService(
            List<int> levelCodes,
            OrderBy orderBy,
            List<Standard> standardsFromRepo,
            List<Standard> standardsFromSortService,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<IStandardsSortOrderService> mockSortOrderService,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetFilteredStandards(new List<Guid>(), levelCodes))
                .ReturnsAsync(standardsFromRepo);
            mockSortOrderService
                .Setup(orderService => orderService.OrderBy(standardsFromRepo, orderBy, ""))
                .Returns(standardsFromSortService.OrderBy(standard => standard.SearchScore));

            var getStandardsListResult = await service.GetStandardsList("", new List<Guid>(), levelCodes, orderBy);
            
            getStandardsListResult
                .Should().BeEquivalentTo(standardsFromSortService.OrderBy(standard => standard.SearchScore),
            config => config
                .Excluding(standard => standard.SearchScore)
                .Excluding(standard => standard.ApprenticeshipFunding)
                .Excluding(standard => standard.LarsStandard)
                .Excluding(standard => standard.Sector)
                .Excluding(standard => standard.RouteId)
                .Excluding(standard => standard.RegulatedBody)
                .Excluding(standard => standard.CoreDuties)
                .WithStrictOrdering());
        }
    }
}
