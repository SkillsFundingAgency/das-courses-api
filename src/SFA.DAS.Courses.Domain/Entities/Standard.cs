namespace SFA.DAS.Courses.Domain.Entities
{
    public class Standard : StandardBase
    {
        public float? SearchScore { get; set; }
        
        // Computed column
        public bool EpaoMustBeApprovedByRegulatorBody { get; set; }

        public static implicit operator Standard(StandardImport source)
        {
            if (source == null)
                return null;

            return new Standard
            {
                ApprenticeshipType = source.ApprenticeshipType,
                ApprovedForDelivery = source.ApprovedForDelivery,
                AssessmentPlanUrl = source.AssessmentPlanUrl,
                CoronationEmblem = source.CoronationEmblem,
                CoreAndOptions = source.CoreAndOptions,
                CoreDuties = source.CoreDuties,
                CourseType = source.CourseType,
                CreatedDate = source.CreatedDate,
                Duties = source.Duties,
                DurationUnits = source.DurationUnits,
                EPAChanged = source.EPAChanged,
                EqaProviderContactEmail = source.EqaProviderContactEmail,
                EqaProviderContactName = source.EqaProviderContactName,
                EqaProviderName = source.EqaProviderName,
                EqaProviderWebLink = source.EqaProviderWebLink,
                IfateReferenceNumber = source.IfateReferenceNumber,
                IntegratedApprenticeship = source.IntegratedApprenticeship,
                IntegratedDegree = source.IntegratedDegree,
                IsRegulatedForEPAO = source.IsRegulatedForEPAO,
                IsRegulatedForProvider = source.IsRegulatedForProvider,
                Keywords = source.Keywords,
                LarsCode = source.LarsCode,
                LastUpdated = source.LastUpdated,
                Level = source.Level,
                OverviewOfRole = source.OverviewOfRole,
                Options = source.Options,
                ProposedMaxFunding = source.ProposedMaxFunding,
                ProposedTypicalDuration = source.ProposedTypicalDuration,
                PublishDate = source.PublishDate,
                RegulatedBody = source.RegulatedBody,
                Route = source.Route,
                RouteCode = source.RouteCode,
                StandardPageUrl = source.StandardPageUrl,
                StandardUId = source.StandardUId,
                Status = source.Status,
                Title = source.Title,
                TrailBlazerContact = source.TrailBlazerContact,
                TypicalJobTitles = source.TypicalJobTitles,
                Version = source.Version,
                VersionEarliestStartDate = source.VersionEarliestStartDate,
                VersionLatestEndDate = source.VersionLatestEndDate,
                VersionLatestStartDate = source.VersionLatestStartDate,
                VersionMajor = source.VersionMajor,
                VersionMinor = source.VersionMinor,
                RelatedOccupations = source.RelatedOccupations
            };
        }
    }
}
