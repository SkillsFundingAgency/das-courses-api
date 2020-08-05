﻿using System;
using System.IO;
using System.Linq;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Courses.Api.AppStart;
using SFA.DAS.Courses.Api.Infrastructure;
using SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Infrastructure.HealthCheck;

namespace SFA.DAS.Courses.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();


            if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
            {

#if DEBUG
                config
                    .AddJsonFile("appsettings.json", true)
                    .AddJsonFile("appsettings.Development.json", true);
#endif

                config.AddAzureTableStorage(options =>
                    {
                        options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                        options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                        options.EnvironmentName = configuration["Environment"];
                        options.PreFixConfigurationKeys = false;
                    }
                );
            }

            _configuration = config.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddOptions();
            services.Configure<CoursesConfiguration>(_configuration.GetSection("Courses"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesConfiguration>>().Value);
            services.Configure<AzureActiveDirectoryConfiguration>(_configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

            var coursesConfiguration = _configuration
                .GetSection("Courses")
                .Get<CoursesConfiguration>();

            if (!ConfigurationIsLocalOrDev())
            {
                var azureAdConfiguration = _configuration
                    .GetSection("AzureAd")
                    .Get<AzureActiveDirectoryConfiguration>();

                services.AddAuthentication(azureAdConfiguration);
            }

            if (_configuration["Environment"] != "DEV")
            {
                services.AddHealthChecks()
                    .AddDbContextCheck<CoursesDataContext>()
                    .AddCheck<LarsHealthCheck>("Lars Data Health Check",
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] {"ready"})
                    .AddCheck<InstituteOfApprenticeshipServiceHealthCheck>("IFATE Health Check",
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] {"ready"})
                    .AddCheck<FrameworksHealthCheck>("Frameworks Health Check",
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] {"ready"});
            }

            services.AddMediatR(typeof(ImportDataCommand).Assembly);

            services.AddServiceRegistration();

            services.AddDatabaseRegistration(coursesConfiguration, _configuration["Environment"]);

            services
                .AddMvc(o =>
                {
                    if (!ConfigurationIsLocalOrDev())
                    {
                        o.Conventions.Add(new AuthorizeControllerModelConvention());
                    }
                    o.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoursesAPI", Version = "v1" });
                c.SwaggerDoc("operations", new OpenApiInfo { Title = "CoursesAPI operations" });
                c.OperationFilter<SwaggerVersionHeaderFilter>();
            });
            
            services.AddApiVersioning(opt => {
                opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");
            });

            services.AddLogging();
        }
        

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IIndexBuilder indexBuilder)
        {
            indexBuilder.Build();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoursesAPI v1");
                c.SwaggerEndpoint("/swagger/operations/swagger.json", "Operations v1");
                c.RoutePrefix = string.Empty;
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            if (!_configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
            {
                app.UseHealthChecks();
            }

            app.UseRouting();
            app.UseEndpoints(builder =>
            {
                builder.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Standards}/{action=Index}/{id?}");
            });

            
        }

        private bool ConfigurationIsLocalOrDev()
        {
            return _configuration["Environment"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase) ||
                   _configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
