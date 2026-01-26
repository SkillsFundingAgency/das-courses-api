using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class ApprenticeshipFunding : ApprenticeshipFundingBase
    {
        public string LarsCode { get; set; }
        public string DurationUnits { get; set; }
        public string FundingStream { get; set; }

        public static implicit operator ApprenticeshipFunding(FundingImport FundingImport)
        {
            return new ApprenticeshipFunding
            {
                Id = Guid.NewGuid(),
                LarsCode = FundingImport.LearnAimRef,
                EffectiveFrom = FundingImport.EffectiveFrom,
                EffectiveTo = FundingImport.EffectiveTo,
                MaxEmployerLevyCap = FundingImport.RateUnWeighted,
                Duration = FundingImport.FundedGuidedLearningHours.GetValueOrDefault(0),
                DurationUnits = "Hours",
                FundingStream = FundingImport.FundingCategory
            };
        }

        public static implicit operator ApprenticeshipFunding(ApprenticeshipFundingImport apprenticeshipFundingImport)
        {
            return new ApprenticeshipFunding
            {
                Id = Guid.NewGuid(),
                LarsCode = apprenticeshipFundingImport.LarsCode.ToString(),
                EffectiveFrom = apprenticeshipFundingImport.EffectiveFrom,
                EffectiveTo = apprenticeshipFundingImport.EffectiveTo,
                MaxEmployerLevyCap = apprenticeshipFundingImport.MaxEmployerLevyCap,
                Duration = apprenticeshipFundingImport.Duration,
                DurationUnits = "Months",
                FundingStream = "Apprenticeship",
                Incentive1618 = apprenticeshipFundingImport.Incentive1618,
                ProviderAdditionalPayment1618 = apprenticeshipFundingImport.ProviderAdditionalPayment1618,
                EmployerAdditionalPayment1618 = apprenticeshipFundingImport.EmployerAdditionalPayment1618,
                CareLeaverAdditionalPayment = apprenticeshipFundingImport.CareLeaverAdditionalPayment,
                FoundationAppFirstEmpPayment = apprenticeshipFundingImport.FoundationAppFirstEmpPayment,
                FoundationAppSecondEmpPayment = apprenticeshipFundingImport.FoundationAppSecondEmpPayment,
                FoundationAppThirdEmpPayment = apprenticeshipFundingImport.FoundationAppThirdEmpPayment
            };
        }
    }
}
