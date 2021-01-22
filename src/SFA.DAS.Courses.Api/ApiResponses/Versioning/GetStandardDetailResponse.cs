using System;
using System.Collections.Generic;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardDetail;

namespace SFA.DAS.Courses.Api.ApiResponses.Versioning
{
    public class GetStandardDetailResponse
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string ReferenceNumber { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Version { get; set; }
        public DateTime? EarliestStartDate { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public DateTime? LatestEndDate { get; set; }
        public string OverviewOfRole { get; set; }
        public int? Level { get; set; }
        public int? TypicalDuration { get; set; }
        public decimal? MaxFunding { get; set; }
        public string Route { get; set; }
        public string SSA1 { get; set; }
        public string SSA2 { get; set; }
        public bool? IntegratedApprenticeship { get; set; }
        public string IntegratedDegree { get; set; }
        public bool? CoreAndOptions { get; set; }
        public List<string> TypicalJobTitles { get; set; }
        public string StandardPageUrl { get; set; }
        public string AssessmentPlanUrl { get; set; }
        public string StandardInformation { get; set; }
        public List<string> Keywords { get; set; }
        public List<string> Knowledges { get; set; }
        public List<string> Behaviours { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Options { get; set; }

        public static implicit operator GetStandardDetailResponse(GetStandardDetailResult result)
        {
            var standard = result.Standard;
            var standardAdditionalInformation = result.StandardAdditionalInformation;

            return new GetStandardDetailResponse
            {
                StandardUId = standard.StandardUId,
                LarsCode = standard.LarsCode,
                ReferenceNumber = standard.ReferenceNumber,
                Title = standard.Title,
                Status = standard.Status,
                Version = standard.Version,
                EarliestStartDate = standard.EarliestStartDate,
                LatestStartDate = standard.LatestStartDate,
                LatestEndDate = standard.LatestEndDate,
                OverviewOfRole = standard.OverviewOfRole,
                Level = standard.Level,
                TypicalDuration = standard.TypicalDuration,
                MaxFunding = standard.MaxFunding,
                Route = standard.Route,
                SSA1 = standard.SSA1,
                SSA2 = standard.SSA2,
                IntegratedApprenticeship = standard.IntegratedApprenticeship,
                IntegratedDegree = standard.IntegratedDegree,
                CoreAndOptions = standard.CoreAndOptions,
                TypicalJobTitles = standard.TypicalJobTitles,
                StandardPageUrl = standard.StandardPageUrl,
                AssessmentPlanUrl = standard.AssessmentPlanUrl,
                StandardInformation = standardAdditionalInformation.StandardInformation,
                Keywords = standardAdditionalInformation.Keywords,
                Knowledges = standardAdditionalInformation.Knowledges,
                Behaviours = standardAdditionalInformation.Behaviours,
                Skills = standardAdditionalInformation.Skills,
                Options = standardAdditionalInformation.Options,
            };
        }
    }
}
