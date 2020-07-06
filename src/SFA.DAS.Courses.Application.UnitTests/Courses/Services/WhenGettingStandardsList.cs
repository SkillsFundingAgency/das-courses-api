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
            int count,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetAll())
                .ReturnsAsync(standardsFromRepo);
            mockStandardsRepository
                .Setup(repository => repository.Count())
                .ReturnsAsync(count);

            var result = (await service.GetStandardsList("", new List<Guid>(), new List<int>())).ToList();

            result.Should().BeEquivalentTo(standardsFromRepo, 
                config => config
                    .Excluding(standard => standard.LarsStandard)
                    .Excluding(standard => standard.ApprenticeshipFunding)
                    .Excluding(standard => standard.SearchScore)
                    .Excluding(standard => standard.Sector)
                    .Excluding(standard => standard.RouteId)
                );
            
            foreach (var standard in result)
            {
                standard.Route.Should().Be(standardsFromRepo.Single(c => c.Id.Equals(standard.Id)).Sector.Route);
            }
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Keyword_Then_Gets_Standards_From_SearchManager(
            string keyword,
            List<Standard> standardsFromRepo,
            StandardSearchResultsList searchResult,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<ISearchManager> mockSearchManager,
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
                .Setup(repository => repository.GetAll())
                .ReturnsAsync(standardsFromRepo);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);

            var standards = await service.GetStandardsList(keyword, new List<Guid>(), new List<int>());

            standards.Should().BeEquivalentTo(standardsFoundInSearch,
                config => config
                    .Excluding(standard => standard.LarsStandard)
                    .Excluding(standard => standard.ApprenticeshipFunding)
                    .Excluding(standard => standard.SearchScore)
                    .Excluding(standard => standard.Sector)
                    .Excluding(standard => standard.RouteId));
        }
        
        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Keyword_And_Sectors_Then_Gets_Standards_From_SearchManager_And_Filters(
            string keyword,
            List<Guid> routeIds,
            List<Standard> standardsFromRepo,
            StandardSearchResultsList searchResult,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<ISearchManager> mockSearchManager,
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

            var getStandardsListResult = await service.GetStandardsList(keyword, routeIds, new List<int>());

            getStandardsListResult.Should().BeEquivalentTo(standardsFoundInSearch,
                config => config
                    .Excluding(standard => standard.SearchScore)
                    .Excluding(standard => standard.ApprenticeshipFunding)
                    .Excluding(standard => standard.LarsStandard)
                    .Excluding(standard => standard.Sector)
                    .Excluding(standard => standard.RouteId));
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Has_Levels_Then_Gets_Standards_From_Filters(
            int count,
            List<int> levelCodes,
            List<Standard> standardsFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetFilteredStandards(new List<Guid>(), levelCodes))
                .ReturnsAsync(standardsFromRepo);
            mockStandardsRepository
                .Setup(repository => repository.Count())
                .ReturnsAsync(count);


            var getStandardsListResult = await service.GetStandardsList("", new List<Guid>(), levelCodes);

            getStandardsListResult.Should().BeEquivalentTo(standardsFromRepo,
                config => config
                    .Excluding(standard => standard.SearchScore)
                    .Excluding(standard => standard.ApprenticeshipFunding)
                    .Excluding(standard => standard.LarsStandard)
                    .Excluding(standard => standard.Sector)
                    .Excluding(standard => standard.RouteId));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Standards_Are_Ordered_By_Title_Then_By_Level(
            int count,
            List<int> levelCodes,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            var standardsFromRepo = new List<Standard>
            {
                new Standard{Title = "zz top", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Title = "aardvark", Level = 2, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Title = "in between", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Title = "zz top", Level = 1, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Title = "in between", Level = 5, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()}
            };
            mockStandardsRepository
                .Setup(repository => repository.GetFilteredStandards(new List<Guid>(), levelCodes))
                .ReturnsAsync(standardsFromRepo);
            mockStandardsRepository
                .Setup(repository => repository.Count())
                .ReturnsAsync(count);

            var getStandardsListResult = await service.GetStandardsList("", new List<Guid>(), levelCodes);
            
            getStandardsListResult.Should().BeEquivalentTo(standardsFromRepo
                    .OrderBy(standard => standard.Title)
                    .ThenBy(standard => standard.Level), 
                config => config
                    .Including(standard => standard.Title)
                    .Including(standard => standard.Level)
                    .WithStrictOrdering());
        }

        [Test, RecursiveMoqAutoData]
        public async Task And_Keyword_Then_Standards_Are_Ordered_By_Score_Then_Title_Then_Level(
            string keyword,
            StandardSearchResultsList searchResult,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            [Frozen] Mock<ISearchManager> mockSearchManager,
            StandardsService service)
        {
            var standardsFromRepo = new List<Standard>
            {
                new Standard{Id = 1, SearchScore = 2f, Title = "zz top", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 2, SearchScore = 1f, Title = "aardvark", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 3, SearchScore = 1f, Title = "aardvark", Level = 2, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 4, SearchScore = 4f, Title = "in between", Level = 3, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 5, SearchScore = 2f, Title = "zz top", Level = 1, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()},
                new Standard{Id = 6, SearchScore = 4f, Title = "in between", Level = 5, Sector = new Sector(), ApprenticeshipFunding = new List<ApprenticeshipFunding>(), LarsStandard = new LarsStandard()}
            };
            searchResult.Standards = standardsFromRepo.Select(standard => new StandardSearchResult
            {
                Id = standard.Id, 
                Score = standard.SearchScore.GetValueOrDefault()
            });
            mockStandardsRepository
                .Setup(repository => repository.GetAll())
                .ReturnsAsync(standardsFromRepo);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);

            var getStandardsListResult = await service.GetStandardsList(keyword, new List<Guid>(), new List<int>());
            
            getStandardsListResult.Should().BeEquivalentTo(standardsFromRepo
                    .OrderByDescending(standard => standard.SearchScore)
                    .ThenBy(standard => standard.Title)
                    .ThenBy(standard => standard.Level), 
                config => config
                    .Including(standard => standard.Title)
                    .Including(standard => standard.Level)
                    .WithStrictOrdering());
        }
    }
}
