using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        DbSet<Domain.Entities.LarsStandardImport> LarsStandardsImport { get; set; }
        DbSet<Domain.Entities.ApprenticeshipFundingImport> ApprenticeshipFundingImport { get; set; }
        DbSet<Domain.Entities.LarsStandard> LarsStandards { get; set; }
        DbSet<Domain.Entities.ApprenticeshipFunding> ApprenticeshipFunding { get; set; }
        DbSet<Domain.Entities.Framework> Frameworks { get; set; }
        DbSet<Domain.Entities.FrameworkImport> FrameworksImport { get; set; }
        DbSet<Domain.Entities.FrameworkFunding> FrameworkFunding { get; set; }
        DbSet<Domain.Entities.FrameworkFundingImport> FrameworkFundingImport { get; set; }
        DbSet<Domain.Entities.FundingImport> FundingImport { get; set; }
        DbSet<Domain.Entities.SectorSubjectAreaTier2> SectorSubjectAreaTier2 { get; set; }
        DbSet<Domain.Entities.SectorSubjectAreaTier2Import> SectorSubjectAreaTier2Import { get; set; }
        DbSet<Domain.Entities.SectorSubjectAreaTier1> SectorSubjectAreaTier1 { get; set; }
        DbSet<Domain.Entities.Route> Routes { get; set; }
        DbSet<Domain.Entities.RouteImport> RoutesImport { get; set; }
        DbSet<Domain.Entities.SectorSubjectAreaTier1Import> SectorSubjectAreaTier1Import { get; set; }
        DbSet<Domain.Entities.ShortCourseDates> ShortCourseDates { get; set; }

        Task DeleteAllBatchedAsync<TEntity>(int batchSize = 200, CancellationToken cancellationToken = default)
            where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public partial class CoursesDataContext : DbContext, ICoursesDataContext
    {
        private const string AzureResource = "https://database.windows.net//.default";

        public DbSet<Domain.Entities.Standard> Standards { get; set; }
        public DbSet<Domain.Entities.StandardImport> StandardsImport { get; set; }
        public DbSet<Domain.Entities.ImportAudit> ImportAudit { get; set; }
        public DbSet<Domain.Entities.LarsStandard> LarsStandards { get; set; }
        public DbSet<Domain.Entities.LarsStandardImport> LarsStandardsImport { get; set; }
        public DbSet<Domain.Entities.ApprenticeshipFunding> ApprenticeshipFunding { get; set; }
        public DbSet<Domain.Entities.ApprenticeshipFundingImport> ApprenticeshipFundingImport { get; set; }
        public DbSet<Domain.Entities.Framework> Frameworks { get; set; }
        public DbSet<Domain.Entities.FrameworkImport> FrameworksImport { get; set; }
        public DbSet<Domain.Entities.FrameworkFunding> FrameworkFunding { get; set; }
        public DbSet<Domain.Entities.FrameworkFundingImport> FrameworkFundingImport { get; set; }
        public DbSet<Domain.Entities.FundingImport> FundingImport { get; set; }
        public DbSet<Domain.Entities.SectorSubjectAreaTier2> SectorSubjectAreaTier2 { get; set; }
        public DbSet<Domain.Entities.SectorSubjectAreaTier2Import> SectorSubjectAreaTier2Import { get; set; }
        public DbSet<Domain.Entities.Route> Routes { get; set; }
        public DbSet<Domain.Entities.RouteImport> RoutesImport { get; set; }
        public DbSet<Domain.Entities.SectorSubjectAreaTier1Import> SectorSubjectAreaTier1Import { get; set; }
        public DbSet<Domain.Entities.SectorSubjectAreaTier1> SectorSubjectAreaTier1 { get; set; }
        public DbSet<Domain.Entities.ShortCourseDates> ShortCourseDates { get; set; }

        private readonly CoursesConfiguration _configuration;
        private readonly TokenCredential _credential;

        public CoursesDataContext(DbContextOptions<CoursesDataContext> options)
            : base(options)
        {
        }

        public CoursesDataContext(
            IOptions<CoursesConfiguration> config,
            DbContextOptions<CoursesDataContext> options,
            TokenCredential credential)
            : base(options)
        {
            _configuration = config.Value;
            _credential = credential;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            if (_configuration == null)
            {
                optionsBuilder.UseSqlServer();
                return;
            }

            var connection = new SqlConnection(_configuration.ConnectionString);

            if (_credential != null)
            {
                connection.AccessTokenCallback = async (_, cancellationToken) =>
                {
                    var token = await _credential.GetTokenAsync(
                        new TokenRequestContext(new[] { AzureResource }),
                        cancellationToken);

                    return new SqlAuthenticationToken(token.Token, token.ExpiresOn);
                };
            }

            optionsBuilder.UseSqlServer(connection, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(20),
                    errorNumbersToAdd: null);
            });

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Standard());
            modelBuilder.ApplyConfiguration(new StandardImport());
            modelBuilder.ApplyConfiguration(new ImportAudit());
            modelBuilder.ApplyConfiguration(new ApprenticeshipFunding());
            modelBuilder.ApplyConfiguration(new ApprenticeshipFundingImport());
            modelBuilder.ApplyConfiguration(new LarsStandard());
            modelBuilder.ApplyConfiguration(new LarsStandardImport());
            modelBuilder.ApplyConfiguration(new Framework());
            modelBuilder.ApplyConfiguration(new FrameworkImport());
            modelBuilder.ApplyConfiguration(new FrameworkFunding());
            modelBuilder.ApplyConfiguration(new FrameworkFundingImport());
            modelBuilder.ApplyConfiguration(new FundingImport());
            modelBuilder.ApplyConfiguration(new SectorSubjectAreaTier2());
            modelBuilder.ApplyConfiguration(new SectorSubjectAreaTier2Import());
            modelBuilder.ApplyConfiguration(new SectorSubjectAreaTier1Import());
            modelBuilder.ApplyConfiguration(new SectorSubjectAreaTier1());
            modelBuilder.ApplyConfiguration(new ShortCourseDates());
            modelBuilder.ApplyConfiguration(new Route());
            modelBuilder.ApplyConfiguration(new RouteImport());

            base.OnModelCreating(modelBuilder);
        }

        public async Task DeleteAllBatchedAsync<TEntity>(
            int batchSize = 200,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var set = Set<TEntity>();

            while (true)
            {
                var deleted = await set
                    .Take(batchSize)
                    .ExecuteDeleteAsync(cancellationToken);

                if (deleted < batchSize)
                {
                    break;
                }
            }
        }
    }
}
