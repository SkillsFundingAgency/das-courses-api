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
                Duration = apprenticeshipFundingImport.Duration
            };
        }
    }
}
