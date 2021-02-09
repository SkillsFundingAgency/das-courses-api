using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardResponse
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Status { get; set; }
        public DateTime? EarliestStartDate { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public DateTime? LatestEndDate { get; set; }
        public float? SearchScore { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public int TypicalDuration { get; set; }
        public int MaxFunding { get; set; }
        public decimal Version { get; set; }
        public string OverviewOfRole { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        public string AssessmentPlanUrl { get; set; }
        public DateTime? ApprovedForDelivery { get; set; }
        public string TrailBlazerContact { get; set; }
        public string EqaProviderName { get; set; }
        public string EqaProviderContactName { get; set; }
        public string EqaProviderContactEmail { get; set; }
        public string EqaProviderWebLink { get; set; }
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
        public bool IntegratedApprenticeship { get ; set ; }
        public List<string> Options { get; set; }

        public static implicit operator GetStandardResponse(Standard source)
        {
            return new GetStandardResponse
            {
                StandardUId = source.StandardUId,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = source.LarsCode,
                Status = source.Status,
                EarliestStartDate = source.EarliestStartDate,
                LatestStartDate = source.LatestStartDate,
                LatestEndDate = source.LatestEndDate,
                SearchScore = source.SearchScore,
                Title = source.Title,
                Level = source.Level,
                TypicalDuration = source.TypicalDuration,
                MaxFunding = source.MaxFunding,
                Version = source.Version,
                OverviewOfRole = source.OverviewOfRole,
                Keywords = source.Keywords,
                Route = source.Route,
                AssessmentPlanUrl = source.AssessmentPlanUrl,
                ApprovedForDelivery = source.ApprovedForDelivery,
                TrailBlazerContact = source.TrailBlazerContact,
                EqaProviderName = source.EqaProviderName,
                EqaProviderContactName = source.EqaProviderContactName,
                EqaProviderContactEmail = source.EqaProviderContactEmail,
                EqaProviderWebLink = source.EqaProviderWebLink,
                TypicalJobTitles = source.TypicalJobTitles,
                CoreSkillsCount = source.CoreSkillsCount,
                Skills = source.Skills,
                Knowledge = source.Knowledge,
                Behaviours = source.Behaviours,
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                ApprenticeshipFunding = source.ApprenticeshipFunding.Select(c=>(ApprenticeshipFundingResponse)c).ToList(),
                StandardDates = (StandardDatesResponse)source.StandardDates,
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                SectorSubjectAreaTier2Description = source.SectorSubjectAreaTier2Description,
                OtherBodyApprovalRequired = source.OtherBodyApprovalRequired,
                ApprovalBody = source.ApprovalBody,
                Duties = source.Duties,
                CoreAndOptions = source.CoreAndOptions,
                CoreDuties = source.CoreDuties,
                IntegratedApprenticeship = source.IntegratedApprenticeship,
                Options = source.Options
            };
        }
    }
}

