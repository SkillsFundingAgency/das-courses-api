using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class CourseApprenticeshipFunding : ApprenticeshipFundingBase
    {
        public DurationUnits DurationUnits { get; set; }

        public static explicit operator CourseApprenticeshipFunding(ApprenticeshipFunding apprenticeshipFunding)
        {
            if (apprenticeshipFunding == null)
            {
                return null;
            }

            return new CourseApprenticeshipFunding
            {
                EffectiveFrom = apprenticeshipFunding.EffectiveFrom,
                EffectiveTo = apprenticeshipFunding.EffectiveTo,
                MaxEmployerLevyCap = apprenticeshipFunding.MaxEmployerLevyCap,
                Duration = apprenticeshipFunding.Duration,
                DurationUnits = apprenticeshipFunding.DurationUnits,
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
