using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.Courses.Data.Extensions;
using DomainEntities = SFA.DAS.Courses.Domain.Entities.Versioning;

namespace SFA.DAS.Courses.Data.Configuration.Versioning
{
    public class StandardConfiguration : IEntityTypeConfiguration<DomainEntities.Standard>
    {
        public void Configure(EntityTypeBuilder<DomainEntities.Standard> builder)
        {
            builder.ToTable(nameof(DomainEntities.Standard), "Versioning");
            builder.HasKey(x => x.StandardUId);

            builder.Property(x => x.StandardUId).HasColumnName(nameof(DomainEntities.Standard.StandardUId)).HasColumnType("varchar").HasMaxLength(40).IsRequired();
            builder.Property(x => x.LarsCode).HasColumnName(nameof(DomainEntities.Standard.LarsCode)).HasColumnType("int");
            builder.Property(x => x.ReferenceNumber).HasColumnName(nameof(DomainEntities.Standard.ReferenceNumber)).HasColumnType("varchar").HasMaxLength(20).IsRequired();
            builder.Property(x => x.Title).HasColumnName(nameof(DomainEntities.Standard.Title)).HasColumnType("varchar").HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Status).HasColumnName(nameof(DomainEntities.Standard.Status)).HasColumnType("varchar").HasMaxLength(200).IsRequired();
            builder.Property(x => x.Version).HasColumnName(nameof(DomainEntities.Standard.Version)).HasColumnType("varchar").HasMaxLength(20);
            builder.Property(x => x.EarlierStartDate).HasColumnName(nameof(DomainEntities.Standard.EarlierStartDate)).HasColumnType("datetime");
            builder.Property(x => x.LatestStartDate).HasColumnName(nameof(DomainEntities.Standard.LatestStartDate)).HasColumnType("datetime");
            builder.Property(x => x.LatestEndDate).HasColumnName(nameof(DomainEntities.Standard.LatestEndDate)).HasColumnType("datetime");
            builder.Property(x => x.OverviewOfRole).HasColumnName(nameof(DomainEntities.Standard.OverviewOfRole));
            builder.Property(x => x.Level).HasColumnName(nameof(DomainEntities.Standard.Level)).HasColumnType("int");
            builder.Property(x => x.TypicalDuration).HasColumnName(nameof(DomainEntities.Standard.TypicalDuration)).HasColumnType("int");
            builder.Property(x => x.MaxFunding).HasColumnName(nameof(DomainEntities.Standard.MaxFunding)).HasColumnType("smallmoney");
            builder.Property(x => x.Route).HasColumnName(nameof(DomainEntities.Standard.Route)).HasColumnType("varchar").HasMaxLength(500);
            builder.Property(x => x.SSA1).HasColumnName(nameof(DomainEntities.Standard.SSA1)).HasColumnType("varchar").HasMaxLength(500);
            builder.Property(x => x.SSA2).HasColumnName(nameof(DomainEntities.Standard.SSA2)).HasColumnType("varchar").HasMaxLength(500);
            builder.Property(x => x.IntegratedApprenticeship).HasColumnName(nameof(DomainEntities.Standard.IntegratedApprenticeship)).HasColumnType("bit");
            builder.Property(x => x.IntegratedDegree).HasColumnName(nameof(DomainEntities.Standard.IntegratedDegree)).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(x => x.CoreAndOptions).HasColumnName(nameof(DomainEntities.Standard.CoreAndOptions)).HasColumnType("bit");
            builder.Property(x => x.TypicalJobTitles).HasColumnName(nameof(DomainEntities.Standard.TypicalJobTitles)).HasJsonConversion();
            builder.Property(x => x.StandardPageUrl).HasColumnName(nameof(DomainEntities.Standard.StandardPageUrl)).HasColumnType("varchar").HasMaxLength(500);

            builder.HasIndex(x => x.Status);
        }
    }
}
