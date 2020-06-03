using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace SFA.DAS.Courses.Data.Configuration
{
    public class Standard : IEntityTypeConfiguration<Domain.Entities.Standard>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Standard> builder)
        {
            throw new System.NotImplementedException();
        }
    }
}