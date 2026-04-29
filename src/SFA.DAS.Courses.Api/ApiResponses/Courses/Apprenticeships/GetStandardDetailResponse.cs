using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Extensions;

using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardDetailResponse : DetailResponse
    {
        public ApprenticeshipType ApprenticeshipType { get; set; }
        public List<StandardApprenticeshipFundingResponse> ApprenticeshipFunding { get; set; }
        public List<KsbResponse> Ksbs { get; set; } = new List<KsbResponse>();
        public List<string> Skills { get; set; }
        public CourseDatesResponse StandardDates { get; set; }
        public StandardVersionDetailResponse VersionDetail { get; set; }
        public int LarsCode { get; set; }

        public static implicit operator GetStandardDetailResponse(Standard source)
        {
            if (source == null) 
                return null;

            return new GetStandardDetailResponse
            {
                StandardUId = source.StandardUId,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = int.Parse(source.LarsCode),
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
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                ApprenticeshipFunding = source.ApprenticeshipFunding.Select(c => (StandardApprenticeshipFundingResponse)c).ToList(),
                StandardDates = (CourseDatesResponse)source.StandardDates,
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
                IsLatestVersion = source.IsLatestVersion,
                IsRegulatedForProvider = source.IsRegulatedForProvider,
                IsRegulatedForEPAO = source.IsRegulatedForEPAO,
                ApprenticeshipType = source.ApprenticeshipType,
                ApprenticeshipStandardTypeCode = source.ApprenticeshipStandardTypeCode,
                RelatedOccupations = source.RelatedOccupations
            };
        }
    }
}

