using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.ImportTypes
{
    public class Standard
    {
        [JsonProperty("referenceNumber")]
        public string ReferenceNumber { get; set; }

        [JsonProperty("larsCode")]
        public int LarsCode { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("earliestStartDate")]
        public DateTime? VersionEarliestStartDate { get; set; }

        [JsonProperty("latestStartDate")]
        public DateTime? VersionLatestStartDate { get; set; }

        [JsonProperty("latestEndDate")]
        public DateTime? VersionLatestEndDate { get; set; }

        [JsonProperty("overviewOfRole")]
        public string OverviewOfRole { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("coronationEmblem")]
        public bool CoronationEmblem { get; set; }

        [JsonProperty("typicalDuration")]
        public int ProposedTypicalDuration { get; set; }

        [JsonProperty("maxFunding")]
        public int ProposedMaxFunding { get; set; }

        [JsonProperty("route")]
        public string Route { get; set; }

        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; }

        [JsonProperty("jobRoles")]
        public List<string> JobRoles { get; set; }

        [JsonProperty("assessmentPlanUrl")]
        public string AssessmentPlanUrl { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("skills")]
        public List<Skill> Skills { get; set; } = new List<Skill>();

        [JsonProperty("knowledges")]
        public List<Knowledge> Knowledge { get; set; } = new List<Knowledge>();

        [JsonProperty("behaviours")]
        public List<Behaviour> Behaviours { get; set; } = new List<Behaviour>();

        [JsonProperty("eQAProvider")]
        public EqaProvider EqaProvider { get; set; }

        [JsonProperty("approvedForDelivery")]
        public DateTime? ApprovedForDelivery { get; set; }

        [JsonProperty("integratedDegree")]
        public string IntegratedDegree { get; set; }

        [JsonProperty("tbMainContact")]
        public string TbMainContact { get; set; }

        [JsonProperty("typicalJobTitles")]
        public List<string> TypicalJobTitles { get; set; }

        [JsonProperty("standardPageUrl")]
        public Uri StandardPageUrl { get; set; }

        [JsonProperty("duties")]
        public List<Duty> Duties { get; set; }

        [JsonProperty("coreAndOptions")]
        public bool CoreAndOptions { get; set; }

        [JsonProperty("regulatedBody")]
        public string RegulatedBody { get; set; }

        [JsonProperty("integratedApprenticeship")]
        public bool? IntegratedApprenticeship { get; set; }

        [JsonProperty("options")]
        public List<Option> Options { get; set; }

        [JsonProperty("optionsUnstructuredTemplate")]
        public List<string> OptionsUnstructuredTemplate { get; set; } = new List<string>();

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public int RouteCode { get; set; }
        
        [JsonIgnore]
        public string ApprenticeshipType { get; set; }

        [JsonProperty("change")]
        public string Change { get; set; }

        [JsonProperty("qualifications")]
        public List<Qualification> Qualifications { get; set; }

        [JsonProperty("regulated")]
        public bool Regulated { get; set; }

        [JsonProperty("regulationDetail")]
        public List<RegulationDetail> RegulationDetail { get; set; }
    }

    public class EqaProvider
    {
        [JsonProperty("providerName")]
        public string ProviderName { get; set; }

        [JsonProperty("contactName")]
        public string ContactName { get; set; }

        [JsonProperty("contactAddress")]
        public string ContactAddress { get; set; }

        [JsonProperty("contactEmail")]
        public string ContactEmail { get; set; }

        [JsonProperty("webLink")]
        public string WebLink { get; set; }
    }

    public class Option
    {
        [JsonProperty("optionId")]
        public Guid OptionId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class Skill
    {
        [JsonProperty("skillId")]
        public Guid SkillId { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }

    public class Duty
    {
        [JsonProperty("dutyID")]
        public Guid DutyId { get; set; }

        [JsonProperty("dutyDetail")]
        public string DutyDetail { get; set; }

        [JsonProperty("isThisACoreDuty"), Range(0, 1)]
        public long IsThisACoreDuty { get; set; }

        [JsonProperty("mappedBehaviour")]
        public List<Guid> MappedBehaviour { get; set; }

        [JsonProperty("mappedKnowledge")]
        public List<Guid> MappedKnowledge { get; set; }

        [JsonProperty("mappedOptions")]
        public List<Guid> MappedOptions { get; set; }

        [JsonProperty("mappedSkills")]
        public List<Guid> MappedSkills { get; set; }

        [JsonProperty("criteriaForMeasuringPerformance")]
        public string CriteriaForMeasuringPerformance { get; set; }
    }

    public class Behaviour
    {
        [JsonProperty("behaviourId")]
        public Guid BehaviourId { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }

    public class Knowledge
    {
        [JsonProperty("knowledgeId")]
        public Guid KnowledgeId { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }

    public class RegulationDetail
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("webLink")]
        public string WebLink { get; set; }

        [JsonProperty("approved")]
        public bool Approved { get; set; }
    }
}
