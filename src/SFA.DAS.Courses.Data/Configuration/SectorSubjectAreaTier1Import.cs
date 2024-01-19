using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration;

public class SectorSubjectAreaTier1Import : IEntityTypeConfiguration<Domain.Entities.SectorSubjectAreaTier1Import>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.SectorSubjectAreaTier1Import> builder)
    {
        builder.ToTable("SectorSubjectAreaTier1_Import");
        builder.HasKey(x => x.SectorSubjectAreaTier1);
    }
}
