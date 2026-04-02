using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Courses
{
    public class Standard : CourseBase
    {
        public ApprenticeshipType ApprenticeshipType { get; set; }
        public IEnumerable<StandardApprenticeshipFunding> ApprenticeshipFunding { get; set; }
        public StandardVersionDetail VersionDetail { get; set; }
        public CourseDates StandardDates { get; set; }

        public static explicit operator Standard(Entities.Standard source)
        {
            if (source == null)
            {
                return null;
            }

            return new Standard
            {
                ApprenticeshipFunding = source.ApprenticeshipFunding?.Select(c => (StandardApprenticeshipFunding)c).ToList() ?? [],
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
                IsLatestVersion = source.IsLatestVersion,
                IsRegulatedForEPAO = source.IsRegulatedForEPAO,
                IsRegulatedForProvider = source.IsRegulatedForProvider,
                Keywords = source.Keywords,
                LarsCode = source.LarsCode,
                Level = source.Level,
                Options = source.Options?.Select(x => (CourseOption)x).ToList(),
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
                StandardDates = (CourseDates)source.LarsStandard,
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
