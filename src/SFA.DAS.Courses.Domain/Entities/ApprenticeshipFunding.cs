using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class ApprenticeshipFunding : ApprenticeshipFundingBase
    {
        public string LarsCode { get; set; }
        public string DurationUnits { get; set; }
        public string FundingStream { get; set; }

        public static implicit operator ApprenticeshipFunding(FundingImport source)
        {
            if (source == null)
                return null;

            return new ApprenticeshipFunding
            {
                Id = Guid.NewGuid(),
                LarsCode = source.LearnAimRef,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                MaxEmployerLevyCap = source.RateUnWeighted,
                Duration = source.FundedGuidedLearningHours.GetValueOrDefault(0),
                DurationUnits = "Hours",
                FundingStream = source.FundingCategory
            };
        }

        public static implicit operator ApprenticeshipFunding(ApprenticeshipFundingImport source)
        {
            if (source == null)
                return null;

            return new ApprenticeshipFunding
            {
                Id = Guid.NewGuid(),
                LarsCode = source.LarsCode.ToString(),
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                MaxEmployerLevyCap = source.MaxEmployerLevyCap,
                Duration = source.Duration,
                DurationUnits = "Months",
                FundingStream = "Apprenticeship",
                Incentive1618 = source.Incentive1618,
                ProviderAdditionalPayment1618 = source.ProviderAdditionalPayment1618,
                EmployerAdditionalPayment1618 = source.EmployerAdditionalPayment1618,
                CareLeaverAdditionalPayment = source.CareLeaverAdditionalPayment,
                FoundationAppFirstEmpPayment = source.FoundationAppFirstEmpPayment,
                FoundationAppSecondEmpPayment = source.FoundationAppSecondEmpPayment,
                FoundationAppThirdEmpPayment = source.FoundationAppThirdEmpPayment
            };
        }
    }
}
