using System;
using System.Globalization;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class LarsStandardImport : LarsStandardBase
    {
        public static implicit operator LarsStandardImport(StandardCsv standardCsv)
        {
            return new LarsStandardImport
            {
                Version = standardCsv.Version,
                EffectiveFrom = standardCsv.EffectiveFrom,
                EffectiveTo = standardCsv.EffectiveTo,
                LastDateStarts = standardCsv.LastDateStarts,
                LarsCode = standardCsv.StandardCode,
                SectorSubjectAreaTier2 = standardCsv.SectorSubjectAreaTier2,
                OtherBodyApprovalRequired = MapOtherBodyApprovalRequired(standardCsv.OtherBodyApprovalRequired),
                SectorCode = standardCsv.StandardSectorCode,
                SectorSubjectAreaTier1 = int.TryParse(standardCsv.SectorSubjectAreaTier1, NumberStyles.AllowDecimalPoint, null, out int code) ? code : -1
            };
        }

        private static bool MapOtherBodyApprovalRequired(string source)
        {
            return string.Equals(source, "y", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
