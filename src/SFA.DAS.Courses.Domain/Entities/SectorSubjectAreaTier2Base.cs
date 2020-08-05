using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class SectorSubjectAreaTier2Base
    {
        public decimal SectorSubjectAreaTier2 { get; set; }
        public string SectorSubjectAreaTier2Desc { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}