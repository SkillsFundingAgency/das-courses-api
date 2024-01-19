using System;

namespace SFA.DAS.Courses.Domain.ImportTypes;

public class SectorSubjectAreaTier1Csv
{
    public string SectorSubjectAreaTier1 { get; set; }
    public string SectorSubjectAreaTier1Desc { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
}
