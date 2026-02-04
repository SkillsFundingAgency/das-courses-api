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
            services.AddHttpClient<ISkillsEnglandService, SkillsEnglandService>();
            services.AddHttpClient<ISlackNotificationService, SlackNotificationService>();
            services.AddTransient<IStandardsImportService, StandardsImportService>();
            services.AddTransient<IStandardsService, StandardsService>();
            services.AddTransient<IStandardsSortOrderService, StandardsSortOrderService>();
            services.AddTransient<ILarsPageParser, LarsPageParser>();
            services.AddTransient<ILevelsService, LevelsService>();
            services.AddTransient<IZipArchiveHelper, ZipArchiveHelper>();
            services.AddTransient<ILarsImportService, LarsImportService>();
            services.AddTransient<ILarsImportStagingService, LarsImportStagingService>();
            services.AddTransient<IFrameworksImportService, FrameworksImportService>();
            services.AddTransient<IJsonFileHelper, JsonFileHelper>();
            services.AddTransient<IFrameworksService, FrameworksService>();
            services.AddTransient<IRouteService, RouteService>();
            services.AddHttpClient<IQualificationSectorSubjectAreaService, QualificationSectorSubjectAreaService>();

            services.AddHttpClient<IDataDownloadService, DataDownloadService>();

            AddDatabaseRegistrations(services);
        }

        private static void AddDatabaseRegistrations(IServiceCollection services)
        {
            services.AddTransient<IStandardImportRepository, StandardImportRepository>();
            services.AddTransient<IStandardRepository, StandardRepository>();
            services.AddTransient<IImportAuditRepository, ImportAuditRepository>();
            services.AddTransient<IApprenticeshipFundingImportRepository, ApprenticeshipFundingImportRepository>();
            services.AddTransient<IApprenticeshipFundingRepository, ApprenticeshipFundingRepository>();
            services.AddTransient<ILarsStandardImportRepository, LarsStandardImportRepository>();
            services.AddTransient<ILarsStandardRepository, LarsStandardRepository>();
            services.AddTransient<IFrameworkRepository, FrameworkRepository>();
            services.AddTransient<IFrameworkImportRepository, FrameworkImportRepository>();
            services.AddTransient<IFrameworkFundingImportRepository, FrameworkFundingImportRepository>();
            services.AddTransient<IFrameworkFundingRepository, FrameworkFundingRepository>();
            services.AddTransient<IFundingImportRepository, FundingImportRepository>();
            services.AddTransient<ISectorSubjectAreaTier2Repository, SectorSubjectAreaTier2Repository>();
            services.AddTransient<ISectorSubjectAreaTier2ImportRepository, SectorSubjectAreaTier2ImportRepository>();
            services.AddTransient<ISectorSubjectAreaTier1ImportRepository, SectorSubjectAreaTier1ImportRepository>();
            services.AddTransient<ISectorSubjectAreaTier1Repository, SectorSubjectAreaTier1Repository>();
            services.AddTransient<IRouteRepository, RouteRepository>();
            services.AddTransient<IRouteImportRepository, RouteImportRepository>();
        }
    }
}
