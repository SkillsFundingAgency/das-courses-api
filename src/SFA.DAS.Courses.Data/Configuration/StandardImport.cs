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
            builder.HasKey(x => x.StandardUId);

            builder.Property(x => x.StandardUId).HasColumnName("StandardUId").HasColumnType("varchar").HasMaxLength(20).IsRequired();
            builder.Property(x => x.LarsCode).HasColumnName("LarsCode").HasColumnType("int");
            builder.Property(x => x.IfateReferenceNumber).HasColumnName("IfateReferenceNumber").HasColumnType("varchar").HasMaxLength(10).IsRequired();
            builder.Property(x => x.Status).HasColumnName("Status").HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Title).HasColumnName("Title").HasColumnType("varchar").HasMaxLength(1000).IsRequired();
            builder.Property(x => x.IntegratedDegree).HasColumnName("IntegratedDegree").HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Level).HasColumnName("Level").HasColumnType("int").IsRequired();
            builder.Property(x => x.Version).HasColumnName("Version").HasColumnType("decimal");
            builder.Property(x => x.OverviewOfRole).HasColumnName("OverviewOfRole").IsRequired();
            builder.Property(x => x.RouteId).HasColumnName("RouteId").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.TypicalJobTitles).HasColumnName("TypicalJobTitles");
            builder.Property(x => x.StandardPageUrl).HasColumnName("StandardPageUrl").IsRequired();
            builder.Property(x => x.Keywords).HasColumnName("Keywords");
            builder.Property(x => x.RegulatedBody).HasColumnName("RegulatedBody").HasColumnType("varchar").HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.Skills).HasJsonConversion();
            builder.Property(x => x.Knowledge).HasJsonConversion();
            builder.Property(x => x.Behaviours).HasJsonConversion();
            builder.Property(x => x.Duties).HasJsonConversion();
            builder.Property(x => x.IntegratedApprenticeship).HasColumnName("IntegratedApprenticeship").HasColumnType("bit").IsRequired();
            builder.Property(x => x.CoreAndOptions).HasColumnName("CoreAndOptions").IsRequired();
            builder.Property(x => x.Options).HasJsonConversion();
            builder.Property(x => x.OptionsUnstructuredTemplate).HasJsonConversion();

            builder.Ignore(x => x.Sector);
            builder.Ignore(x => x.ApprenticeshipFunding);
            builder.Ignore(x => x.LarsStandard);
            builder.Ignore(x => x.CoreDuties);

            builder.HasIndex(x => x.StandardUId).IsUnique();
        }
    }
}
