using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class SectorSubjectAreaTier1 : SectorSubjectAreaTier1Base
    {
        public static implicit operator SectorSubjectAreaTier1(SectorSubjectAreaTier1Import source)
        {
            return new SectorSubjectAreaTier1
            {
                SectorSubjectAreaTier1 = source.SectorSubjectAreaTier1,
                SectorSubjectAreaTier1Desc = source.SectorSubjectAreaTier1Desc,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo
            };
        }

        public virtual ICollection<LarsStandard> LarsStandard { get; set; }
    }
}
