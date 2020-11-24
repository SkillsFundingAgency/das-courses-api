using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.Courses.Data.Extensions;
using SFA.DAS.Courses.Domain.Entities.Versioning;

namespace SFA.DAS.Courses.Data.Configuration.Versioning
{
    public class StandardStagingConfiguration : IEntityTypeConfiguration<StandardStaging>
    {
        public void Configure(EntityTypeBuilder<StandardStaging> builder)
        {
            builder.ToTable("Standard_Staging", "Versioning");
            builder.HasKey(x => x.StandardUId);

            builder.Property(x => x.StandardUId).HasColumnName(nameof(StandardStaging.StandardUId)).HasColumnType("varchar").HasMaxLength(40).IsRequired();
            builder.Property(x => x.LarsCode).HasColumnName(nameof(StandardStaging.LarsCode)).HasColumnType("int");
            builder.Property(x => x.ReferenceNumber).HasColumnName(nameof(StandardStaging.ReferenceNumber)).HasColumnType("varchar").HasMaxLength(20).IsRequired();
            builder.Property(x => x.Title).HasColumnName(nameof(StandardStaging.Title)).HasColumnType("varchar").HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Status).HasColumnName(nameof(StandardStaging.Status)).HasColumnType("varchar").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Version).HasColumnName(nameof(StandardStaging.Version)).HasColumnType("varchar").HasMaxLength(20);
            builder.Property(x => x.EarlierStartDate).HasColumnName(nameof(StandardStaging.EarlierStartDate)).HasColumnType("datetime");
            builder.Property(x => x.LatestStartDate).HasColumnName(nameof(StandardStaging.LatestStartDate)).HasColumnType("datetime");
            builder.Property(x => x.LatestEndDate).HasColumnName(nameof(StandardStaging.LatestEndDate)).HasColumnType("datetime");
            builder.Property(x => x.OverviewOfRole).HasColumnName(nameof(StandardStaging.OverviewOfRole));
            builder.Property(x => x.Level).HasColumnName(nameof(StandardStaging.Level)).HasColumnType("int");
            builder.Property(x => x.TypicalDuration).HasColumnName(nameof(StandardStaging.TypicalDuration)).HasColumnType("int");
            builder.Property(x => x.MaxFunding).HasColumnName(nameof(StandardStaging.MaxFunding)).HasColumnType("smallmoney");
            builder.Property(x => x.Route).HasColumnName(nameof(StandardStaging.Route)).HasColumnType("varchar").HasMaxLength(500);
            builder.Property(x => x.Keywords).HasColumnName(nameof(StandardStaging.Keywords)).HasJsonConversion();
            builder.Property(x => x.AssessmentPlanUrl).HasColumnName(nameof(StandardStaging.AssessmentPlanUrl)).HasColumnType("varchar").HasMaxLength(500);
            builder.Property(x => x.SSA1).HasColumnName(nameof(StandardStaging.SSA1)).HasColumnType("varchar").HasMaxLength(500);
            builder.Property(x => x.SSA2).HasColumnName(nameof(StandardStaging.SSA2)).HasColumnType("varchar").HasMaxLength(500);
            builder.Property(x => x.StandardInformation).HasColumnName(nameof(StandardStaging.StandardInformation));
            builder.Property(x => x.Knowledges).HasColumnName(nameof(StandardStaging.Knowledges)).HasJsonConversion();
            builder.Property(x => x.Behaviours).HasColumnName(nameof(StandardStaging.Behaviours)).HasJsonConversion();
            builder.Property(x => x.Skills).HasColumnName(nameof(StandardStaging.Skills)).HasJsonConversion();
            builder.Property(x => x.Options).HasColumnName(nameof(StandardStaging.Options)).HasJsonConversion();
            builder.Property(x => x.OptionsUnstructuredTemplate).HasColumnName(nameof(StandardStaging.OptionsUnstructuredTemplate)).HasJsonConversion();
            builder.Property(x => x.IntegratedApprenticeship).HasColumnName(nameof(StandardStaging.IntegratedApprenticeship)).HasColumnType("bit");
            builder.Property(x => x.IntegratedDegree).HasColumnName(nameof(StandardStaging.IntegratedDegree)).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.CoreAndOptions).HasColumnName(nameof(StandardStaging.CoreAndOptions)).HasColumnType("bit");
            builder.Property(x => x.TypicalJobTitles).HasColumnName(nameof(StandardStaging.TypicalJobTitles)).HasJsonConversion();
            builder.Property(x => x.StandardPageUrl).HasColumnName(nameof(StandardStaging.StandardPageUrl)).HasColumnType("varchar").HasMaxLength(500);

            builder.HasIndex(x => x.ReferenceNumber).IsUnique();
        }
    }
}
