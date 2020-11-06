using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.Courses.Data.Extensions;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class Standard : IEntityTypeConfiguration<Domain.Entities.Standard>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Standard> builder)
        {
            builder.ToTable("Standard");
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
            builder.Property(x => x.RegulatedBody).HasColumnName("RegulatedBody").HasColumnType("varchar").HasMaxLength(1000).IsRequired(false);
            builder.Property(x => x.Skills).HasJsonConversion();
            builder.Property(x => x.Knowledge).HasJsonConversion();
            builder.Property(x => x.Behaviours).HasJsonConversion();

            builder.HasOne(c => c.Sector)
                .WithMany(c => c.Standards)
                .HasPrincipalKey(c => c.Id)
                .HasForeignKey(c => c.RouteId).Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.HasOne(c => c.LarsStandard)
                .WithOne(c => c.Standard)
                .HasForeignKey<Domain.Entities.LarsStandard>(c => c.StandardId)
                .HasPrincipalKey<Domain.Entities.Standard>(c => c.Id).Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.HasMany(c => c.ApprenticeshipFunding)
                .WithOne(c => c.Standard)
                .HasForeignKey(c => c.StandardId)
                .HasPrincipalKey(c => c.Id).Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder.Ignore(x => x.SearchScore);

            builder.HasIndex(x => x.Id).IsUnique();
        }
    }
}
