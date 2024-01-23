using System;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Api.ApiResponses;

public class GetSectorSubjectAreaTier1Response
{
    public int SectorSubjectAreaTier1 { get; set; }
    public string SectorSubjectAreaTier1Desc { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }

    public static implicit operator GetSectorSubjectAreaTier1Response(SectorSubjectAreaTier1 source)
        => new()
        {
            SectorSubjectAreaTier1 = source.SectorSubjectAreaTier1,
            SectorSubjectAreaTier1Desc = source.SectorSubjectAreaTier1Desc,
            EffectiveFrom = source.EffectiveFrom,
            EffectiveTo = source.EffectiveTo
        };
}
