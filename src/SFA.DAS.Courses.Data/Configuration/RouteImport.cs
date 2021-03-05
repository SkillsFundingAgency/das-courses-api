using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class RouteImport : IEntityTypeConfiguration<Domain.Entities.RouteImport>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.RouteImport> builder)
        {
            builder.ToTable("Route_Import");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("int").IsRequired();
            builder.Property(x => x.Name).HasColumnName("Name").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            
            builder.Ignore(c => c.Standards);
            
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}