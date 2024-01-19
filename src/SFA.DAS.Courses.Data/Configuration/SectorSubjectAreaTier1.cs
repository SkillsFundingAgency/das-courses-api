using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.Courses.Data.Configuration
{
    public class SectorSubjectAreaTier1 : IEntityTypeConfiguration<Domain.Entities.SectorSubjectAreaTier1>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.SectorSubjectAreaTier1> builder)
        {
            builder.ToTable("SectorSubjectAreaTier1");
            builder.HasKey(x => x.SectorSubjectAreaTier1);

            builder.HasMany(c => c.LarsStandard)
                .WithOne(c => c.SectorSubjectArea1)
                .HasForeignKey(c => c.SectorSubjectAreaTier1)
                .HasPrincipalKey(c => c.SectorSubjectAreaTier1)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
