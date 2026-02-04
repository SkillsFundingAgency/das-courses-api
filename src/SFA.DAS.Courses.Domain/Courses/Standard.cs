using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class Standard
    {
        public string ApprenticeshipStandardTypeCode { get; set; }
        public ApprenticeshipType ApprenticeshipType { get; set; }
        public IEnumerable<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }
        public string ApprovalBody { get; set; }
        public string AssessmentPlanUrl { get; private set; }
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
        public string TrailBlazerContact { get; private set; }
        public string TypicalJobTitles { get; set; }
        public string Version { get; set; }
        public StandardVersionDetail VersionDetail { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }

        public static explicit operator Standard(Entities.Standard source)
        {
            if (source == null)
            {
                return null;
            }

            return new Standard
            {
                ApprenticeshipFunding = source.ApprenticeshipFunding?.Select(c => (ApprenticeshipFunding)c).ToList() ?? [],
                ApprenticeshipStandardTypeCode = source.LarsStandard?.ApprenticeshipStandardTypeCode,
                ApprenticeshipType = source.ApprenticeshipType,
                ApprovalBody = source.RegulatedBody,
                AssessmentPlanUrl = source.AssessmentPlanUrl,
                CoreAndOptions = source.CoreAndOptions,
                CoreDuties = source.CoreDuties,
                CoronationEmblem = source.CoronationEmblem,
                Duties = source.Duties,
                EPAChanged = source.EPAChanged,
                EpaoMustBeApprovedByRegulatorBody = source.EpaoMustBeApprovedByRegulatorBody,
                EqaProvider = (EqaProvider)source,
                IfateReferenceNumber = source.IfateReferenceNumber,
                IntegratedApprenticeship = source.IntegratedApprenticeship,
                IntegratedDegree = source.IntegratedDegree,
                IsRegulatedForEPAO = source.IsRegulatedForEPAO,
                IsRegulatedForProvider = source.IsRegulatedForProvider,
                Keywords = source.Keywords,
                LarsCode = source.LarsCode,
                Level = source.Level,
                Options = source.Options?.Select(x => (StandardOption)x).ToList(),
                OtherBodyApprovalRequired = source.LarsStandard != null ? source.LarsStandard.OtherBodyApprovalRequired : false,
                OverviewOfRole = source.OverviewOfRole,
                Route = source.Route.Name,
                RouteCode = source.Route.Id,
                SearchScore = source.SearchScore,
                SectorCode = source.LarsStandard?.SectorCode ?? 0,
                SectorSubjectAreaTier1 = source.LarsStandard?.SectorSubjectArea1?.SectorSubjectAreaTier1,
                SectorSubjectAreaTier1Description = source.LarsStandard?.SectorSubjectArea1?.SectorSubjectAreaTier1Desc,
                SectorSubjectAreaTier2 = source.LarsStandard != null ? source.LarsStandard.SectorSubjectArea2.SectorSubjectAreaTier2 : 0m,
                SectorSubjectAreaTier2Description = source.LarsStandard != null ? source.LarsStandard.SectorSubjectArea2.Name : "",
                StandardDates = (StandardDates)source.LarsStandard,
                StandardPageUrl = source.StandardPageUrl,
                StandardUId = source.StandardUId,
                Status = source.Status,
                Title = source.Title,
                TrailBlazerContact = source.TrailBlazerContact,
                TypicalJobTitles = source.TypicalJobTitles,
                Version = source.Version,
                VersionDetail = (StandardVersionDetail)source,
                VersionMajor = source.VersionMajor,
                VersionMinor = source.VersionMinor
            };
        }
    }
}
