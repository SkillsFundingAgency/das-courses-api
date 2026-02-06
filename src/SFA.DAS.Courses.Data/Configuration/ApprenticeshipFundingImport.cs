using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class ApprenticeshipFundingImport : IEntityTypeConfiguration<Domain.Entities.ApprenticeshipFundingImport>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ApprenticeshipFundingImport> builder)
        {
            builder.ToTable("ApprenticeshipFunding_Import");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.LarsCode).HasColumnName("LarsCode").HasColumnType("int").IsRequired();
            builder.Property(x => x.EffectiveFrom).HasColumnName("EffectiveFrom").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EffectiveTo).HasColumnName("EffectiveTo").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.MaxEmployerLevyCap).HasColumnName("MaxEmployerLevyCap").HasColumnType("decimal").HasPrecision(7, 2).IsRequired();
            builder.Property(x => x.Duration).HasColumnName("Duration").HasColumnType("int").IsRequired();
        }
    }
}
