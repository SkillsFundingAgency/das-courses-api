using System;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class ApprenticeshipFundingResponse
    {
        public int MaxEmployerLevyCap { get ; set ; }

        public DateTime? EffectiveTo { get ; set ; }

        public DateTime EffectiveFrom { get ; set ; }

        public static implicit operator ApprenticeshipFundingResponse (ApprenticeshipFunding apprenticeshipFunding)
        {
            return new ApprenticeshipFundingResponse
            {
                EffectiveFrom = apprenticeshipFunding.EffectiveFrom,
                EffectiveTo = apprenticeshipFunding.EffectiveTo,
                MaxEmployerLevyCap = apprenticeshipFunding.MaxEmployerLevyCap
            };
        }
    }
}