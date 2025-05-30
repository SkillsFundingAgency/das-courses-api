using System;
using CsvHelper.Configuration.Attributes;

namespace SFA.DAS.Courses.Domain.ImportTypes
{
    public class ApprenticeshipFundingCsv
    {
        public string ApprenticeshipType { get; set; }
        public int ApprenticeshipCode { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public decimal MaxEmployerLevyCap { get; set; }
        public decimal Duration { get; set; }
        [Name("1618Incentive")]
        public decimal? Incentive1618 { get; set; }
        [Name("1618ProviderAdditionalPayment")]
        public decimal? ProviderAdditionalPayment1618 { get; set; }
        [Name("1618EmployerAdditionalPayment")]
        public decimal? EmployerAdditionalPayment1618 { get; set; }
        public decimal? CareLeaverAdditionalPayment { get; set; }
        public decimal? FoundationAppFirstEmpPayment { get; set; }
        public decimal? FoundationAppSecondEmpPayment { get; set; }
        public decimal? FoundationAppThirdEmpPayment { get; set; }
    }
}
