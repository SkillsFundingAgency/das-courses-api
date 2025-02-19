using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.Configuration;
public class StandardApprenticeshipTypeConfiguration : IEntityTypeConfiguration<StandardApprenticeshipType>
{
    public void Configure(EntityTypeBuilder<StandardApprenticeshipType> builder)
    {
        builder.ToTable(nameof(StandardApprenticeshipType));
        builder.HasKey(x => x.IfateReferenceNumber);
        builder
            .HasMany(x => x.Standards)
            .WithOne(s => s.StandardApprenticeshipType)
            .HasForeignKey(s => s.IfateReferenceNumber)
            .HasPrincipalKey(s => s.IfateReferenceNumber);
    }
}
