using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.Courses.Data.Extensions;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class Standard : IEntityTypeConfiguration<Domain.Entities.Standard>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Standard> builder)
        {
            builder.ToTable("Standard");
            builder.HasKey(x => x.StandardUId);

            builder.Property(x => x.StandardUId).HasColumnName("StandardUId").HasColumnType("varchar").HasMaxLength(20).IsRequired();
            builder.Property(x => x.LarsCode).HasColumnName("LarsCode").HasColumnType("int");
            builder.Property(x => x.IfateReferenceNumber).HasColumnName("IfateReferenceNumber").HasColumnType("varchar").HasMaxLength(10).IsRequired();
            builder.Property(x => x.Status).HasColumnName("Status").HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Title).HasColumnName("Title").HasColumnType("varchar").HasMaxLength(1000).IsRequired();
            builder.Property(x => x.IntegratedDegree).HasColumnName("IntegratedDegree").HasColumnType("varchar").HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.Level).HasColumnName("Level").HasColumnType("int").IsRequired();
            builder.Property(x => x.Version).HasColumnName("Version").HasColumnType("varchar").HasMaxLength(20);
            builder.Property(x => x.OverviewOfRole).HasColumnName("OverviewOfRole").IsRequired();
            builder.Property(x => x.RouteCode).HasColumnName("RouteCode").HasColumnType("int").IsRequired();
            builder.Property(x => x.TypicalJobTitles).HasColumnName("TypicalJobTitles");
            builder.Property(x => x.StandardPageUrl).HasColumnName("StandardPageUrl").IsRequired();
            builder.Property(x => x.Keywords).HasColumnName("Keywords");
            builder.Property(x => x.RegulatedBody).HasColumnName("RegulatedBody").HasColumnType("varchar").HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.Duties).HasJsonConversion();
            builder.Property(x => x.CoreDuties).HasJsonConversion();
            builder.Property(x => x.IntegratedApprenticeship).HasColumnName("IntegratedApprenticeship").HasColumnType("bit").IsRequired();
            builder.Property(x => x.CoreAndOptions).HasColumnName("CoreAndOptions").IsRequired();
            builder.Property(x => x.Options).HasJsonConversion();

            builder.HasOne(c => c.Route)
                .WithMany(c => c.Standards)
                .HasPrincipalKey(c => c.Id)
                .HasForeignKey(c => c.RouteCode).Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            
            builder.HasOne(c => c.LarsStandard)
                .WithMany(c => c.Standards)
                .HasForeignKey(s => s.LarsCode)
                .HasPrincipalKey(l => l.LarsCode)
                .IsRequired(false)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.HasMany(c => c.ApprenticeshipFunding)
                .WithOne(c => c.Standard)
                .HasForeignKey(c => c.StandardUId)
                .HasPrincipalKey(c => c.StandardUId)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.Ignore(x => x.SearchScore);

            builder.HasIndex(x => x.StandardUId).IsUnique();
        }
    }
}
