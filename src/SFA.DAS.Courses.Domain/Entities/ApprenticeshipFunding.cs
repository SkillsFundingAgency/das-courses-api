using System;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class ApprenticeshipFunding : ApprenticeshipFundingBase
    {
        public string StandardUId { get; set; }

        public static ApprenticeshipFunding ConvertFrom(ApprenticeshipFundingImport apprenticeshipFundingImport, string standardUId)
        {
            return new ApprenticeshipFunding
            {
                Id = Guid.NewGuid(),
                StandardUId = standardUId,
                EffectiveFrom = apprenticeshipFundingImport.EffectiveFrom,
                EffectiveTo = apprenticeshipFundingImport.EffectiveTo,
                MaxEmployerLevyCap = apprenticeshipFundingImport.MaxEmployerLevyCap,
                Duration = apprenticeshipFundingImport.Duration,
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
