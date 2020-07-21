using System;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SFA.DAS.Courses.Data.Configuration;
using SFA.DAS.Courses.Domain.Configuration;

namespace SFA.DAS.Courses.Data
{
    public interface ICoursesDataContext
    {
        DbSet<Domain.Entities.Standard> Standards { get; set; }
        DbSet<Domain.Entities.StandardImport> StandardsImport { get; set; }
        DbSet<Domain.Entities.ImportAudit> ImportAudit { get; set; }
        DbSet<Domain.Entities.Sector> Sectors { get; set; }
        DbSet<Domain.Entities.SectorImport> SectorsImport { get; set; }
        DbSet<Domain.Entities.LarsStandardImport> LarsStandardsImport { get; set; }
        DbSet<Domain.Entities.ApprenticeshipFundingImport> ApprenticeshipFundingImport { get; set; }
        DbSet<Domain.Entities.LarsStandard> LarsStandards { get; set; }
        DbSet<Domain.Entities.ApprenticeshipFunding> ApprenticeshipFunding { get; set; }
        DbSet<Domain.Entities.Framework> Frameworks { get; set; }
        DbSet<Domain.Entities.FrameworkImport> FrameworksImport { get; set; }
        DbSet<Domain.Entities.FrameworkFunding> FrameworkFunding { get; set; }
        DbSet<Domain.Entities.FrameworkFundingImport> FrameworkFundingImport { get; set; }
        int SaveChanges();
    }
    
    public partial class CoursesDataContext : DbContext, ICoursesDataContext
    {
        private const string AzureResource = "https://database.windows.net/";

        public DbSet<Domain.Entities.Standard> Standards { get; set; }
        public DbSet<Domain.Entities.StandardImport> StandardsImport { get; set; }
        public DbSet<Domain.Entities.ImportAudit> ImportAudit { get; set; }
        public DbSet<Domain.Entities.Sector> Sectors { get; set; }
        public DbSet<Domain.Entities.SectorImport> SectorsImport { get; set; }
        public DbSet<Domain.Entities.LarsStandard> LarsStandards { get; set; }
        public DbSet<Domain.Entities.LarsStandardImport> LarsStandardsImport { get; set; }
        public DbSet<Domain.Entities.ApprenticeshipFunding> ApprenticeshipFunding { get; set; }
        public DbSet<Domain.Entities.ApprenticeshipFundingImport> ApprenticeshipFundingImport { get; set; }
        public DbSet<Domain.Entities.Framework> Frameworks { get; set; }
        public DbSet<Domain.Entities.FrameworkImport> FrameworksImport { get; set; }
        public DbSet<Domain.Entities.FrameworkFunding> FrameworkFunding { get; set; }
        public DbSet<Domain.Entities.FrameworkFundingImport> FrameworkFundingImport { get; set; }

        private readonly CoursesConfiguration _configuration;
        private readonly AzureServiceTokenProvider _azureServiceTokenProvider;
     
        public CoursesDataContext()
        {
        }

        public CoursesDataContext(DbContextOptions options) : base(options)
        {
            
        }
        public CoursesDataContext(IOptions<CoursesConfiguration> config, DbContextOptions options, AzureServiceTokenProvider azureServiceTokenProvider) :base(options)
        {
            _configuration = config.Value;
            _azureServiceTokenProvider = azureServiceTokenProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            
            if (_configuration == null || _azureServiceTokenProvider == null)
            {
                return;
            }
            
            var connection = new SqlConnection
            {
                ConnectionString = _configuration.ConnectionString,
                AccessToken = _azureServiceTokenProvider.GetAccessTokenAsync(AzureResource).Result
            };
            optionsBuilder.UseSqlServer(connection);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Standard());
            modelBuilder.ApplyConfiguration(new StandardImport());
            modelBuilder.ApplyConfiguration(new ImportAudit());
            modelBuilder.ApplyConfiguration(new Sector());
            modelBuilder.ApplyConfiguration(new SectorImport());
            modelBuilder.ApplyConfiguration(new ApprenticeshipFunding());
            modelBuilder.ApplyConfiguration(new ApprenticeshipFundingImport());
            modelBuilder.ApplyConfiguration(new LarsStandard());
            modelBuilder.ApplyConfiguration(new LarsStandardImport());
            modelBuilder.ApplyConfiguration(new Framework());
            modelBuilder.ApplyConfiguration(new FrameworkImport());
            modelBuilder.ApplyConfiguration(new FrameworkFunding());
            modelBuilder.ApplyConfiguration(new FrameworkFundingImport());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
