namespace SFA.DAS.Courses.Domain.Entities
{
    public class FrameworkFundingImport : FrameworkFundingBase
    {
        public FrameworkFundingImport Map(ImportTypes.FundingPeriod fundingPeriod, string frameworkId)
        {
            return new FrameworkFundingImport
            {
                EffectiveFrom = fundingPeriod.EffectiveFrom,
                EffectiveTo = fundingPeriod.EffectiveTo,
                FundingCap = fundingPeriod.FundingCap,
                FrameworkId = frameworkId
                    
            };
        }
    }
}