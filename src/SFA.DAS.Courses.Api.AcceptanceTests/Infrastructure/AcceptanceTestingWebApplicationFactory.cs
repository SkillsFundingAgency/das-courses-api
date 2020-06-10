using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Api.AcceptanceTests.Infrastructure
{
    public class AcceptanceTestingWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<CoursesDataContext>(options =>
                {
                    options.UseInMemoryDatabase("testDbName");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CoursesDataContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<AcceptanceTestingWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        DbUtilities.LoadTestData(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }

    public static class DbUtilities
    {
        public static void LoadTestData(CoursesDataContext context)
        {
            context.Standards.AddRange(GetTestStandards());
            context.SaveChanges();
        }

        public static void ClearTestData(CoursesDataContext context)
        {
            context.Standards.RemoveRange(GetTestStandards());
            context.SaveChanges();
        }

        public static IEnumerable<Standard> GetTestStandards()
        {
            return new List<Standard>
            {
                new Standard{Id = 1, Title = "Head Brewer", Keywords = "Head, Brewer, Beer", OverviewOfRole = "Overseer of brewery operations", Level = 6, Route = "Engineering and manufacturing"},
                new Standard{Id = 2, Title = "Brewer", Keywords = "Brewer, Beer", OverviewOfRole = "Brewery operations", Level = 4, Route = "Engineering and manufacturing"},
                new Standard{Id = 3, Title = "Senior / head of facilities management (degree)", Keywords = "Head", OverviewOfRole = "Overseer of brewery operations", Level = 6, Route = "Construction"},
            };
        }
    }
}
