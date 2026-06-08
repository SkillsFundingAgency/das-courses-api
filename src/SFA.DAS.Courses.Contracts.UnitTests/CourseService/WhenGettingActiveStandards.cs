using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Courses.Contracts.ApiRequests;
using SFA.DAS.Courses.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Contracts.UnitTests.CourseService;

public class WhenGettingActiveStandards
{
    [Test, MoqAutoData]
    public async Task Then_If_The_Standards_Are_Not_Cached_Then_The_Api_Endpoint_Is_Called_And_Results_Cached(
        TestGetStandardsListResponse coursesFromApi,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
        [Frozen] Mock<ICacheStorageService> mockCacheService,
        Contracts.CourseService service)
    {
        mockCacheService.Setup(x => x.RetrieveFromCache<TestGetStandardsListResponse>(nameof(TestGetStandardsListResponse)))
            .ReturnsAsync((TestGetStandardsListResponse)null);
        mockCoursesApiClient.Setup(x => x.Get<TestGetStandardsListResponse>(It.IsAny<GetCoursesSearchApiRequest>()))
            .ReturnsAsync(coursesFromApi);

        var actual = await service.GetActiveStandards<TestGetStandardsListResponse>(nameof(TestGetStandardsListResponse));
        
        actual.Should().BeEquivalentTo(coursesFromApi);
        mockCacheService.Verify(x=>x.SaveToCache(nameof(TestGetStandardsListResponse), coursesFromApi, 4, null), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Standards_Are_Cached_Then_Returned_And_Api_Not_Called(
        TestGetStandardsListResponse coursesFromCache,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
        [Frozen] Mock<ICacheStorageService> mockCacheService,
        Contracts.CourseService service)
    {
        mockCacheService.Setup(x => x.RetrieveFromCache<TestGetStandardsListResponse>(nameof(TestGetStandardsListResponse)))
            .ReturnsAsync(coursesFromCache);

        var actual = await service.GetActiveStandards<TestGetStandardsListResponse>(nameof(TestGetStandardsListResponse));

        actual.Should().BeEquivalentTo(coursesFromCache);
        mockCoursesApiClient.Verify(x=>x.Get<TestGetStandardsListResponse>(It.IsAny<GetCoursesSearchApiRequest>()), Times.Never);
    }

    public class TestGetStandardsListResponse
    {
        public int Id { get; set; }
        public int Title { get; set; }
    }
}
