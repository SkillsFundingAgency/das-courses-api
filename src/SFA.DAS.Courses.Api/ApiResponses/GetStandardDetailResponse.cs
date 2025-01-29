using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Extensions;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardDetailResponse
    {
        public string StandardUId { get; set; }
        public string IfateReferenceNumber { get; set; }
        public int LarsCode { get; set; }
        public string Status { get; set; }
        public float? SearchScore { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public bool CoronationEmblem { get; set; }
        public string Version { get; set; }
        public string OverviewOfRole { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        public int RouteCode { get; set; }
        public string AssessmentPlanUrl { get; set; }
        public string TrailBlazerContact { get; set; }
        public string TypicalJobTitles { get; set; }
        public List<string> Skills { get; set; }
        public List<KsbResponse> Ksbs { get; set; }
        public string StandardPageUrl { get; set; }
        public string IntegratedDegree { get; set; }
        public decimal SectorSubjectAreaTier2 { get; set; }
        public string SectorSubjectAreaTier2Description { get; set; }
        public int? SectorSubjectAreaTier1 { get; set; }
        public string SectorSubjectAreaTier1Description { get; set; }

        public List<ApprenticeshipFundingResponse> ApprenticeshipFunding { get; set; }

        public StandardDatesResponse StandardDates { get; set; }

        public StandardVersionDetailResponse VersionDetail { get; set; }

        public EqaProviderResponse EqaProvider { get; set; }

        public bool OtherBodyApprovalRequired { get; set; }
        public string ApprovalBody { get; set; }
        public List<string> Duties { get; set; }
        public bool CoreAndOptions { get; set; }
        public List<string> CoreDuties { get; set; }
        public bool IntegratedApprenticeship { get; set; }
        public List<string> Options { get; set; }
        public int SectorCode { get; set; }
        public bool EPAChanged { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public bool EpaoMustBeApprovedByRegulatorBody { get; set; }
        public bool Regulated { get; set; }
        public bool IsRegulatedForProvider { get; set; }
        public bool IsRegulatedForEPAO { get; set; }

        public static explicit operator GetStandardDetailResponse(Standard source)
        {
            if (source == null) return null;

            return new GetStandardDetailResponse
            {
                StandardUId = source.StandardUId,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = source.LarsCode,
                Status = source.Status,
                SearchScore = source.SearchScore,
                Title = source.Title,
                Level = source.Level,
                CoronationEmblem = source.CoronationEmblem,
                Version = source.Version,
                OverviewOfRole = source.OverviewOfRole,
                Keywords = source.Keywords,
                Route = source.Route,
                RouteCode = source.RouteCode,
                AssessmentPlanUrl = source.AssessmentPlanUrl,
                TrailBlazerContact = source.TrailBlazerContact,
                TypicalJobTitles = source.TypicalJobTitles,
                Skills = source.Options
                               .SelectManyOrEmptyList(x => x.Skills)
                               .Select(x => x.Detail)
                               .Distinct().ToList(),
                Ksbs = source.Options
                             .SelectManyOrEmptyList(x => x.Skills.EmptyEnumerableIfNull().Union(x.Knowledge.EmptyEnumerableIfNull()).Union(x.Behaviours.EmptyEnumerableIfNull()))
                             .Select(x => (KsbResponse)x).ToList(),
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                ApprenticeshipFunding = source.ApprenticeshipFunding.Select(c => (ApprenticeshipFundingResponse)c).ToList(),
                StandardDates = (StandardDatesResponse)source.StandardDates,
                VersionDetail = (StandardVersionDetailResponse)source.VersionDetail,
                EqaProvider = (EqaProviderResponse)source.EqaProvider,
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                SectorSubjectAreaTier2Description = source.SectorSubjectAreaTier2Description,
                SectorSubjectAreaTier1 = source.SectorSubjectAreaTier1,
                SectorSubjectAreaTier1Description = source.SectorSubjectAreaTier1Description,
                OtherBodyApprovalRequired = source.OtherBodyApprovalRequired,
                ApprovalBody = source.ApprovalBody,
                Duties = source.Duties,
                CoreAndOptions = source.CoreAndOptions,
                CoreDuties = source.CoreDuties,
                IntegratedApprenticeship = source.IntegratedApprenticeship,
                Options = source.Options?.Where(x => x.IsRealOption).Select(x => x.Title).ToList(),
                SectorCode = source.SectorCode,
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

