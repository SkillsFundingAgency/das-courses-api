using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Extensions;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardResponse : CourseResponseBase
    {
        public List<StandardApprenticeshipFundingResponse> ApprenticeshipFunding { get; set; }
        public ApprenticeshipType ApprenticeshipType { get; set; }
        public int LarsCode { get; set; }

        public static implicit operator GetStandardResponse(Standard source)
        {
            if (source == null)
                return null;

            return new GetStandardResponse
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
                TypicalJobTitles = source.TypicalJobTitles,
                Skills = source.Options.SelectManyOrEmptyList(x => x.Skills).Select(x => x.Detail).Distinct().ToList(),
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                ApprenticeshipFunding = source.ApprenticeshipFunding?.Select(c => (StandardApprenticeshipFundingResponse)c).ToList() ?? [],
                StandardDates = (StandardDatesResponse)source.StandardDates,
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
                SectorCode = source.SectorCode,
                EpaoMustBeApprovedByRegulatorBody = source.EpaoMustBeApprovedByRegulatorBody,
                ApprenticeshipType = source.ApprenticeshipType,
                ApprenticeshipStandardTypeCode = source.ApprenticeshipStandardTypeCode,
                IsLatestVersion = source.IsLatestVersion,
                IsRegulatedForProvider = source.IsRegulatedForProvider,
                IsRegulatedForEPAO = source.IsRegulatedForEPAO
            };
        }
    }
}
