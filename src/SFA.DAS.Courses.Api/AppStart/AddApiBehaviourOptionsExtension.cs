using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.Courses.Api.AppStart
{
    public static class AddApiBehaviourOptionsExtension
    {
        public static void AddApiBehaviourOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(kvp => kvp.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key, 
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                    var problem = new ValidationProblemDetails(errors)
                    {
                        Title = "Invalid request parameters",
                        Status = StatusCodes.Status400BadRequest,
                        Type = "https://httpstatuses.com/400"
                    };

                    return new BadRequestObjectResult(problem)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });
        }
    }
}
