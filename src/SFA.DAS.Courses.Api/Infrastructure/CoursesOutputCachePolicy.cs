using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace SFA.DAS.Courses.Api.Infrastructure
{
    /// <summary>
    /// A policy which allows authorized endpoints to be cached, by default authorized endpoints
    /// in an API authorized by a Bearer token are not cached, this policy overrides that behaviour
    /// as Courses API will return the same data between DataLoad operations
    /// </summary>
    public sealed class CoursesOutputCachePolicy : IOutputCachePolicy
    {
        public const string CoursesDataLoad = nameof(CoursesDataLoad);
        public const string CoursesTag = nameof(CoursesTag);

        public CoursesOutputCachePolicy()
        {
        }

        public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            var request = context.HttpContext.Request;

            var canCache =
                HttpMethods.IsGet(request.Method) ||
                HttpMethods.IsHead(request.Method);

            context.EnableOutputCaching = true;
            context.AllowCacheLookup = canCache;
            context.AllowCacheStorage = canCache;
            context.AllowLocking = true;

            return ValueTask.CompletedTask;
        }

        public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
            => ValueTask.CompletedTask;

        public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            var response = context.HttpContext.Response;

            if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
            {
                context.AllowCacheStorage = false;
                return ValueTask.CompletedTask;
            }

            if (response.StatusCode != StatusCodes.Status200OK)
            {
                context.AllowCacheStorage = false;
            }

            return ValueTask.CompletedTask;
        }
    }
}
