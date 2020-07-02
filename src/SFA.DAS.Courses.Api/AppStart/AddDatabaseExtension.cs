using System;
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
                services.AddDbContext<CoursesDataContext>(options => options.UseInMemoryDatabase("SFA.DAS.Courses"));
            }
            else
            {
                services.AddDbContext<CoursesDataContext>(options => options.UseSqlServer(config.ConnectionString), ServiceLifetime.Transient);
            }

            services.AddTransient<ICoursesDataContext, CoursesDataContext>(provider => provider.GetService<CoursesDataContext>());
            services.AddTransient(provider => new Lazy<CoursesDataContext>(provider.GetService<CoursesDataContext>()));
            
        }
    }
}