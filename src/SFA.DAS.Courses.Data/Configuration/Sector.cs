using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class Sector : IEntityTypeConfiguration<Domain.Entities.Sector>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Sector> builder)
        {
            builder.ToTable("Sector");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.Route).HasColumnName("Route").HasColumnType("varchar").HasMaxLength(500).IsRequired();

            builder.HasMany(c => c.Standards)
                .WithOne(c => c.Sector)
                .HasForeignKey(c => c.RouteId)
                .HasPrincipalKey(c => c.Id);
            
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}