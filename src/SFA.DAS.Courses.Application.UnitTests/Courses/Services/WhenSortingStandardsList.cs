using NUnit.Framework;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenSortingStandardsList
    {
        [Test, Ignore("todo")]
        public void And_OrderBy_Score_Then_Orders_By_Score_Then_Title_Then_Level()
        {
            
        }

        [Test, Ignore("todo")]
        public void And_OrderBy_Title_Then_Orders_By_Title_Then_Level()
        {

        }

        // tests previously in StandardsService:

        /*[Test, RecursiveMoqAutoData]
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
        }*/
    }
}
