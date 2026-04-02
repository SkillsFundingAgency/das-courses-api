using System;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class StandardApprenticeshipFundingResponse : ApprenticeshipFundingResponseBase
    {
        public static explicit operator StandardApprenticeshipFundingResponse(StandardApprenticeshipFunding apprenticeshipFunding)
        {
            if (apprenticeshipFunding == null)
            {
                return null;
            }
            return new StandardApprenticeshipFundingResponse
            {
                EffectiveFrom = apprenticeshipFunding.EffectiveFrom,
                EffectiveTo = apprenticeshipFunding.EffectiveTo,
                MaxEmployerLevyCap = Convert.ToInt32(apprenticeshipFunding.MaxEmployerLevyCap),
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
