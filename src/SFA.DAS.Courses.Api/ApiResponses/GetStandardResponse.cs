using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public decimal Version { get; set; }
        public long MaxFunding { get; set; }
        public string OverViewOfRole { get; set; }
        public int TypicalDuration { get; set; }
        public string Route { get; set; }
        public string TypicalJobTitles { get; set; }
        public string CoreSkillsCount { get; set; }
        public string StandardPageUrl { get; set; }
        public string IntegratedDegree { get; set; }
        public string Location { get ; set ; }
        public int ProvidersAtLocation { get ; set ; }

        public static implicit operator GetStandardResponse(Standard source)
        {
            return new GetStandardResponse
            {
                Id = source.Id,
                Title = source.Title,
                Level = source.Level,
                Version = source.Version,
                MaxFunding = source.MaxFunding,
                OverViewOfRole = source.OverViewOfRole,
                TypicalDuration = source.TypicalDuration,
                Route = source.Route,
                TypicalJobTitles = source.TypicalJobTitles,
                CoreSkillsCount = source.CoreSkillsCount,
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree
            };
        }
    }
}
