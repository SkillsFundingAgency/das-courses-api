using System;
using System.Globalization;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class LarsStandardImport : LarsStandardBase
    {
        public int LarsCode { get; set; }

        public static implicit operator LarsStandardImport(StandardCsv source)
        {
            if (source == null)
                return null;

            return new LarsStandardImport
            {
                Version = source.Version,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                LastDateStarts = source.LastDateStarts,
                LarsCode = source.StandardCode,
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                OtherBodyApprovalRequired = MapOtherBodyApprovalRequired(source.OtherBodyApprovalRequired),
                SectorCode = source.StandardSectorCode,
                SectorSubjectAreaTier1 = int.TryParse(source.SectorSubjectAreaTier1, NumberStyles.AllowDecimalPoint, null, out int code) ? code : default,
                ApprenticeshipStandardTypeCode = source.ApprenticeshipStandardTypeCode
            };
        }

        private static bool MapOtherBodyApprovalRequired(string source)
        {
            return string.Equals(source, "y", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
