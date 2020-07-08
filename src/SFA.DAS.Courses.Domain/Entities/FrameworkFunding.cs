namespace SFA.DAS.Courses.Domain.Entities
{
    public class FrameworkFunding : FrameworkFundingBase
    {
        public static implicit operator FrameworkFunding(FrameworkFundingImport frameworkFundingImport)
        {
            return new FrameworkFunding
            {
                FrameworkId = frameworkFundingImport.FrameworkId,
                EffectiveFrom = frameworkFundingImport.EffectiveFrom,
                EffectiveTo = frameworkFundingImport.EffectiveTo,
                FundingCap = frameworkFundingImport.FundingCap
            };
        }
    }
}