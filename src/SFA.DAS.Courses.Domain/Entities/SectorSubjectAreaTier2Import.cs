using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class SectorSubjectAreaTier2Import : SectorSubjectAreaTier2Base
    {
        public SectorSubjectAreaTier2Import Map(SectorSubjectAreaTier2Csv source, string name)
        {
            return new SectorSubjectAreaTier2Import
            {
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                SectorSubjectAreaTier2Desc = source.SectorSubjectAreaTier2Desc,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                Name = name
            };
        }
    }
}