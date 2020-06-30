using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Data.Configuration;

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
        int SaveChanges();
    }
    
    public partial class CoursesDataContext : DbContext, ICoursesDataContext
    {
        public DbSet<Domain.Entities.Standard> Standards { get; set; }
        public DbSet<Domain.Entities.StandardImport> StandardsImport { get; set; }
        public DbSet<Domain.Entities.ImportAudit> ImportAudit { get; set; }
        public DbSet<Domain.Entities.Sector> Sectors { get; set; }
        public DbSet<Domain.Entities.SectorImport> SectorsImport { get; set; }
        public DbSet<Domain.Entities.LarsStandardImport> LarsStandardsImport { get; set; }
        public DbSet<Domain.Entities.ApprenticeshipFundingImport> ApprenticeshipFundingImport { get; set; }

        public CoursesDataContext()
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public CoursesDataContext(DbContextOptions options) :base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Standard());
            modelBuilder.ApplyConfiguration(new StandardImport());
            modelBuilder.ApplyConfiguration(new ImportAudit());
            modelBuilder.ApplyConfiguration(new Sector());
            modelBuilder.ApplyConfiguration(new SectorImport());
            modelBuilder.ApplyConfiguration(new ApprenticeshipFundingImport());
            modelBuilder.ApplyConfiguration(new LarsStandardImport());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}