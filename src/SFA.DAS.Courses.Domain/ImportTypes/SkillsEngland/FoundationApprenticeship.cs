using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland
{
    [InitializeSettables]
    public class FoundationApprenticeship
    {
        [JsonProperty("approvedForDelivery")]
        public Settable<DateTime?> ApprovedForDelivery { get; set; }

        [JsonProperty("assessmentPlanUrl")]
        public Settable<string> AssessmentPlanUrl { get; set; }

        [JsonProperty("change")]
        public Settable<string> Change { get; set; }

        [JsonProperty("changedDate")]
        public Settable<DateTime> ChangedDate { get; set; }

        [JsonProperty("createdDate")]
        public Settable<DateTime> CreatedDate { get; set; }

        [JsonProperty("eQAProvider")]
        public Settable<FoundationApprenticeshipEqaProvider> EqaProvider { get; set; }

        [JsonProperty("earliestStartDate")]
        public Settable<DateTime?> VersionEarliestStartDate { get; set; }

        [JsonProperty("keywords")]
        public Settable<List<string>> Keywords { get; set; }

        [JsonProperty("larsCode")]
        public Settable<int> LarsCode { get; set; }
        
        [JsonProperty("lastUpdated")]
        public Settable<DateTime> LastUpdated { get; set; }

        [JsonProperty("latestEndDate")]
        public Settable<DateTime?> VersionLatestEndDate { get; set; }

        [JsonProperty("latestStartDate")]
        public Settable<DateTime?> VersionLatestStartDate { get; set; }

        [JsonProperty("level")]
        public Settable<int> Level { get; set; }

        [JsonProperty("maxFunding")]
        public Settable<int> ProposedMaxFunding { get; set; }

        [JsonProperty("overviewOfRole")]
        public Settable<string> OverviewOfRole { get; set; }

        [JsonProperty("publishDate")]
        public Settable<DateTime> PublishDate { get; set; }

        [JsonProperty("qualifications")]
        public Settable<List<Qualification>> Qualifications { get; set; }

        [JsonProperty("referenceNumber")]
        public Settable<string> ReferenceNumber { get; set; }

        [JsonProperty("regulatedBody")]
        public Settable<string> RegulatedBody { get; set; }

        [JsonProperty("regulated")]
        public Settable<bool> Regulated { get; set; }

        [JsonProperty("regulationDetail")]
        public Settable<List<RegulationDetail>> RegulationDetails { get; set; }

        [JsonProperty("route")]
        public Settable<string> Route { get; set; }

        [JsonIgnore]
        public Settable<int> RouteCode { get; set; }

        [JsonProperty("status")]
        public Settable<string> Status { get; set; }

        [JsonProperty("title")]
        public Settable<string> Title { get; set; }

        [JsonProperty("typicalDuration")]
        public Settable<int> ProposedTypicalDuration { get; set; }

        [JsonProperty("typicalJobTitles")]
        public Settable<List<string>> TypicalJobTitles { get; set; }

        [JsonProperty("version")]
        public Settable<string> Version { get; set; }

        [JsonProperty("versionNumber")]
        public Settable<string> VersionNumber { get; set; }

        [JsonProperty("technicalKnowledges")]
        public Settable<List<IdDetailPair>> TechnicalKnowledges { get; set; } = new Settable<List<IdDetailPair>>();

        [JsonProperty("technicalSkills")]
        public Settable<List<IdDetailPair>> TechnicalSkills { get; set; } = new Settable<List<IdDetailPair>>();

        [JsonProperty("employabilitySkillsAndBehaviours")]
        public Settable<List<IdDetailPair>> EmployabilitySkillsAndBehaviours { get; set; } = new Settable<List<IdDetailPair>>();

        [JsonProperty("foundationApprenticeshipUrl")]
        public Settable<Uri> FoundationApprenticeshipUrl { get; set; } = new();

        [JsonProperty("assessmentChanged")]
        public Settable<bool> AssessmentChanged { get; set; } = new();

        [JsonProperty("relatedOccupations")]
        public Settable<List<RelatedOccupation>> RelatedOccupations { get; set; } = new Settable<List<RelatedOccupation>>();

        public class FoundationApprenticeshipEqaProvider
        {
            [JsonProperty("providerName")]
            public Settable<string> ProviderName { get; set; }

            [JsonProperty("contactName")]
            public Settable<string> ContactName { get; set; }

            [JsonProperty("contactAddress")]
            public Settable<string> ContactAddress { get; set; }

            [JsonProperty("contactEmail")]
            public Settable<string> ContactEmail { get; set; }

            [JsonProperty("webLink")]
            public Settable<string> WebLink { get; set; }
        }

        public class RegulationDetail
        {
            [JsonProperty("name")]
            public Settable<string> Name { get; set; }

            [JsonProperty("webLink")]
            public Settable<string> WebLink { get; set; }

            [JsonProperty("approved")]
            public Settable<bool> Approved { get; set; }
        }

        public class Qualification
        {
            [JsonProperty("qualificationId")]
            public Settable<string> QualificationId { get; set; }

            [JsonProperty("title")]
            public Settable<string> Title { get; set; }

            [JsonProperty("level")]
            public Settable<string> Level { get; set; }

            [JsonProperty("anyAdditionalInformation")]
            public Settable<string> AnyAdditionalInformation { get; set; }
        }

        public class IdDetailPair
        {
            [JsonProperty("id")]
            public Settable<Guid> Id { get; set; }
            [JsonProperty("detail")]
            public Settable<string> Detail { get; set; }
        }

        public class RelatedOccupation
        {
            [JsonProperty("name")]
            public Settable<string> Name { get; set; } = new Settable<string>(string.Empty);
            [JsonProperty("reference")]
            public Settable<string> Reference { get; set; } = new Settable<string>(string.Empty);
        }
    }
}
