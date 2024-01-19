using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class SectorSubjectAreaTier1Base
    {
        public int SectorSubjectAreaTier1 { get; set; }
        public string SectorSubjectAreaTier1Desc { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
