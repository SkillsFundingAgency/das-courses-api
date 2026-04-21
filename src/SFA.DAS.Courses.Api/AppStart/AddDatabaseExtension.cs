using System;
using Azure.Core;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Configuration;

namespace SFA.DAS.Courses.Api.AppStart
{
    public static class AddDatabaseExtension
    {
        public static void AddDatabaseRegistration(this IServiceCollection services, CoursesConfiguration config, string environmentName)
        {
            if (environmentName.Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddDbContext<CoursesDataContext>(options => options.UseInMemoryDatabase("SFA.DAS.Courses"), ServiceLifetime.Transient);
            }
            else if (environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddDbContext<CoursesDataContext>(options=>options.UseSqlServer(config.ConnectionString),ServiceLifetime.Transient);
            }
            else
            {
                services.AddSingleton<TokenCredential, DefaultAzureCredential>();

                services.AddDbContext<CoursesDataContext>(ServiceLifetime.Transient, ServiceLifetime.Transient);
            }

            services.AddTransient<ICoursesDataContext>(provider => provider.GetRequiredService<CoursesDataContext>());
            services.AddTransient(provider => new Lazy<CoursesDataContext>(provider.GetRequiredService<CoursesDataContext>()));
        }
    }
}
