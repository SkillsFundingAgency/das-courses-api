using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class LarsStandard : LarsStandardBase
    {
        public virtual SectorSubjectAreaTier2 SectorSubjectArea2 { get; set; }
        public virtual SectorSubjectAreaTier1 SectorSubjectArea1 { get; set; }
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
                OtherBodyApprovalRequired = larsStandardImport.OtherBodyApprovalRequired,
                SectorCode = larsStandardImport.SectorCode,
                SectorSubjectAreaTier1 = larsStandardImport.SectorSubjectAreaTier1,
                ApprenticeshipStandardTypeCode = larsStandardImport.ApprenticeshipStandardTypeCode,
            };
        }
    }
}
