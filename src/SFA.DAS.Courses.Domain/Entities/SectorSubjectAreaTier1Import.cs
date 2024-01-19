using System.Globalization;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class SectorSubjectAreaTier1Import : SectorSubjectAreaTier1Base
    {
        public static implicit operator SectorSubjectAreaTier1Import(SectorSubjectAreaTier1Csv source)
        {
            return new SectorSubjectAreaTier1Import
            {
                SectorSubjectAreaTier1 = int.TryParse(source.SectorSubjectAreaTier1, NumberStyles.AllowDecimalPoint, null, out int code) ? code : default,
                SectorSubjectAreaTier1Desc = source.SectorSubjectAreaTier1Desc,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
            };
        }
    }
}
