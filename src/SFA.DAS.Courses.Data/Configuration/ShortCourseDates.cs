using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class ShortCourseDates : IEntityTypeConfiguration<Domain.Entities.ShortCourseDates>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ShortCourseDates> builder)
        {
            builder.ToTable("ShortCourseDates");
            builder.HasKey(x => x.LarsCode);

            builder.Property(x => x.LarsCode).HasColumnName("LarsCode").HasColumnType("varchar").HasMaxLength(8).IsRequired();
            
            builder.Property(x => x.EffectiveFrom).HasColumnName("EffectiveFrom").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EffectiveTo).HasColumnName("EffectiveTo").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.LastDateStarts).HasColumnName("LastDateStarts").HasColumnType("datetime").IsRequired(false);
            
            builder.HasMany(c => c.Standards)
                .WithOne(c => c.ShortCourseDates)
                .HasForeignKey(s => s.LarsCode)
                .HasPrincipalKey(l => l.LarsCode)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
