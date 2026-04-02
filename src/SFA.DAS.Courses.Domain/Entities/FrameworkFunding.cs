namespace SFA.DAS.Courses.Domain.Entities
{
    public class FrameworkFunding : FrameworkFundingBase
    {
        public static implicit operator FrameworkFunding(FrameworkFundingImport source)
        {
            if (source == null)
                return null;

            return new FrameworkFunding
            {
                FrameworkId = source.FrameworkId,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                FundingCap = source.FundingCap
            };
        }
    }
}
