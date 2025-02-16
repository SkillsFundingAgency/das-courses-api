using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Configuration;
public class StandardApprenticeshipTypeConfiguration : IEntityTypeConfiguration<StandardApprenticeshipType>
{
    public void Configure(EntityTypeBuilder<StandardApprenticeshipType> builder)
    {
        builder.ToTable("ApprenticeshipTypeConfig");
        builder.HasKey(x => x.IfateReferenceNumber);
        builder
            .HasOne(x => x.Standard)
            .WithOne(s => s.StandardApprenticeshipType)
            .HasForeignKey<StandardApprenticeshipType>(s => s.IfateReferenceNumber)
            .HasPrincipalKey<Domain.Entities.Standard>(s => s.IfateReferenceNumber);
    }
}
