using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace SFA.DAS.Courses.Data.Configuration
{
    public class Standard : IEntityTypeConfiguration<Domain.Entities.Standard>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Standard> builder)
        {
            builder.ToTable("Standard");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("int").IsRequired();
            builder.Property(x => x.TypicalDuration).HasColumnName("TypicalDuration").HasColumnType("int").IsRequired();
            builder.Property(x => x.Title).HasColumnName("Title").HasColumnType("varchar").HasMaxLength(1000).IsRequired();
            builder.Property(x => x.IntegratedDegree).HasColumnName("IntegratedDegree").HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Level).HasColumnName("Level").HasColumnType("int").IsRequired();
            builder.Property(x => x.Version).HasColumnName("Version").HasColumnType("decimal");
            builder.Property(x => x.MaxFunding).HasColumnName("MaxFunding").HasColumnType("bigint").IsRequired();
            builder.Property(x => x.OverviewOfRole).HasColumnName("OverviewOfRole").IsRequired();
            builder.Property(x => x.Route).HasColumnName("Route").IsRequired();
            builder.Property(x => x.TypicalJobTitles).HasColumnName("TypicalJobTitles");
            builder.Property(x => x.CoreSkillsCount).HasColumnName("CoreSkillsCount");
            builder.Property(x => x.StandardPageUrl).HasColumnName("StandardPageUrl").IsRequired();
            builder.Property(x => x.Keywords).HasColumnName("Keywords");
            
            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}