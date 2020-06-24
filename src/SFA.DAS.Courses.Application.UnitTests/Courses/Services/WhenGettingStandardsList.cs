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
        [Test, MoqAutoData]
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

            var result = await service.GetStandardsList(null);

            result.Standards.Should().BeEquivalentTo(standardsFromRepo, 
                config => config
                .Excluding(standard => standard.SearchScore)
                .Excluding(standard => standard.Sector)
                .Excluding(standard => standard.RouteId)
                );
            result.Total.Should().Be(count);
            result.TotalFiltered.Should().Be(standardsFromRepo.Count);
            
            foreach (var standard in result.Standards)
            {
                standard.Route.Should().Be(standardsFromRepo.Single(c => c.Id.Equals(standard.Id)).Sector.Route);
            }
        }

        [Test, MoqAutoData]
        public async Task And_Has_Keyword_Then_Gets_Standards_From_SearchManager(
            string keyword,
            int count,
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
            mockStandardsRepository
                .Setup(repository => repository.Count())
                .ReturnsAsync(count);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);

            var getStandardsListResult = (await service.GetStandardsList(keyword));

            getStandardsListResult.Standards.Should().BeEquivalentTo(standardsFoundInSearch,
                config => config.Excluding(standard => standard.SearchScore));
            getStandardsListResult.Total.Should().Be(count);
            getStandardsListResult.TotalFiltered.Should().Be(standardsFoundInSearch.Count);
        }
    }
}
