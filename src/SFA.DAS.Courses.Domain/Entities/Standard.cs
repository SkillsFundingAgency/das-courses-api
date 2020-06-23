using System.ComponentModel.DataAnnotations.Schema;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class Standard : StandardBase
    {
        [NotMapped]
        public float? SearchScore { get; set; }

        public static implicit operator Standard(StandardImport standard)
        {
            return new Standard
            {
                Id = standard.Id,
                CoreSkillsCount = standard.CoreSkillsCount,
                IntegratedDegree = standard.IntegratedDegree,
                Level = standard.Level,
                MaxFunding = standard.MaxFunding,
                OverviewOfRole = standard.OverviewOfRole,
                StandardPageUrl = standard.StandardPageUrl,
                Route = standard.Route,
                Title = standard.Title,
                TypicalDuration = standard.TypicalDuration,
                TypicalJobTitles = standard.TypicalJobTitles,
                Version = standard.Version,
                Keywords = standard.Keywords
            };
        }
    }
}
