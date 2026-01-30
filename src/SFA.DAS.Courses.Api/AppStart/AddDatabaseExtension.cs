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
                services.AddDbContext<CoursesDataContext>(options => options.UseInMemoryDatabase("SFA.DAS.Courses"), ServiceLifetime.Scoped);
            }
            else if (environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddDbContext<CoursesDataContext>(options=>options.UseSqlServer(config.ConnectionString),ServiceLifetime.Scoped);
            }
            else
            {
                services.AddSingleton(new AzureServiceTokenProvider());
                services.AddDbContext<CoursesDataContext>(ServiceLifetime.Scoped);    
            }
            
            

            services.AddScoped<ICoursesDataContext, CoursesDataContext>(provider => provider.GetService<CoursesDataContext>());
            services.AddScoped(provider => new Lazy<CoursesDataContext>(provider.GetService<CoursesDataContext>()));
            
        }
    }
}
