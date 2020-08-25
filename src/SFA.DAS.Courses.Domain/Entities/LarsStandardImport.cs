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
                Id = Guid.NewGuid(),
                Version = standardCsv.Version,
                EffectiveFrom = standardCsv.EffectiveFrom,
                EffectiveTo = standardCsv.EffectiveTo,
                LastDateStarts = standardCsv.LastDateStarts,
                StandardId = standardCsv.StandardCode,
                SectorSubjectAreaTier2 = standardCsv.SectorSubjectAreaTier2,
                OtherBodyApprovalRequired = MapOtherBodyApprovalRequired(standardCsv.OtherBodyApprovalRequired)
            };
        }

        private static bool MapOtherBodyApprovalRequired(string source)
        {
            if (source == "Y")
                return true;
            return false;
        }
    }
}
