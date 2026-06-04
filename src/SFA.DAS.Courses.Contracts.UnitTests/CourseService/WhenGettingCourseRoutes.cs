using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Courses.Contracts.ApiRequests;
using SFA.DAS.Courses.Contracts.ApiResponses;
using SFA.DAS.Courses.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Contracts.UnitTests.CourseService;

public class WhenGettingCourseRoutes
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_Sectors_Returned_And_Added_To_Cache(
        GetRoutesListResponse apiResponse,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        Contracts.CourseService service)
    {
        //Arrange
        cacheStorageService.Setup(x => x.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
            .ReturnsAsync((GetRoutesListResponse)default);
        apiClient.Setup(x => x.Get<GetRoutesListResponse>(It.IsAny<GetCoursesRoutesApiRequest>())).ReturnsAsync(apiResponse);

        //Act
        var actual = await service.GetRoutes();

        //Assert
        actual.Should().BeEquivalentTo(apiResponse);
        cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetRoutesListResponse), apiResponse, 23, null));
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_Sectors_Are_In_The_Cache_The_Api_Is_Not_Called(
        GetRoutesListResponse apiResponse,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        Contracts.CourseService service)
    {
        //Arrange
        cacheStorageService.Setup(x => x.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
            .ReturnsAsync(apiResponse);
        
        //Act
        var actual = await service.GetRoutes();

        //Assert
        actual.Should().BeEquivalentTo(apiResponse);
        apiClient.Verify(x => x.Get<GetRoutesListResponse>(It.IsAny<GetCoursesRoutesApiRequest>()), Times.Never);
    }
        
}
