using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class LarsStandard : LarsStandardBase
    {
        public string LarsCode { get; set; }
        public virtual SectorSubjectAreaTier2 SectorSubjectArea2 { get; set; }
        public virtual SectorSubjectAreaTier1 SectorSubjectArea1 { get; set; }
        public virtual ICollection<Standard> Standards { get; set; }

        public static implicit operator LarsStandard(LarsStandardImport source)
        {
            if (source == null)
                return null;

            return new LarsStandard
            {
                Version = source.Version,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                LarsCode = source.LarsCode.ToString(),
                LastDateStarts = source.LastDateStarts,
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                OtherBodyApprovalRequired = source.OtherBodyApprovalRequired,
                SectorCode = source.SectorCode,
                SectorSubjectAreaTier1 = source.SectorSubjectAreaTier1,
                ApprenticeshipStandardTypeCode = source.ApprenticeshipStandardTypeCode,
            };
        }
    }
}
