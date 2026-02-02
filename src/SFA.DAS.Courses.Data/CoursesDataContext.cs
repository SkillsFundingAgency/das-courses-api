using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Data.Configuration;

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
        DbSet<Domain.Entities.SectorSubjectAreaTier2> SectorSubjectAreaTier2 { get; set; }
        DbSet<Domain.Entities.SectorSubjectAreaTier2Import> SectorSubjectAreaTier2Import { get; set; }
        DbSet<Domain.Entities.SectorSubjectAreaTier1> SectorSubjectAreaTier1 { get; set; }
        DbSet<Domain.Entities.Route> Routes { get; set; }
        DbSet<Domain.Entities.RouteImport> RoutesImport { get; set; }
        DbSet<Domain.Entities.SectorSubjectAreaTier1Import> SectorSubjectAreaTier1Import { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public partial class CoursesDataContext : DbContext, ICoursesDataContext
    {
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
        public DbSet<Domain.Entities.SectorSubjectAreaTier2> SectorSubjectAreaTier2 { get; set; }
        public DbSet<Domain.Entities.SectorSubjectAreaTier2Import> SectorSubjectAreaTier2Import { get; set; }
        public DbSet<Domain.Entities.Route> Routes { get; set; }
        public DbSet<Domain.Entities.RouteImport> RoutesImport { get; set; }
        public DbSet<Domain.Entities.SectorSubjectAreaTier1Import> SectorSubjectAreaTier1Import { get; set; }
        public DbSet<Domain.Entities.SectorSubjectAreaTier1> SectorSubjectAreaTier1 { get; set; }

        public CoursesDataContext()
        {
        }

        public CoursesDataContext(DbContextOptions options) : base(options)
        {
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
            modelBuilder.ApplyConfiguration(new SectorSubjectAreaTier2());
            modelBuilder.ApplyConfiguration(new SectorSubjectAreaTier2Import());
            modelBuilder.ApplyConfiguration(new SectorSubjectAreaTier1Import());
            modelBuilder.ApplyConfiguration(new SectorSubjectAreaTier1());
            modelBuilder.ApplyConfiguration(new Route());
            modelBuilder.ApplyConfiguration(new RouteImport());
            base.OnModelCreating(modelBuilder);
        }
    }
}
