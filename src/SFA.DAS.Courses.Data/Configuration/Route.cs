using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class Route : IEntityTypeConfiguration<Domain.Entities.Route>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Route> builder)
        {
            builder.ToTable("Route");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("int").IsRequired();
            builder.Property(x => x.Name).HasColumnName("Name").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            
            builder.HasMany(c => c.Standards)
                .WithOne(c => c.Route)
                .HasForeignKey(c => c.RouteId)
                .HasPrincipalKey(c => c.Id).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}