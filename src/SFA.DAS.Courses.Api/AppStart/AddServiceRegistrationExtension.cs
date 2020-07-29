using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Application.CoursesImport.Services;
using SFA.DAS.Courses.Data.Repository;
using SFA.DAS.Courses.Data.Search;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Infrastructure.Api;
using SFA.DAS.Courses.Infrastructure.FileHelper;
using SFA.DAS.Courses.Infrastructure.PageParsing;
using SFA.DAS.Courses.Infrastructure.StreamHelper;

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
            services.AddTransient<IStandardsSortOrderService, StandardsSortOrderService>();
            services.AddTransient<IStandardImportRepository, StandardImportRepository>();
            services.AddTransient<IStandardRepository, StandardRepository>();
            services.AddTransient<IImportAuditRepository, ImportAuditRepository>();
            services.AddTransient<ISectorRepository, SectorRepository>();
            services.AddTransient<ISectorImportRepository, SectorImportRepository>();
            services.AddTransient<IApprenticeshipFundingImportRepository, ApprenticeshipFundingImportRepository>();
            services.AddTransient<IApprenticeshipFundingRepository, ApprenticeshipFundingRepository>();
            services.AddTransient<ILarsStandardImportRepository, LarsStandardImportRepository>();
            services.AddTransient<ILarsStandardRepository, LarsStandardRepository>();
            services.AddTransient<IFrameworkRepository, FrameworkRepository>();
            services.AddTransient<IFrameworkImportRepository, FrameworkImportRepository>();
            services.AddTransient<IFrameworkFundingImportRepository, FrameworkFundingImportRepository>();
            services.AddTransient<IFrameworkFundingRepository, FrameworkFundingRepository>();
            services.AddTransient<ISectorService, SectorService>();
            services.AddTransient<ILarsPageParser, LarsPageParser>();
            services.AddTransient<ILevelsService, LevelsService>();
            services.AddHttpClient<ILarsDataDownloadService, LarsDataDownloadService>();
            services.AddTransient<IZipArchiveHelper, ZipArchiveHelper>();
            services.AddTransient<ILarsImportService, LarsImportService>();
            services.AddTransient<IFrameworksImportService, FrameworksImportService>();
            services.AddTransient<IJsonFileHelper, JsonFileHelper>();   
            services.AddTransient<IFrameworksService, FrameworksService>();   
        }
    }
}
