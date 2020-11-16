using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SFA.DAS.Courses.Domain.ImportTypes
{
    public class Standard
    {
        [JsonProperty("larsCode")]
        public int LarsCode { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("overviewOfRole")]
        public string OverviewOfRole { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("route")]
        public string Route { get; set; }

        [JsonIgnore]
        public Guid RouteId { get; set; }

        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; }

        [JsonProperty("jobRoles")]
        public List<string> JobRoles { get; set; }

        [JsonProperty("version")]
        public decimal? Version { get; set; }

        [JsonProperty("skills")]
        public List<Skill> Skills { get; set; }

        [JsonProperty("knowledges")]
        public List<Knowledge> Knowledge { get; set; } = new List<Knowledge>();

        [JsonProperty("behaviours")]
        public List<Behaviour> Behaviours { get; set; } = new List<Behaviour>();

        [JsonProperty("integratedDegree")]
        public string IntegratedDegree { get; set; }

        [JsonProperty("typicalJobTitles")]
        public List<string> TypicalJobTitles { get; set; }

        [JsonProperty("standardPageUrl")]
        public Uri StandardPageUrl { get; set; }

        [JsonProperty("duties")]
        public List<Duty> Duties { get; set; }

        [JsonProperty("coreAndOptions")]
        public bool CoreAndOptions { get ; set ; }

        [JsonProperty("regulatedBody")] 
        public string RegulatedBody { get; set; }
    }
    
    public class Skill
    {
        [JsonProperty("skillId")]
        public string SkillId { get; set; }
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
        public object MappedOptions { get; set; }

        [JsonProperty("mappedSkills")]
        public List<Guid> MappedSkills { get; set; }

        [JsonProperty("criteriaForMeasuringPerformance")]
        public string CriteriaForMeasuringPerformance { get; set; }
    }

    public class Behaviour
    {
        [JsonProperty("behaviourId")]
        public string BehaviourId { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }

    public class Knowledge
    {
        [JsonProperty("knowledgeId")]
        public string KnowledgeId { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }
}
