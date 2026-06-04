using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Courses.Contracts.ApiRequests;
using SFA.DAS.Courses.Contracts.ApiResponses;
using SFA.DAS.Courses.Contracts.Client;

namespace SFA.DAS.Courses.Contracts;
public interface ICourseService
{
    Task<GetRoutesListResponse> GetRoutes();
    Task<GetLevelsListResponse> GetLevels();
    Task<T> GetActiveStandards<T>(string cacheItemName);
}

public class CourseService : ICourseService
{
    private const int CourseCacheExpiryInHours = 4;
    private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
    private readonly ICacheStorageService _cacheStorageService;

    public CourseService (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ICacheStorageService cacheStorageService)
    {
        _coursesApiClient = coursesApiClient;
        _cacheStorageService = cacheStorageService;
    }
    
    public async Task<GetRoutesListResponse> GetRoutes()
    {
        var response = await _cacheStorageService.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse));
        if (response == null)
        {
            response = await _coursesApiClient.Get<GetRoutesListResponse>(new GetCoursesRoutesApiRequest());

            await _cacheStorageService.SaveToCache(nameof(GetRoutesListResponse), response, 23);
        }

        return response;
    }

    public async Task<GetLevelsListResponse> GetLevels()
    {
        var response = await _cacheStorageService.RetrieveFromCache<GetLevelsListResponse>(nameof(GetLevelsListResponse));
        if (response == null)
        {
            response = await _coursesApiClient.Get<GetLevelsListResponse>(new GetCoursesLevelsApiRequest());

            await _cacheStorageService.SaveToCache(nameof(GetLevelsListResponse), response, 23);
        }
        return response;
    }

    public async Task<T> GetActiveStandards<T>(string cacheItemName)
    {
        var cachedCourses =
            await _cacheStorageService.RetrieveFromCache<T>(
                cacheItemName);

        if (cachedCourses != null)
        {
            return cachedCourses;
        }

        var apiCourses = await _coursesApiClient.Get<T>(new GetCoursesSearchApiRequest(null, null, null, null, null, OrderBy.Score, StandardFilter.ActiveAvailable));

        await _cacheStorageService.SaveToCache(cacheItemName, apiCourses, CourseCacheExpiryInHours);

        return apiCourses;
    }
}
