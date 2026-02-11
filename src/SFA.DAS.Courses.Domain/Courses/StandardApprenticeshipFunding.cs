namespace SFA.DAS.Courses.Domain.Courses
{
    public class StandardApprenticeshipFunding : ApprenticeshipFundingBase
    {
        public static explicit operator StandardApprenticeshipFunding(Entities.ApprenticeshipFunding apprenticeshipFunding)
        {
            if (apprenticeshipFunding == null)
            {
                return null;
            }

            return new StandardApprenticeshipFunding
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
