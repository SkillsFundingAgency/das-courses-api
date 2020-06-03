using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Data.Configuration;

namespace SFA.DAS.Courses.Data
{
    public interface ICoursesDataContext
    {
        DbSet<Domain.Entities.Standard> Standards { get; set; }

        int SaveChanges();
    }
    
    public partial class CoursesDataContext : DbContext, ICoursesDataContext
    {
        public DbSet<Domain.Entities.Standard> Standards { get; set; }

        public CoursesDataContext()
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public CoursesDataContext(DbContextOptions options) :base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Standard());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}