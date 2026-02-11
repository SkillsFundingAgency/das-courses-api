using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class CourseBase
    {
        public string ApprenticeshipStandardTypeCode { get; set; }
        public string ApprovalBody { get; set; }
        public string AssessmentPlanUrl { get; set; }
        public bool CoreAndOptions { get; set; }
        public List<string> CoreDuties { get; set; }
        public bool CoronationEmblem { get; set; }
        public List<string> Duties { get; set; }
        public bool EPAChanged { get; set; }
        public bool EpaoMustBeApprovedByRegulatorBody { get; set; }
        public EqaProvider EqaProvider { get; set; }
        public string IfateReferenceNumber { get; set; }
        public bool IntegratedApprenticeship { get; set; }
        public string IntegratedDegree { get; set; }
        public bool IsLatestVersion { get; set; }
        public bool IsRegulatedForEPAO { get; set; }
        public bool IsRegulatedForProvider { get; set; }
        public string Keywords { get; set; }
        public string LarsCode { get; set; }
        public int Level { get; set; }

        private List<StandardOption> _options = new List<StandardOption>();
        public List<StandardOption> Options
        {
            get => _options;
            set => _options = value ?? new List<StandardOption>();
        }
        public bool OtherBodyApprovalRequired { get; set; }
        public string OverviewOfRole { get; set; }
        public List<RelatedOccupation> RelatedOccupations { get; set; } = [];
        public string Route { get; set; }
        public int RouteCode { get; set; }
        public float? SearchScore { get; set; }
        public int SectorCode { get; set; }
        public int? SectorSubjectAreaTier1 { get; set; }
        public string SectorSubjectAreaTier1Description { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public string SectorSubjectAreaTier2Description { get; set; }
        public StandardDates StandardDates { get; set; }
        public string StandardPageUrl { get; set; }
        public string StandardUId { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string TrailBlazerContact { get; set; }
        public string TypicalJobTitles { get; set; }
        public string Version { get; set; }
        public StandardVersionDetail VersionDetail { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
    }
}
