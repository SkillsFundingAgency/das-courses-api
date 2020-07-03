namespace SFA.DAS.Courses.Domain.Entities
{
    public class ApprenticeshipFunding : ApprenticeshipFundingBase
    {
        public static implicit operator ApprenticeshipFunding(ApprenticeshipFundingImport apprenticeshipFundingImport)
        {
            return new ApprenticeshipFunding
            {
                Id = apprenticeshipFundingImport.Id,
                EffectiveFrom = apprenticeshipFundingImport.EffectiveFrom,
                EffectiveTo = apprenticeshipFundingImport.EffectiveTo,
                StandardId = apprenticeshipFundingImport.StandardId,
                MaxEmployerLevyCap = apprenticeshipFundingImport.MaxEmployerLevyCap
            };
        }
    }
}