using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class Standard : StandardBase
    {
        public float? SearchScore { get; set; }

        public static implicit operator Standard(StandardImport standard)
        {
            return new Standard
            {
                StandardUId = standard.StandardUId,
                LarsCode = standard.LarsCode,
                IfateReferenceNumber = standard.IfateReferenceNumber,
                Status = standard.Status,
                VersionEarliestStartDate = standard.VersionEarliestStartDate,
                VersionLatestStartDate = standard.VersionLatestStartDate,
                VersionLatestEndDate = standard.VersionLatestEndDate,
                IntegratedDegree = standard.IntegratedDegree,
                Level = standard.Level,
                ProposedTypicalDuration = standard.ProposedTypicalDuration,
                ProposedMaxFunding = standard.ProposedMaxFunding,
                OverviewOfRole = standard.OverviewOfRole,
                StandardPageUrl = standard.StandardPageUrl,
                RouteCode = standard.RouteCode,
                AssessmentPlanUrl = standard.AssessmentPlanUrl,
                ApprovedForDelivery = standard.ApprovedForDelivery,
                TrailBlazerContact = standard.TrailBlazerContact,
                EqaProviderName = standard.EqaProviderName,
                EqaProviderContactName = standard.EqaProviderContactName,
                EqaProviderContactEmail = standard.EqaProviderContactEmail,
                EqaProviderWebLink = standard.EqaProviderWebLink,
                Title = standard.Title,
                Route = standard.Route,
                TypicalJobTitles = standard.TypicalJobTitles,
                Version = standard.Version,
                Keywords = standard.Keywords,
                RegulatedBody = standard.RegulatedBody,
                Skills = standard.Skills,
                Knowledge = standard.Knowledge,
                Behaviours = standard.Behaviours,
                Duties = standard.Duties,
                CoreAndOptions = standard.CoreAndOptions,
                CoreDuties = standard.CoreDuties,
                IntegratedApprenticeship = standard.IntegratedApprenticeship,
                Options = standard.Options.Any() ? standard.Options : standard.OptionsUnstructuredTemplate.Any() ? standard.OptionsUnstructuredTemplate : new List<string>(),
                EPAChanged = standard.EPAChanged
            };
        }
    }
}
