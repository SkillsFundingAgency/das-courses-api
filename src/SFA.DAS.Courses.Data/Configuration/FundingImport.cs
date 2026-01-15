using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class FundingImport : IEntityTypeConfiguration<Domain.Entities.FundingImport>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.FundingImport> builder)
        {
            builder.ToTable("Funding_Import");
            builder.HasKey(x=>x.LearnAimRef);

            builder.Property(x => x.FundingCategory).HasColumnName("FundingCategory").HasColumnType("varchar").HasMaxLength(30).IsRequired();
            builder.Property(x => x.EffectiveFrom).HasColumnName("EffectiveFrom").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EffectiveTo).HasColumnName("EffectiveTo").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.RateWeighted).HasColumnName("RateWeighted").HasColumnType("decimal").HasPrecision(7, 2).IsRequired();
            builder.Property(x => x.RateUnWeighted).HasColumnName("RateUnWeighted").HasColumnType("decimal").HasPrecision(7, 2).IsRequired();
            builder.Property(x => x.RatingFactory).HasColumnName("RatingFactory").HasColumnType("char").HasMaxLength(1).IsRequired();

            builder.HasIndex(x => x.LearnAimRef).IsUnique();
        }
    }
}
