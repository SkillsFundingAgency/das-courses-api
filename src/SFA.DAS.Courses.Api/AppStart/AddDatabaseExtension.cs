using System;
using Microsoft.Azure.Services.AppAuthentication;
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
                services.AddDbContext<CoursesDataContext>(
                    options => options.UseInMemoryDatabase("SFA.DAS.Courses"),
                    ServiceLifetime.Scoped);
            }
            else
            {
                services.AddSingleton(new AzureServiceTokenProvider());
                services.AddScoped<AzureSqlAccessTokenInterceptor>();

                services.AddDbContext<CoursesDataContext>((sp, options) =>
                {
                    options.UseLazyLoadingProxies();

                    options.UseSqlServer(
                        config.ConnectionString,
                        sql => sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(20), null));

                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                    if (!environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
                    {
                        options.AddInterceptors(sp.GetRequiredService<AzureSqlAccessTokenInterceptor>());
                    }
                }, ServiceLifetime.Scoped);
            }

            services.AddScoped<ICoursesDataContext>(sp => sp.GetRequiredService<CoursesDataContext>());
            services.AddScoped(sp => new Lazy<CoursesDataContext>(sp.GetRequiredService<CoursesDataContext>()));
        }
    }
}
