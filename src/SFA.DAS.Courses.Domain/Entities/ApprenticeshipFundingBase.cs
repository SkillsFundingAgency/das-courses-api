using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class ApprenticeshipFundingBase
    {
        public Guid Id { get; set; }
        public int StandardId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int MaxEmployerLevyCap { get; set; }
        public virtual Standard Standard { get; set; }
    }
}