using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class SectorImport : IEntityTypeConfiguration<Domain.Entities.SectorImport>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.SectorImport> builder)
        {
            builder.ToTable("Sector_Import");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.Route).HasColumnName("Route").HasColumnType("varchar").HasMaxLength(500).IsRequired();

            builder.Ignore(c => c.Standards);
            
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}