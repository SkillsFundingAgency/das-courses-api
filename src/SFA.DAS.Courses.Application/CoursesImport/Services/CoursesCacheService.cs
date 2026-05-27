using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using SFA.DAS.Courses.Application.Infrastructure;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Services
{
    public class CoursesCacheService : ICoursesCacheService
    {
        private readonly IOutputCacheStore _outputCacheStore;
        private readonly ILogger<CoursesCacheService> _logger;

        public CoursesCacheService(
            IOutputCacheStore outputCacheStore,
            ILogger<CoursesCacheService> logger)
        {
            _outputCacheStore = outputCacheStore;
            _logger = logger;
        }

        public async Task ClearCoursesCache(string reason, CancellationToken cancellationToken)
        {
            await _outputCacheStore.EvictByTagAsync(
                CoursesOutputCachePolicy.CoursesTag,
                cancellationToken);

            _logger.LogInformation("Courses cache cleared {Reason}", reason);
        }
    }
}
