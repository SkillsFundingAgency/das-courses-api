﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        public string Version { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
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
        public List<string> Skills => _options2?.SelectMany(x => x.Skills ?? new List<string>()).ToList() ?? new List<string>();
        public List<string> Knowledge => _options2?.SelectMany(x => x.Knowledge ?? new List<string>()).ToList() ?? new List<string>();
        public List<string> Behaviours => _options2?.SelectMany(x => x.Behaviours ?? new List<string>()).ToList() ?? new List<string>();
        public List<string> Duties { get; set; }
        public bool CoreAndOptions { get; set; }
        public List<string> CoreDuties { get; set; }
        public bool IntegratedApprenticeship { get ; set ; }
        //public List<StandardOption> Options { get => _options2; }
        // WIP - public properties and fields affect lots of tests
        private List<StandardOption> _options2;
        public List<StandardOption> Options2Setter { set { _options2 = value; } }
        public List<StandardOption> Options2() => _options2;
        //  /WIP
        public bool EPAChanged { get; set; }
    }
}
