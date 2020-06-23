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
            builder.Property(x => x.Route).HasColumnName("Route").HasColumnType("varchar").IsRequired();

            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}