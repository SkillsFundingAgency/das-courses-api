using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class Standard
    {
        public string StandardUId { get; set; }
        public float? SearchScore { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Status { get; set; }
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

        public IEnumerable<ApprenticeshipFunding> ApprenticeshipFunding { get ; set ; }

        public StandardDates StandardDates { get ; set ; }
        public decimal SectorSubjectAreaTier2 { get ; set ; }
        public string SectorSubjectAreaTier2Description { get ; set ; }
        public bool OtherBodyApprovalRequired { get; set; }
        public string ApprovalBody { get; set; }
        public List<string> Duties { get; set; }
        public bool CoreAndOptions { get; set; }
        public string CoreDuties { get; set; }
        public bool IntegratedApprenticeship { get ; set ; }
        public List<string> Options { get; set; }
        public DateTime? EarliestStartDate { get; private set; }
        public DateTime? LatestStartDate { get; private set; }
        public DateTime? LatestEndDate { get; private set; }
        public int TypicalDuration { get; private set; }
        public int MaxFunding { get; private set; }
        public string AssessmentPlanUrl { get; private set; }
        public DateTime? ApprovedForDelivery { get; private set; }
        public string TrailBlazerContact { get; private set; }
        public string EqaProviderName { get; private set; }
        public string EqaProviderContactName { get; private set; }
        public string EqaProviderContactEmail { get; private set; }
        public string EqaProviderWebLink { get; private set; }

        public static explicit operator Standard(Entities.Standard source)
        {
            return new Standard
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
                Route = source.Sector.Route,
                AssessmentPlanUrl = source.AssessmentPlanUrl,
                ApprovedForDelivery = source.ApprovedForDelivery,
                TrailBlazerContact = source.TrailBlazerContact,
                EqaProviderName = source.EqaProviderName,
                EqaProviderContactName = source.EqaProviderContactName,
                EqaProviderContactEmail = source.EqaProviderContactEmail,
                EqaProviderWebLink = source.EqaProviderWebLink,
                TypicalJobTitles = source.TypicalJobTitles,
                Skills = source.Skills,
                Knowledge = source.Knowledge,
                Behaviours = source.Behaviours,
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                ApprenticeshipFunding = source.ApprenticeshipFunding.Select(c=>(ApprenticeshipFunding)c).ToList(),
                StandardDates = (StandardDates)source.LarsStandard,
                SectorSubjectAreaTier2 = source.LarsStandard != null ? source.LarsStandard.SectorSubjectArea.SectorSubjectAreaTier2 : 0m,
                SectorSubjectAreaTier2Description = source.LarsStandard != null ? source.LarsStandard.SectorSubjectArea.Name : "",
                OtherBodyApprovalRequired = source.LarsStandard != null ? source.LarsStandard.OtherBodyApprovalRequired : false,
                ApprovalBody = source.RegulatedBody,
                Duties = source.Duties,
                CoreAndOptions = source.CoreAndOptions,
                CoreDuties = source.CoreDuties,
                IntegratedApprenticeship = source.IntegratedApprenticeship,
                Options = source.Options
            };
        }
    }
}
