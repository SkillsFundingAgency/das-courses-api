using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.Courses.Data.Extensions;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class StandardImport : IEntityTypeConfiguration<Domain.Entities.StandardImport>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.StandardImport> builder)
        {
            builder.ToTable("Standard_Import");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("int").IsRequired();
            builder.Property(x => x.Title).HasColumnName("Title").HasColumnType("varchar").HasMaxLength(1000).IsRequired();
            builder.Property(x => x.IntegratedDegree).HasColumnName("IntegratedDegree").HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Level).HasColumnName("Level").HasColumnType("int").IsRequired();
            builder.Property(x => x.Version).HasColumnName("Version").HasColumnType("decimal");
            builder.Property(x => x.OverviewOfRole).HasColumnName("OverviewOfRole").IsRequired();
            builder.Property(x => x.RouteId).HasColumnName("RouteId").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.TypicalJobTitles).HasColumnName("TypicalJobTitles");
            builder.Property(x => x.CoreSkillsCount).HasColumnName("CoreSkillsCount");
            builder.Property(x => x.StandardPageUrl).HasColumnName("StandardPageUrl").IsRequired();
            builder.Property(x => x.Keywords).HasColumnName("Keywords");
            builder.Property(x => x.Behaviours).HasJsonConversion();

            builder.Ignore(x => x.Sector);
            builder.Ignore(x => x.ApprenticeshipFunding);
            builder.Ignore(x => x.LarsStandard);

            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
