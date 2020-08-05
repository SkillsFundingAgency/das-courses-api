using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class SectorSubjectAreaTier2Import : SectorSubjectAreaTier2Base
    {
        public static implicit operator SectorSubjectAreaTier2Import(SectorSubjectAreaTier2Csv source)
        {
            return new SectorSubjectAreaTier2Import
            {
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                SectorSubjectAreaTier2Desc = source.SectorSubjectAreaTier2Desc,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo
            };
        }
    }
}