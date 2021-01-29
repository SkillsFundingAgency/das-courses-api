using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class LarsStandard : IEntityTypeConfiguration<Domain.Entities.LarsStandard>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.LarsStandard> builder)
        {
            builder.ToTable("LarsStandard");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.LarsCode).HasColumnName("LarsCode").HasColumnType("int").IsRequired();
            builder.Property(x => x.Version).HasColumnName("Version").HasColumnType("int").IsRequired();
            builder.Property(x => x.EffectiveFrom).HasColumnName("EffectiveFrom").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EffectiveTo).HasColumnName("EffectiveTo").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastDateStarts).HasColumnName("LastDateStarts").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.SectorSubjectAreaTier2).HasColumnName("SectorSubjectAreaTier2").HasColumnType("decimal");
            builder.Property(x => x.OtherBodyApprovalRequired).HasColumnName("OtherBodyApprovalRequired").HasColumnType("bit").IsRequired();

            builder.HasMany(c => c.Standards)
                .WithOne(c => c.LarsStandard)
                .HasForeignKey(s => s.LarsCode)
                .HasPrincipalKey(l => l.LarsCode)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            
            builder.HasOne(c=>c.SectorSubjectArea)
                .WithMany(c=>c.LarsStandard)
                .HasForeignKey(c=>c.SectorSubjectAreaTier2)
                .HasPrincipalKey(c=>c.SectorSubjectAreaTier2)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
