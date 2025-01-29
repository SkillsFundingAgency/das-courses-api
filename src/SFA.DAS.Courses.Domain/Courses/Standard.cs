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
        public bool CoronationEmblem { get; set; }
        public string Version { get; set; }
        public string OverviewOfRole { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        public int RouteCode { get; set; }
        public string TypicalJobTitles { get; set; }
        public string StandardPageUrl { get; set; }
        public string IntegratedDegree { get; set; }

        public IEnumerable<ApprenticeshipFunding> ApprenticeshipFunding { get; set; }

        public StandardVersionDetail VersionDetail { get; set; }

        public EqaProvider EqaProvider { get; set; }

        public StandardDates StandardDates { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public string SectorSubjectAreaTier2Description { get; set; }
        public int? SectorSubjectAreaTier1 { get; set; }
        public string SectorSubjectAreaTier1Description { get; set; }
        public bool OtherBodyApprovalRequired { get; set; }
        public string ApprovalBody { get; set; }
        public List<string> Duties { get; set; }
        public bool CoreAndOptions { get; set; }
        public List<string> CoreDuties { get; set; }
        public bool IntegratedApprenticeship { get; set; }
        public string AssessmentPlanUrl { get; private set; }
        public string TrailBlazerContact { get; private set; }
        public int SectorCode { get; set; }
        public bool EPAChanged { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public bool EpaoMustBeApprovedByRegulatorBody { get; set; }
        public bool Regulated { get; set; }
        public bool IsRegulatedForProvider { get; set; }
        public bool IsRegulatedForEPAO { get; set; }

        private List<StandardOption> _options = new List<StandardOption>();
        public List<StandardOption> Options
        {
            get => _options;
            set => _options = value ?? new List<StandardOption>();
        }

        public static explicit operator Standard(Entities.Standard source)
        {
            if (source == null)
            {
                return null;
            }

            return new Standard
            {
                StandardUId = source.StandardUId,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = source.LarsCode,
                Status = source.Status,
                VersionDetail = (StandardVersionDetail)source,
                SearchScore = source.SearchScore,
                Title = source.Title,
                Level = source.Level,
                CoronationEmblem = source.CoronationEmblem,
                Version = source.Version,
                OverviewOfRole = source.OverviewOfRole,
                Keywords = source.Keywords,
                Route = source.Route.Name,
                RouteCode = source.Route.Id,
                AssessmentPlanUrl = source.AssessmentPlanUrl,
                TrailBlazerContact = source.TrailBlazerContact,
                EqaProvider = (EqaProvider)source,
                TypicalJobTitles = source.TypicalJobTitles,
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                ApprenticeshipFunding = source.ApprenticeshipFunding.Select(c => (ApprenticeshipFunding)c).ToList(),
                StandardDates = (StandardDates)source.LarsStandard,
                SectorSubjectAreaTier2 = source.LarsStandard != null ? source.LarsStandard.SectorSubjectArea2.SectorSubjectAreaTier2 : 0m,
                SectorSubjectAreaTier2Description = source.LarsStandard != null ? source.LarsStandard.SectorSubjectArea2.Name : "",
                SectorSubjectAreaTier1 = source.LarsStandard?.SectorSubjectArea1?.SectorSubjectAreaTier1,
                SectorSubjectAreaTier1Description = source.LarsStandard?.SectorSubjectArea1?.SectorSubjectAreaTier1Desc,
                OtherBodyApprovalRequired = source.LarsStandard != null ? source.LarsStandard.OtherBodyApprovalRequired : false,
                ApprovalBody = source.RegulatedBody,
                Duties = source.Duties,
                CoreAndOptions = source.CoreAndOptions,
                CoreDuties = source.CoreDuties,
                IntegratedApprenticeship = source.IntegratedApprenticeship,
                Options = source.Options?.Select(x => (StandardOption)x).ToList(),
                SectorCode = source.LarsStandard?.SectorCode ?? 0,
                EPAChanged = source.EPAChanged,
                VersionMajor = source.VersionMajor,
                VersionMinor = source.VersionMinor,
                EpaoMustBeApprovedByRegulatorBody = source.EpaoMustBeApprovedByRegulatorBody,
                Regulated = source.Regulated,
                IsRegulatedForProvider = source.IsRegulatedForProvider,
                IsRegulatedForEPAO = source.IsRegulatedForEPAO
            };
        }
    }
}
