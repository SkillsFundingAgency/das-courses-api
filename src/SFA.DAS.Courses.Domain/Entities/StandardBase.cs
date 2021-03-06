﻿using System;
using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardBase
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Status { get; set; }
        public DateTime? VersionEarliestStartDate { get; set; }
        public DateTime? VersionLatestStartDate { get; set; }
        public DateTime? VersionLatestEndDate { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public int ProposedTypicalDuration { get; set; }
        public int ProposedMaxFunding { get; set; }
        public decimal Version { get; set; }
        public string OverviewOfRole { get; set; }
        public virtual Route Route { get ; set ; }
        public int RouteCode { get; set; }
        public string AssessmentPlanUrl { get; set; }
        public string Keywords { get; set; }
        public string TypicalJobTitles { get; set; }
        public string StandardPageUrl { get; set; }
        public string IntegratedDegree { get; set; }
        public DateTime? ApprovedForDelivery { get; set; }
        public string TrailBlazerContact { get; set; }
        public string EqaProviderName { get; set; }
        public string EqaProviderContactName { get; set; }
        public string EqaProviderContactEmail { get; set; }
        public string EqaProviderWebLink { get; set; }
        public string RegulatedBody { get; set; }
        public virtual LarsStandard LarsStandard { get; set; }
        public virtual ICollection<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Knowledge { get; set; }
        public List<string> Behaviours { get; set; }
        public List<string> Duties { get; set; }
        public bool CoreAndOptions { get; set; }
        public List<string> CoreDuties { get; set; }
        public bool IntegratedApprenticeship { get ; set ; }
        public List<string> Options { get; set; }
        public bool EPAChanged { get; set; }
    }
}
