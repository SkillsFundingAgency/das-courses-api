using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class SectorSubjectAreaTier2Import : IEntityTypeConfiguration<Domain.Entities.SectorSubjectAreaTier2Import>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.SectorSubjectAreaTier2Import> builder)
        {
            builder.ToTable("SectorSubjectAreaTier2_Import");
            builder.HasKey(x => x.SectorSubjectAreaTier2);
            
            builder.Property(x => x.SectorSubjectAreaTier2).HasColumnName("SectorSubjectAreaTier2").HasColumnType("decimal");
            builder.Property(x => x.SectorSubjectAreaTier2Desc).HasColumnName("SectorSubjectAreaTier2Desc").HasColumnType("varchar").HasMaxLength(500);
            builder.Property(x => x.Name).HasColumnName("Name").HasColumnType("varchar").HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.EffectiveFrom).HasColumnName("EffectiveFrom").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EffectiveTo).HasColumnName("EffectiveTo").HasColumnType("datetime").IsRequired(false);
            
            builder.HasIndex(x => x.SectorSubjectAreaTier2).IsUnique();
        }
    }
}