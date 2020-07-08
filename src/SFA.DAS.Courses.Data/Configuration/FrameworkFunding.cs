using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class FrameworkFunding : IEntityTypeConfiguration<Domain.Entities.FrameworkFunding>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.FrameworkFunding> builder)
        {
            builder.ToTable("FrameworkFundingPeriod");
            builder.HasKey(x=>x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.FrameworkId).HasColumnName("FrameworkId").HasColumnType("varchar").HasMaxLength(15).IsRequired();
            builder.Property(x => x.EffectiveFrom).HasColumnName("EffectiveFrom").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EffectiveTo).HasColumnName("EffectiveTo").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.FundingCap).HasColumnName("FundingCap").HasColumnType("int").IsRequired();

            builder.HasOne(c => c.Framework)
                .WithMany(c => c.FundingPeriods)
                .HasForeignKey(c => c.FrameworkId)
                .HasPrincipalKey(c => c.Id).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}