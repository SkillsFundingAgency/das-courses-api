using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class FrameworkFundingBase
    {
        public int Id { get; set; }
        public string FrameworkId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int FundingCap { get; set; }
        public virtual Framework Framework { get ; set ; }
    }
}