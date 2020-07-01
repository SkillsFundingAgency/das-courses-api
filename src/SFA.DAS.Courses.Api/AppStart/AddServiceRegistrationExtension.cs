﻿using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Application.StandardsImport.Services;
using SFA.DAS.Courses.Data.Repository;
using SFA.DAS.Courses.Data.Search;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Infrastructure.Api;

namespace SFA.DAS.Courses.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddSingleton<IDirectoryFactory>(new DirectoryFactory());
            services.AddTransient<IIndexBuilder, CoursesIndexBuilder>();
            services.AddTransient<ISearchManager, CoursesSearchManager>();
            services.AddHttpClient<IInstituteOfApprenticeshipService, InstituteOfApprenticeshipService>();
            services.AddTransient<IStandardsImportService, StandardsImportService>();
            services.AddTransient<IStandardsService, StandardsService>();
            services.AddTransient<IStandardImportRepository, StandardImportRepository>();
            services.AddTransient<IStandardRepository, StandardRepository>();
            services.AddTransient<IImportAuditRepository, ImportAuditRepository>();
        }
    }
}
