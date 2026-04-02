using System;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class ApprenticeshipFundingImport : ApprenticeshipFundingBase
    {
        public int LarsCode { get; set; }

        public static implicit operator ApprenticeshipFundingImport(ApprenticeshipFundingCsv source)
        {
            if (source == null)
                return null;

            return new ApprenticeshipFundingImport
            {
                Id = Guid.NewGuid(),
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                LarsCode = source.ApprenticeshipCode,
                MaxEmployerLevyCap = source.MaxEmployerLevyCap,
                Duration = (int)source.Duration,
                Incentive1618 = (int)source.Incentive1618,
                ProviderAdditionalPayment1618 = (int?)source.ProviderAdditionalPayment1618,
                EmployerAdditionalPayment1618 = (int?)source.EmployerAdditionalPayment1618,
                CareLeaverAdditionalPayment = (int?)source.CareLeaverAdditionalPayment,
                FoundationAppFirstEmpPayment = (int?)source.FoundationAppFirstEmpPayment,
                FoundationAppSecondEmpPayment = (int?)source.FoundationAppSecondEmpPayment,
                FoundationAppThirdEmpPayment = (int?)source.FoundationAppThirdEmpPayment
            };
        }
    }
}
