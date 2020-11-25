using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.Courses.Data.Extensions;
using DomainEntities = SFA.DAS.Courses.Domain.Entities.Versioning;

namespace SFA.DAS.Courses.Data.Configuration.Versioning
{
    public class StandardAdditionalInformationConfiguration: IEntityTypeConfiguration<DomainEntities.StandardAdditionalInformation>
    {
        public void Configure(EntityTypeBuilder<DomainEntities.StandardAdditionalInformation> builder)
        {
            builder.ToTable(nameof(DomainEntities.StandardAdditionalInformation), "Versioning");
            builder.HasKey(x => x.StandardUId);

            builder.Property(x => x.StandardUId).HasColumnName(nameof(DomainEntities.StandardAdditionalInformation.StandardUId)).HasColumnType("varchar").HasMaxLength(40).IsRequired();
            builder.Property(x => x.Keywords).HasColumnName(nameof(DomainEntities.StandardAdditionalInformation.Keywords)).HasJsonConversion();
            builder.Property(x => x.StandardInformation).HasColumnName(nameof(DomainEntities.StandardAdditionalInformation.StandardInformation));
            builder.Property(x => x.Knowledges).HasColumnName(nameof(DomainEntities.StandardAdditionalInformation.Knowledges)).HasJsonConversion();
            builder.Property(x => x.Behaviours).HasColumnName(nameof(DomainEntities.StandardAdditionalInformation.Behaviours)).HasJsonConversion();
            builder.Property(x => x.Skills).HasColumnName(nameof(DomainEntities.StandardAdditionalInformation.Skills)).HasJsonConversion();
            builder.Property(x => x.Options).HasColumnName(nameof(DomainEntities.StandardAdditionalInformation.Options)).HasJsonConversion();
            builder.Property(x => x.OptionsUnstructuredTemplate).HasColumnName(nameof(DomainEntities.StandardAdditionalInformation.OptionsUnstructuredTemplate)).HasJsonConversion();
        }
    }
}
