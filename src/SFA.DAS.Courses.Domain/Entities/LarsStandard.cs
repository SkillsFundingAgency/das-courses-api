using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class LarsStandard : LarsStandardBase
    {
        public virtual SectorSubjectAreaTier2 SectorSubjectArea { get; set; }
        public virtual ICollection<Standard> Standards { get; set; }

        public static implicit operator LarsStandard(LarsStandardImport larsStandardImport)
        {
            return new LarsStandard
            {
                Version = larsStandardImport.Version,
                EffectiveFrom = larsStandardImport.EffectiveFrom,
                EffectiveTo = larsStandardImport.EffectiveTo,
                LarsCode = larsStandardImport.LarsCode,
                LastDateStarts = larsStandardImport.LastDateStarts,
                SectorSubjectAreaTier2 = larsStandardImport.SectorSubjectAreaTier2,
                OtherBodyApprovalRequired = larsStandardImport.OtherBodyApprovalRequired
            };
        }
    }
}
