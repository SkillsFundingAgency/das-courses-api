using System;
using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardBase
    {
        public DateTime? ApprovedForDelivery { get; set; }

        public ApprenticeshipType ApprenticeshipType { get; set; }

        public virtual ICollection<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }

        public string AssessmentPlanUrl { get; set; }

        public CourseType CourseType { get; set; }
        public bool CoreAndOptions { get; set; }

        public List<string> CoreDuties { get; set; }

        public bool CoronationEmblem { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DurationUnits DurationUnits { get; set; }

        public List<string> Duties { get; set; }

        public string EqaProviderContactEmail { get; set; }

        public string EqaProviderContactName { get; set; }

        public string EqaProviderName { get; set; }

        public string EqaProviderWebLink { get; set; }

        public bool EPAChanged { get; set; }

        public string IfateReferenceNumber { get; set; }

        public bool IntegratedApprenticeship { get; set; }

        public string IntegratedDegree { get; set; }

        public bool IsRegulatedForEPAO { get; set; } = false;

        public bool IsRegulatedForProvider { get; set; } = false;

        public string Keywords { get; set; }

        public string LarsCode { get; set; }

        public DateTime LastUpdated { get; set; }

        public virtual LarsStandard LarsStandard { get; set; }

        public int Level { get; set; }

        public List<StandardOption> Options { get; set; }

        public string OverviewOfRole { get; set; }

        public DateTime? PublishDate { get; set; }

        public int ProposedMaxFunding { get; set; }

        public int ProposedTypicalDuration { get; set; }

        public List<string> RelatedOccupations { get; set; } = [];

        public string RegulatedBody { get; set; }

        public virtual Route Route { get; set; }

        public int RouteCode { get; set; }

        public string StandardPageUrl { get; set; }

        public string StandardUId { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public string TrailBlazerContact { get; set; }

        public string TypicalJobTitles { get; set; }

        public string Version { get; set; }

        public DateTime? VersionEarliestStartDate { get; set; }

        public DateTime? VersionLatestEndDate { get; set; }

        public DateTime? VersionLatestStartDate { get; set; }

        public int VersionMajor { get; set; }

        public int VersionMinor { get; set; }
    }
}
