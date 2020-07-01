using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class Standard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public decimal Version { get; set; }
        public string OverviewOfRole { get; set; }
        public string Keywords { get; set; }
        public int TypicalDuration { get; set; }
        public string Route { get; set; }
        public string TypicalJobTitles { get; set; }
        public string CoreSkillsCount { get; set; }
        public string StandardPageUrl { get; set; }
        public string IntegratedDegree { get; set; }

        public IEnumerable<ApprenticeshipFunding> ApprenticeshipFunding { get ; set ; }

        public List<StandardDates> StandardDates { get ; set ; }

        public static implicit operator Standard(Entities.Standard source)
        {
            return new Standard
            {
                Id = source.Id,
                Title = source.Title,
                Level = source.Level,
                Version = source.Version,
                OverviewOfRole = source.OverviewOfRole,
                Keywords = source.Keywords,
                TypicalDuration = source.TypicalDuration,
                Route = source.Sector.Route,
                TypicalJobTitles = source.TypicalJobTitles,
                CoreSkillsCount = source.CoreSkillsCount,
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                ApprenticeshipFunding = source.ApprenticeshipFunding.Select(c=>(ApprenticeshipFunding)c).ToList(),
                StandardDates = source.LarsStandard.Select(c=>(StandardDates)c).ToList()
            };
        }
    }
}
