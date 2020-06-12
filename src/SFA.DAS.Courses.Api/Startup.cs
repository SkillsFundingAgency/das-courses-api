using System;
using System.IO;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Courses.Api.AppStart;
using SFA.DAS.Courses.Api.Infrastructure;
using SFA.DAS.Courses.Application.StandardsImport.Handlers.ImportStandards;
using SFA.DAS.Courses.Domain.Configuration;

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
#if DEBUG
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json", true)
#endif
                .AddEnvironmentVariables();

            if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
            {
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
            
            var serviceProvider = services.BuildServiceProvider();

            if (!ConfigurationIsLocalOrDev())
            {
                services.AddAuthentication(serviceProvider.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

            } 
            
            services.AddMediatR(typeof(ImportStandardsCommand).Assembly);

            services.AddServiceRegistration();
            
            services.AddDatabaseRegistration(serviceProvider.GetService<IOptions<CoursesConfiguration>>().Value, _configuration["Environment"]);
            
            services
                .AddMvc(o =>
                {
                    if (!ConfigurationIsLocalOrDev())
                    {
                        o.Conventions.Add(new AuthorizeControllerModelConvention());
                    }
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseAuthentication();    
        
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller=Standards}/{action=Index}/{id?}");
            });
            
        }
        
        private bool ConfigurationIsLocalOrDev()
        {
            return _configuration["Environment"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase) ||
                   _configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
