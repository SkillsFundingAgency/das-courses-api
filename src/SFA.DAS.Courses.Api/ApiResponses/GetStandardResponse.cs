using System.Collections.Generic;
using System.Linq;
using Standard = SFA.DAS.Courses.Domain.Courses.Standard;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardResponse
    {
        public int Id { get; set; }
        public float? SearchScore { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public decimal Version { get; set; }
        
        public string OverviewOfRole { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        public string TypicalJobTitles { get; set; }
        public string CoreSkillsCount { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Knowledge { get; set; }
        public List<string> Behaviours { get; set; }
        public string StandardPageUrl { get; set; }
        public string IntegratedDegree { get; set; }
        public decimal SectorSubjectAreaTier2 { get ; set ; }
        public string SectorSubjectAreaTier2Description { get ; set ; }
        public List<ApprenticeshipFundingResponse> ApprenticeshipFunding { get ; set ; }

        public StandardDatesResponse StandardDates { get ; set ; }
        public bool OtherBodyApprovalRequired { get; set; }
        public string ApprovalBody { get; set; }
        public List<string> Duties { get; set; }
        public bool CoreAndOptions { get; set; }
        public string CoreDuties { get; set; }

        public static implicit operator GetStandardResponse(Standard source)
        {
            return new GetStandardResponse
            {
                Id = source.Id,
                SearchScore = source.SearchScore,
                Title = source.Title,
                Level = source.Level,
                Version = source.Version,
                OverviewOfRole = source.OverviewOfRole,
                Keywords = source.Keywords,
                Route = source.Route,
                TypicalJobTitles = source.TypicalJobTitles,
                CoreSkillsCount = source.CoreSkillsCount,
                Skills = source.Skills,
                Knowledge = source.Knowledge,
                Behaviours = source.Behaviours,
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                ApprenticeshipFunding = source.ApprenticeshipFunding.Select(c=>(ApprenticeshipFundingResponse)c).ToList(),
                StandardDates = source.StandardDates,
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                SectorSubjectAreaTier2Description = source.SectorSubjectAreaTier2Description,
                OtherBodyApprovalRequired = source.OtherBodyApprovalRequired,
                ApprovalBody = source.ApprovalBody,
                Duties = source.Duties,
                CoreAndOptions = source.CoreAndOptions,
                CoreDuties = source.CoreDuties
            };
        }
    }
}

