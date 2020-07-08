using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class Framework : IEntityTypeConfiguration<Domain.Entities.Framework>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Framework> builder)
        {
            builder.ToTable("Framework");
            builder.HasKey(x=>x.Id);
            
            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("varchar").HasMaxLength(15).IsRequired();
            builder.Property(x => x.Title).HasColumnName("Title").HasColumnType("varchar").HasMaxLength(1000).IsRequired();
            builder.Property(x => x.FrameworkName).HasColumnName("FrameworkName").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.PathwayName).HasColumnName("PathwayName").HasColumnType("varchar").HasMaxLength(500).IsRequired();
            builder.Property(x => x.ProgType).HasColumnName("ProgType").HasColumnType("int").IsRequired();
            builder.Property(x => x.FrameworkCode).HasColumnName("FrameworkCode").HasColumnType("int").IsRequired();
            builder.Property(x => x.PathwayCode).HasColumnName("PathwayCode").HasColumnType("int").IsRequired();
            builder.Property(x => x.Level).HasColumnName("Level").HasColumnType("int").IsRequired();
            builder.Property(x => x.TypicalLengthFrom).HasColumnName("TypicalLengthFrom").HasColumnType("int").IsRequired();
            builder.Property(x => x.TypicalLengthTo).HasColumnName("TypicalLengthTo").HasColumnType("int").IsRequired();
            builder.Property(x => x.TypicalLengthUnit).HasColumnName("TypicalLengthUnit").HasColumnType("varchar").HasMaxLength(10).IsRequired();
            builder.Property(x => x.Duration).HasColumnName("Duration").HasColumnType("int").IsRequired();
            builder.Property(x => x.CurrentFundingCap).HasColumnName("CurrentFundingCap").HasColumnType("int").IsRequired();
            builder.Property(x => x.MaxFunding).HasColumnName("MaxFunding").HasColumnType("int").IsRequired();
            builder.Property(x => x.Ssa1).HasColumnName("Ssa1").HasColumnType("decimal").IsRequired();
            builder.Property(x => x.Ssa2).HasColumnName("Ssa2").HasColumnType("decimal").IsRequired();
            builder.Property(x => x.EffectiveFrom).HasColumnName("EffectiveFrom").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EffectiveTo).HasColumnName("EffectiveTo").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.IsActiveFramework).HasColumnName("IsActiveFramework").HasColumnType("bit").IsRequired();
            builder.Property(x => x.HasSubGroups).HasColumnName("HasSubGroups").HasColumnType("bit").IsRequired();
            builder.Property(x => x.ProgrammeType).HasColumnName("ProgrammeType").HasColumnType("int").IsRequired();
            builder.Property(x => x.ExtendedTitle).HasColumnName("ExtendedTitle").HasColumnType("varchar").HasMaxLength(2000).IsRequired();

            builder.HasMany(c => c.FundingPeriods)
                .WithOne(c => c.Framework)
                .HasForeignKey(c => c.FrameworkId)
                .HasPrincipalKey(c => c.Id).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}