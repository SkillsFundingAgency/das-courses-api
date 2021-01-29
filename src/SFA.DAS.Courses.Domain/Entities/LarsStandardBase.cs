using System;
using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class LarsStandardBase
    {
        public Guid Id { get; set; }
        public int LarsCode { get; set; }
        public int Version { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? LastDateStarts { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public bool OtherBodyApprovalRequired { get; set; }
    }
}
