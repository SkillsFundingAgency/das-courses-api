using System;
using System.IO;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Courses.Application.StandardsImport.Handlers.ImportStandards;
using SFA.DAS.Courses.Application.StandardsImport.Services;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Data.Repository;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Infrastructure.Api;

namespace SFA.DAS.Courses.Api
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _environment = environment;
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
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddOptions();
            services.Configure<CoursesConfiguration>(_configuration.GetSection("Courses"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesConfiguration>>().Value);
            
            var serviceProvider = services.BuildServiceProvider();
            var config = serviceProvider.GetService<IOptions<CoursesConfiguration>>().Value;
            
            services.AddMediatR(typeof(ImportStandardsCommand).Assembly);

            services.AddTransient<IInstituteOfApprenticeshipService, InstituteOfApprenticeshipService>();
            services.AddTransient<IStandardsImportService, StandardsImportService>();
            //services.AddTransient<IStandardsService, StandardsService>();
            services.AddTransient<IStandardImportRepository, StandardImportRepository>();
            services.AddTransient<IStandardRepository, StandardRepository>();
            
            if (_configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddDbContext<CoursesDataContext>(options => options.UseInMemoryDatabase("SFA.DAS.Courses"));
            }
            else
            {
                services.AddDbContext<CoursesDataContext>(options => options.UseSqlServer(config.ConnectionString));
            }

            services.AddScoped<ICoursesDataContext, CoursesDataContext>(provider => provider.GetService<CoursesDataContext>());
            services.AddTransient(provider => new Lazy<CoursesDataContext>(provider.GetService<CoursesDataContext>()));

            
            services.AddApplicationInsightsTelemetry(_configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

            services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller=Standards}/{action=Index}/{id?}");
            });
            
            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}