using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class LarsStandardImport : IEntityTypeConfiguration<Domain.Entities.LarsStandardImport>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.LarsStandardImport> builder)
        {
            builder.ToTable("LarsStandard_Import");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.StandardId).HasColumnName("StandardId").HasColumnType("int").IsRequired();
            builder.Property(x => x.Version).HasColumnName("Version").HasColumnType("int").IsRequired();
            builder.Property(x => x.EffectiveFrom).HasColumnName("EffectiveFrom").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EffectiveTo).HasColumnName("EffectiveTo").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastDateStarts).HasColumnName("LastDateStarts").HasColumnType("datetime").IsRequired(false);
        }
    }
}