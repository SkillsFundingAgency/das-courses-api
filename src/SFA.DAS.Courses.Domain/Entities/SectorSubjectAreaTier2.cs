using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class SectorSubjectAreaTier2 : SectorSubjectAreaTier2Base
    {
        public static implicit operator SectorSubjectAreaTier2(SectorSubjectAreaTier2Import source)
        {
            if (source == null)
                return null;

            return new SectorSubjectAreaTier2
            {
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                SectorSubjectAreaTier2Desc = source.SectorSubjectAreaTier2Desc,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                Name = source.Name
            };
        }

        public virtual ICollection<LarsStandard> LarsStandard { get ; set ; }
    }
}
