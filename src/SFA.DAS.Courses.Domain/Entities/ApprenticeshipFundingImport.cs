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
                Id =  Guid.NewGuid(),
                EffectiveFrom = apprenticeshipFundingCsv.EffectiveFrom,
                EffectiveTo = apprenticeshipFundingCsv.EffectiveTo,
                LarsCode = apprenticeshipFundingCsv.ApprenticeshipCode,
                MaxEmployerLevyCap = Convert.ToInt32(apprenticeshipFundingCsv.MaxEmployerLevyCap),
                Duration = (int)apprenticeshipFundingCsv.Duration
            }; 
        }
    }
}
