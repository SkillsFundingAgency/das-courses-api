using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland
{
    [InitializeSettables]
    public class Apprenticeship
    {
        [JsonProperty("approvedForDelivery")]
        public Settable<DateTime?> ApprovedForDelivery { get; set; }

        [JsonProperty("assessmentPlanUrl")]
        public Settable<string> AssessmentPlanUrl { get; set; }

        [JsonProperty("behaviours")]
        public Settable<List<Behaviour>> Behaviours { get; set; } = new Settable<List<Behaviour>>();

        [JsonProperty("change")]
        public Settable<string> Change { get; set; }

        [JsonProperty("changedDate")]
        public Settable<DateTime?> ChangedDate { get; set; }

        [JsonProperty("coreAndOptions")]
        public Settable<bool> CoreAndOptions { get; set; }

        [JsonProperty("coronationEmblem")]
        public Settable<bool> CoronationEmblem { get; set; }

        [JsonProperty("createdDate")]
        public Settable<DateTime> CreatedDate { get; set; }

        [JsonProperty("duties")]
        public Settable<List<Duty>> Duties { get; set; } = new();

        [JsonProperty("earliestStartDate")]
        public Settable<DateTime?> VersionEarliestStartDate { get; set; }

        [JsonProperty("eQAProvider")]
        public Settable<ApprenticeshipEqaProvider> EqaProvider { get; set; }

        [JsonProperty("integratedApprenticeship")]
        public Settable<bool?> IntegratedApprenticeship { get; set; }

        [JsonProperty("integratedDegree")]
        public Settable<string> IntegratedDegree { get; set; } = new Settable<string>(string.Empty);

        [JsonProperty("keywords")]
        public Settable<List<string>> Keywords { get; set; }

        [JsonProperty("knowledges")]
        public Settable<List<Knowledge>> Knowledges { get; set; } = new Settable<List<Knowledge>>();

        [JsonProperty("larsCode")]
        public Settable<int> LarsCode { get; set; }

        [JsonProperty("lastUpdated")]
        public Settable<DateTime?> LastUpdated { get; set; }

        [JsonProperty("latestEndDate")]
        public Settable<DateTime?> VersionLatestEndDate { get; set; }

        [JsonProperty("latestStartDate")]
        public Settable<DateTime?> VersionLatestStartDate { get; set; }

        [JsonProperty("level")]
        public Settable<int> Level { get; set; }

        [JsonProperty("maxFunding")]
        public Settable<int> ProposedMaxFunding { get; set; }

        [JsonProperty("options")]
        public Settable<List<Option>> Options { get; set; }

        [JsonProperty("optionsUnstructuredTemplate")]
        public Settable<List<string>> OptionsUnstructuredTemplate { get; set; } = new Settable<List<string>>();

        [JsonProperty("overviewOfRole")]
        public Settable<string> OverviewOfRole { get; set; }

        [JsonProperty("publishDate")]
        public Settable<DateTime> PublishDate { get; set; }

        [JsonProperty("qualifications")]
        public Settable<List<Qualification>> Qualifications { get; set; }

        [JsonProperty("referenceNumber")]
        public Settable<string> ReferenceNumber { get; set; }

        [JsonProperty("regulated")]
        public Settable<bool> Regulated { get; set; }

        [JsonProperty("regulatedBody")]
        public Settable<string> RegulatedBody { get; set; }

        [JsonProperty("regulationDetail")]
        public Settable<List<RegulationDetail>> RegulationDetails { get; set; }

        [JsonProperty("route")]
        public Settable<string> Route { get; set; }

        [JsonIgnore]
        public Settable<int> RouteCode { get; set; }

        [JsonProperty("skills")]
        public Settable<List<Skill>> Skills { get; set; } = new Settable<List<Skill>>();

        [JsonProperty("standardPageUrl")]
        public Settable<Uri> StandardPageUrl { get; set; }

        [JsonProperty("status")]
        public Settable<string> Status { get; set; }

        [JsonProperty("tbMainContact")]
        public Settable<string> TbMainContact { get; set; } = new Settable<string>(string.Empty);

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

        public class ApprenticeshipEqaProvider
        {
            [JsonProperty("contactAddress")]
            public Settable<string> ContactAddress { get; set; }

            [JsonProperty("contactEmail")]
            public Settable<string> ContactEmail { get; set; }

            [JsonProperty("contactName")]
            public Settable<string> ContactName { get; set; }

            [JsonProperty("providerName")]
            public Settable<string> ProviderName { get; set; }

            [JsonProperty("webLink")]
            public Settable<string> WebLink { get; set; }
        }

        public class Behaviour
        {
            [JsonProperty("behaviourId")]
            public Settable<Guid> BehaviourId { get; set; }

            [JsonProperty("detail")]
            public Settable<string> Detail { get; set; }
        }

        public class Duty
        {
            [JsonProperty("dutyDetail")]
            public Settable<string> DutyDetail { get; set; }

            [JsonProperty("dutyID")]
            public Settable<Guid> DutyId { get; set; }

            [JsonProperty("isThisACoreDuty"), Range(0, 1)]
            public Settable<long> IsThisACoreDuty { get; set; }

            [JsonProperty("mappedBehaviour")]
            public Settable<List<Guid>> MappedBehaviour { get; set; }

            [JsonProperty("mappedKnowledge")]
            public Settable<List<Guid>> MappedKnowledge { get; set; }

            [JsonProperty("mappedOptions")]
            public Settable<List<Guid>> MappedOptions { get; set; }

            [JsonProperty("mappedSkills")]
            public Settable<List<Guid>> MappedSkills { get; set; }
        }

        public class Knowledge
        {
            [JsonProperty("detail")]
            public Settable<string> Detail { get; set; }

            [JsonProperty("knowledgeId")]
            public Settable<Guid> KnowledgeId { get; set; }
        }

        public class Option
        {
            [JsonProperty("optionId")]
            public Settable<Guid> OptionId { get; set; }

            [JsonProperty("title")]
            public Settable<string> Title { get; set; }
        }

        public class Qualification
        {
            [JsonProperty("anyAdditionalInformation")]
            public Settable<string> AnyAdditionalInformation { get; set; }

            [JsonProperty("level")]
            public Settable<string> Level { get; set; }

            [JsonProperty("qualificationId")]
            public Settable<string> QualificationId { get; set; }

            [JsonProperty("title")]
            public Settable<string> Title { get; set; }
        }

        public class RegulationDetail
        {
            [JsonProperty("approved")]
            public Settable<bool> Approved { get; set; }

            [JsonProperty("name")]
            public Settable<string> Name { get; set; }

            [JsonProperty("webLink")]
            public Settable<string> WebLink { get; set; }
        }

        public class Skill
        {
            [JsonProperty("detail")]
            public Settable<string> Detail { get; set; }

            [JsonProperty("skillId")]
            public Settable<Guid> SkillId { get; set; }
        }
    }
}
