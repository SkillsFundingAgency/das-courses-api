﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class ApprenticeshipFunding : IEntityTypeConfiguration<Domain.Entities.ApprenticeshipFunding>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.ApprenticeshipFunding> builder)
        {
            builder.ToTable("ApprenticeshipFunding");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(x => x.StandardUId).HasColumnName("StandardUId").HasColumnType("varchar").HasMaxLength(20).IsRequired(); 
            builder.Property(x => x.EffectiveFrom).HasColumnName("EffectiveFrom").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.EffectiveTo).HasColumnName("EffectiveTo").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.MaxEmployerLevyCap).HasColumnName("MaxEmployerLevyCap").HasColumnType("int").IsRequired();
            builder.Property(x => x.Duration).HasColumnName("Duration").HasColumnType("int").IsRequired();

            builder.HasOne(c => c.Standard)
                .WithMany(c => c.ApprenticeshipFunding)
                .HasForeignKey(c => c.StandardUId)
                .HasPrincipalKey(c => c.StandardUId)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
