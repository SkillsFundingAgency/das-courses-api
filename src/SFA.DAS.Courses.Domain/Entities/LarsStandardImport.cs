using System;
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
                OtherBodyApprovalRequired = MapOtherBodyApprovalRequired(standardCsv.OtherBodyApprovalRequired)
            };
        }

        private static bool MapOtherBodyApprovalRequired(string source)
        {
            if (String.Equals(source, "y", StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
    }
}
