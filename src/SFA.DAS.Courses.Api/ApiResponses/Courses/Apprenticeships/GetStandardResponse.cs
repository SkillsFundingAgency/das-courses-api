using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Extensions;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardResponse : SingleResponse
    {
        public List<StandardApprenticeshipFundingResponse> ApprenticeshipFunding { get; set; }
        public ApprenticeshipType ApprenticeshipType { get; set; }
        public int LarsCode { get; set; }
        public CourseDatesResponse StandardDates { get; set; }

        public static implicit operator GetStandardResponse(Standard source)
        {
            if (source == null)
                return null;

            return new GetStandardResponse
            {
                ApprenticeshipFunding = source.ApprenticeshipFunding?.Select(c => (StandardApprenticeshipFundingResponse)c).ToList() ?? [],
                ApprenticeshipStandardTypeCode = source.ApprenticeshipStandardTypeCode,
                ApprenticeshipType = source.ApprenticeshipType,
                ApprovalBody = source.ApprovalBody,
                CoreAndOptions = source.CoreAndOptions,
                CoreDuties = source.CoreDuties,
                CoronationEmblem = source.CoronationEmblem,
                Duties = source.Duties,
                EpaoMustBeApprovedByRegulatorBody = source.EpaoMustBeApprovedByRegulatorBody,
                IfateReferenceNumber = source.IfateReferenceNumber,
                IntegratedApprenticeship = source.IntegratedApprenticeship,
                IntegratedDegree = source.IntegratedDegree,
                IsLatestVersion = source.IsLatestVersion,
                IsRegulatedForEPAO = source.IsRegulatedForEPAO,
                IsRegulatedForProvider = source.IsRegulatedForProvider,
                Keywords = source.Keywords,
                LarsCode = int.Parse(source.LarsCode),
                Level = source.Level,
                OtherBodyApprovalRequired = source.OtherBodyApprovalRequired,
                OverviewOfRole = source.OverviewOfRole,
                Route = source.Route,
                RouteCode = source.RouteCode,
                SearchScore = source.SearchScore,
                SectorCode = source.SectorCode,
                SectorSubjectAreaTier1 = source.SectorSubjectAreaTier1,
                SectorSubjectAreaTier1Description = source.SectorSubjectAreaTier1Description,
                SectorSubjectAreaTier2 = source.SectorSubjectAreaTier2,
                SectorSubjectAreaTier2Description = source.SectorSubjectAreaTier2Description,
                Skills = source.Options.SelectManyOrEmptyList(x => x.Skills).Select(x => x.Detail).Distinct().ToList(),
                StandardDates = (CourseDatesResponse)source.StandardDates,
                StandardPageUrl = source.StandardPageUrl,
                StandardUId = source.StandardUId,
                Status = source.Status,
                Title = source.Title,
                TypicalJobTitles = source.TypicalJobTitles,
                Version = source.Version
            };
        }
    }
}
