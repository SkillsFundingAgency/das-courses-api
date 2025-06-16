using System;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class ApprenticeshipFunding
    {
        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public int MaxEmployerLevyCap { get; set; }
        public int Duration { get; set; }
        public int? Incentive1618 { get; set; }
        public int? ProviderAdditionalPayment1618 { get; set; }
        public int? EmployerAdditionalPayment1618 { get; set; }
        public int? CareLeaverAdditionalPayment { get; set; }
        public int? FoundationAppFirstEmpPayment { get; set; }
        public int? FoundationAppSecondEmpPayment { get; set; }
        public int? FoundationAppThirdEmpPayment { get; set; }

        public static explicit operator ApprenticeshipFunding(Domain.Entities.ApprenticeshipFunding apprenticeshipFunding)
        {
            if (apprenticeshipFunding == null)
            {
                return null;
            }

            return new ApprenticeshipFunding
            {
                EffectiveFrom = apprenticeshipFunding.EffectiveFrom,
                EffectiveTo = apprenticeshipFunding.EffectiveTo,
                MaxEmployerLevyCap = apprenticeshipFunding.MaxEmployerLevyCap,
                Duration = apprenticeshipFunding.Duration,
                Incentive1618 = apprenticeshipFunding.Incentive1618,
                ProviderAdditionalPayment1618 = apprenticeshipFunding.ProviderAdditionalPayment1618,
                EmployerAdditionalPayment1618 = apprenticeshipFunding.EmployerAdditionalPayment1618,
                CareLeaverAdditionalPayment = apprenticeshipFunding.CareLeaverAdditionalPayment,
                FoundationAppFirstEmpPayment = apprenticeshipFunding.FoundationAppFirstEmpPayment,
                FoundationAppSecondEmpPayment = apprenticeshipFunding.FoundationAppSecondEmpPayment,
                FoundationAppThirdEmpPayment = apprenticeshipFunding.FoundationAppThirdEmpPayment
            };
        }
    }
}
