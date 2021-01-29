namespace SFA.DAS.Courses.Domain.Entities
{
    public class ApprenticeshipFunding : ApprenticeshipFundingBase
    {
        public string StandardUId { get; set; }
        public static implicit operator ApprenticeshipFunding(ApprenticeshipFundingImport apprenticeshipFundingImport)
        {
            return new ApprenticeshipFunding
            {
                Id = apprenticeshipFundingImport.Id,
                EffectiveFrom = apprenticeshipFundingImport.EffectiveFrom,
                EffectiveTo = apprenticeshipFundingImport.EffectiveTo,
                LarsCode = apprenticeshipFundingImport.LarsCode,
                MaxEmployerLevyCap = apprenticeshipFundingImport.MaxEmployerLevyCap,
                Duration = apprenticeshipFundingImport.Duration
            };
        }
    }
}
