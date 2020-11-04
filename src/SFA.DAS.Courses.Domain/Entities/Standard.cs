namespace SFA.DAS.Courses.Domain.Entities
{
    public class Standard : StandardBase
    {
        public float? SearchScore { get; set; }

        public static implicit operator Standard(StandardImport standard)
        {
            return new Standard
            {
                Id = standard.Id,
                CoreSkillsCount = standard.CoreSkillsCount,
                IntegratedDegree = standard.IntegratedDegree,
                Level = standard.Level,
                OverviewOfRole = standard.OverviewOfRole,
                StandardPageUrl = standard.StandardPageUrl,
                RouteId = standard.RouteId,
                Title = standard.Title,
                Sector = standard.Sector,
                TypicalJobTitles = standard.TypicalJobTitles,
                Version = standard.Version,
                Keywords = standard.Keywords,
                Skills = standard.Skills,
                Knowledge = standard.Knowledge,
                Behaviours = standard.Behaviours,
            };
        }
    }
}
