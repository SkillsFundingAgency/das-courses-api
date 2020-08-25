using System;

namespace SFA.DAS.Courses.Domain.ImportTypes
{
    public class StandardCsv
    {
        public int StandardCode { get; set; }
        public int Version { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? LastDateStarts { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public bool OtherBodyApprovalRequired { get; set; }
    }
}
