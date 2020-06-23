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
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetAll())
                .ReturnsAsync(standardsFromRepo);

            var standards = (await service.GetStandardsList(null)).ToList();

            standards.Should().BeEquivalentTo(standardsFromRepo, 
                config => config.Excluding(standard => standard.SearchScore));
        }

        [Test, MoqAutoData]
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
                .Where(standard => searchResult.Standards.Select(result => result.Id).Contains(standard.Id));
            mockStandardsRepository
                .Setup(repository => repository.GetAll())
                .ReturnsAsync(standardsFromRepo);
            mockSearchManager
                .Setup(manager => manager.Query(keyword))
                .Returns(searchResult);

            var standards = (await service.GetStandardsList(keyword)).ToList();

            standards.Should().BeEquivalentTo(standardsFoundInSearch,
                config => config.Excluding(standard => standard.SearchScore));
        }
    }
}
