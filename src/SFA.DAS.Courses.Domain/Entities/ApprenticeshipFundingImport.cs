using System;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class ApprenticeshipFundingImport : ApprenticeshipFundingBase
    {
        public int LarsCode { get; set; }

        public static implicit operator ApprenticeshipFundingImport(ApprenticeshipFundingCsv apprenticeshipFundingCsv)
        {
            return new ApprenticeshipFundingImport
            {
                Id = Guid.NewGuid(),
                EffectiveFrom = apprenticeshipFundingCsv.EffectiveFrom,
                EffectiveTo = apprenticeshipFundingCsv.EffectiveTo,
                LarsCode = apprenticeshipFundingCsv.ApprenticeshipCode,
                MaxEmployerLevyCap = Convert.ToInt32(apprenticeshipFundingCsv.MaxEmployerLevyCap),
                Duration = (int)apprenticeshipFundingCsv.Duration,
                Incentive1618 = (int)apprenticeshipFundingCsv.Incentive1618,
                ProviderAdditionalPayment1618 = (int?)apprenticeshipFundingCsv.ProviderAdditionalPayment1618,
                EmployerAdditionalPayment1618 = (int?)apprenticeshipFundingCsv.EmployerAdditionalPayment1618,
                CareLeaverAdditionalPayment = (int?)apprenticeshipFundingCsv.CareLeaverAdditionalPayment,
                FoundationAppFirstEmpPayment = (int?)apprenticeshipFundingCsv.FoundationAppFirstEmpPayment,
                FoundationAppSecondEmpPayment = (int?)apprenticeshipFundingCsv.FoundationAppSecondEmpPayment,
                FoundationAppThirdEmpPayment = (int?)apprenticeshipFundingCsv.FoundationAppThirdEmpPayment
            };
        }
    }
}
