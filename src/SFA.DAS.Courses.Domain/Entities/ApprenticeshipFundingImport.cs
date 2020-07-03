using System;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class ApprenticeshipFundingImport : ApprenticeshipFundingBase
    {
        public static implicit operator ApprenticeshipFundingImport(ApprenticeshipFundingCsv apprenticeshipFundingCsv)
        {
            return new ApprenticeshipFundingImport
            {
                Id =  Guid.NewGuid(),
                EffectiveFrom = apprenticeshipFundingCsv.EffectiveFrom,
                EffectiveTo = apprenticeshipFundingCsv.EffectiveTo,
                StandardId = apprenticeshipFundingCsv.ApprenticeshipCode,
                MaxEmployerLevyCap = Convert.ToInt32(apprenticeshipFundingCsv.MaxEmployerLevyCap)
            }; 
                
        }
    }
}