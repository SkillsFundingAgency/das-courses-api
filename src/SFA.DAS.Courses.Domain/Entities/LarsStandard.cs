namespace SFA.DAS.Courses.Domain.Entities
{
    public class LarsStandard : LarsStandardBase
    {
        public static implicit operator LarsStandard(LarsStandardImport larsStandardImport)
        {
            return new LarsStandard
            {
                Id = larsStandardImport.Id,
                Version = larsStandardImport.Version,
                EffectiveFrom = larsStandardImport.EffectiveFrom,
                EffectiveTo = larsStandardImport.EffectiveTo,
                StandardId = larsStandardImport.StandardId,
                LastDateStarts = larsStandardImport.LastDateStarts
            };
        }
    }
}