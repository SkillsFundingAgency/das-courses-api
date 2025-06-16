using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class LarsStandardBase
    {
        public int LarsCode { get; set; }
        public int Version { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? LastDateStarts { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public int? SectorSubjectAreaTier1 { get; set; }
        public bool OtherBodyApprovalRequired { get; set; }
        public int SectorCode { get; set; }
        public string ApprenticeshipStandardTypeCode { get; set; }
    }
}
