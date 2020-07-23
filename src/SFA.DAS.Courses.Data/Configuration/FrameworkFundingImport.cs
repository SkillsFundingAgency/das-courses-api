using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class FrameworkFundingImport : IEntityTypeConfiguration<Domain.Entities.FrameworkFundingImport>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.FrameworkFundingImport> builder)
        {
            builder.ToTable("FrameworkFundingPeriod_Import");
            builder.HasKey(x=>x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.FrameworkId).HasColumnName("FrameworkId").HasColumnType("varchar").HasMaxLength(15).IsRequired();
            builder.Property(x => x.EffectiveFrom).HasColumnName("EffectiveFrom").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EffectiveTo).HasColumnName("EffectiveTo").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.FundingCap).HasColumnName("FundingCap").HasColumnType("int").IsRequired();

            builder.Ignore(x => x.Framework);
            
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}