using System;

namespace SFA.DAS.Courses.Domain.ImportTypes
{
    public class SectorSubjectAreaTier2Csv
    {
        public decimal SectorSubjectAreaTier2 { get; set; }
        public string SectorSubjectAreaTier2Desc { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}