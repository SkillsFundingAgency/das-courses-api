namespace SFA.DAS.Courses.Domain.Entities
{
    public class Standard : StandardBase
    {
        public float? SearchScore { get; set; }
        
        // Computed column
        public bool EpaoMustBeApprovedByRegulatorBody { get; set; }

        public static implicit operator Standard(StandardImport import)
        {
            return new Standard
            {
                ApprenticeshipType = import.ApprenticeshipType,
                ApprovedForDelivery = import.ApprovedForDelivery,
                AssessmentPlanUrl = import.AssessmentPlanUrl,
                CoronationEmblem = import.CoronationEmblem,
                CoreAndOptions = import.CoreAndOptions,
                CoreDuties = import.CoreDuties,
                CreatedDate = import.CreatedDate,
                Duties = import.Duties,
                DurationUnits = import.DurationUnits,
                EPAChanged = import.EPAChanged,
                EqaProviderContactEmail = import.EqaProviderContactEmail,
                EqaProviderContactName = import.EqaProviderContactName,
                EqaProviderName = import.EqaProviderName,
                EqaProviderWebLink = import.EqaProviderWebLink,
                IfateReferenceNumber = import.IfateReferenceNumber,
                IntegratedApprenticeship = import.IntegratedApprenticeship,
                IntegratedDegree = import.IntegratedDegree,
                IsRegulatedForEPAO = import.IsRegulatedForEPAO,
                IsRegulatedForProvider = import.IsRegulatedForProvider,
                Keywords = import.Keywords,
                LarsCode = import.LarsCode,
                LastUpdated = import.LastUpdated,
                Level = import.Level,
                OverviewOfRole = import.OverviewOfRole,
                Options = import.Options,
                ProposedMaxFunding = import.ProposedMaxFunding,
                ProposedTypicalDuration = import.ProposedTypicalDuration,
                PublishDate = import.PublishDate,
                RegulatedBody = import.RegulatedBody,
                Route = import.Route,
                RouteCode = import.RouteCode,
                StandardPageUrl = import.StandardPageUrl,
                StandardUId = import.StandardUId,
                Status = import.Status,
                Title = import.Title,
                TrailBlazerContact = import.TrailBlazerContact,
                TypicalJobTitles = import.TypicalJobTitles,
                Version = import.Version,
                VersionEarliestStartDate = import.VersionEarliestStartDate,
                VersionLatestEndDate = import.VersionLatestEndDate,
                VersionLatestStartDate = import.VersionLatestStartDate,
                VersionMajor = import.VersionMajor,
                VersionMinor = import.VersionMinor,
                RelatedOccupations = import.RelatedOccupations
            };
        }
    }
}
