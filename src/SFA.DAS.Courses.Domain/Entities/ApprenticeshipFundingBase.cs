using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class ApprenticeshipFundingBase
    {
        public Guid Id { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public decimal MaxEmployerLevyCap { get; set; }
        public virtual Standard Standard { get; set; }
        public int Duration { get; set; }
        public string DurationUnits { get; set; }
        public string FundingStream { get; set; }
        public int? Incentive1618 { get; set; }
        public int? ProviderAdditionalPayment1618 { get; set; }
        public int? EmployerAdditionalPayment1618 { get; set; }
        public int? CareLeaverAdditionalPayment { get; set; }
        public int? FoundationAppFirstEmpPayment { get; set; }
        public int? FoundationAppSecondEmpPayment { get; set; }
        public int? FoundationAppThirdEmpPayment { get; set; }
    }
}
