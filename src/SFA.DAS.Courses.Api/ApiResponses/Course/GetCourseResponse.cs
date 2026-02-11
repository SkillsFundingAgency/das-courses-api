using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Extensions;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetCourseResponse : CourseResponseBase
    {
        public List<CourseApprenticeshipFundingResponse> ApprenticeshipFunding { get; set; }
        public ApprenticeshipType LearningType { get; set; }
        public CourseType CourseType { get; set; }
        public string LarsCode { get; set; }

        public static implicit operator GetCourseResponse(Course source)
        {
            if (source == null)
                return null;

            return new GetCourseResponse
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
                TypicalJobTitles = source.TypicalJobTitles,
                Skills = source.Options.SelectManyOrEmptyList(x => x.Skills).Select(x => x.Detail).Distinct().ToList(),
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                ApprenticeshipFunding = source.ApprenticeshipFunding?.Select(c => (CourseApprenticeshipFundingResponse)c).ToList() ?? [],
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
                LearningType = source.LearningType,
                ApprenticeshipStandardTypeCode = source.ApprenticeshipStandardTypeCode,
                IsLatestVersion = source.IsLatestVersion,
                IsRegulatedForProvider = source.IsRegulatedForProvider,
                IsRegulatedForEPAO = source.IsRegulatedForEPAO,
                CourseType = source.CourseType
            };
        }
    }
}
