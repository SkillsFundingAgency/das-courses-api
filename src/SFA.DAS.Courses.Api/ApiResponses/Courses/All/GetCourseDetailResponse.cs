using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Extensions;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetCourseDetailResponse : DetailResponse
    {
        public ApprenticeshipType LearningType { get; set; }
        public List<CourseApprenticeshipFundingResponse> ApprenticeshipFunding { get; set; }
        public CourseDatesResponse CourseDates { get; set; }
        public CourseVersionDetailResponse VersionDetail { get; set; }
        public CourseType CourseType { get; set; }

        public static implicit operator GetCourseDetailResponse(Course source)
        {
            if (source == null) 
                return null;

            return new GetCourseDetailResponse
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
                Ksbs = source.Options
                            .SelectManyOrEmptyList(x => x.Skills.EmptyEnumerableIfNull()
                            .Union(x.Knowledge.EmptyEnumerableIfNull())
                            .Union(x.Behaviours.EmptyEnumerableIfNull())
                            .Union(x.TechnicalSkills.EmptyEnumerableIfNull())
                            .Union(x.TechnicalKnowledges.EmptyEnumerableIfNull())
                            .Union(x.EmployabilitySkillsAndBehaviours.EmptyEnumerableIfNull()))
                            .Select(x => (KsbResponse)x).ToList(),
                StandardPageUrl = source.StandardPageUrl,
                IntegratedDegree = source.IntegratedDegree,
                ApprenticeshipFunding = source.ApprenticeshipFunding.Select(c => (CourseApprenticeshipFundingResponse)c).ToList(),
                CourseDates = (CourseDatesResponse)source.CourseDates,
                VersionDetail = (CourseVersionDetailResponse)source.VersionDetail,
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
                LearningType = source.LearningType,
                ApprenticeshipStandardTypeCode = source.ApprenticeshipStandardTypeCode,
                RelatedOccupations = source.RelatedOccupations,
                CourseType = source.CourseType
            };
        }
    }
}

