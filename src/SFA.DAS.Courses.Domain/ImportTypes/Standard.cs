using System;
using System.Collections.Generic;
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

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("overviewOfRole")]
        public string OverviewOfRole { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("typicalDuration")]
        public int TypicalDuration { get; set; }

        [JsonProperty("maxFunding")]
        public long MaxFunding { get; set; }

        [JsonProperty("route")]
        public string Route { get; set; }

        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; }

        [JsonProperty("jobRoles")]
        public List<string> JobRoles { get; set; }

        [JsonProperty("version")]
        public decimal Version { get; set; }

        [JsonProperty("skills")]
        public List<Skill> Skills { get; set; }

        [JsonProperty("integratedDegree")]
        public string IntegratedDegree { get; set; }

        [JsonProperty("typicalJobTitles")]
        public List<string> TypicalJobTitles { get; set; }

        [JsonProperty("standardPageUrl")]
        public Uri StandardPageUrl { get; set; }
        
        [JsonProperty("duties")]
        public List<Duty> Duties { get; set; }

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

        [JsonProperty("isThisACoreDuty")]
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
}