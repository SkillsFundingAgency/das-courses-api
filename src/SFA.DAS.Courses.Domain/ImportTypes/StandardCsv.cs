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
        public string SectorSubjectAreaTier1 { get; set; }
        public string OtherBodyApprovalRequired { get; set; }
        public int StandardSectorCode { get; set; }
        public string ApprenticeshipStandardTypeCode { get; set; }
    }
}
