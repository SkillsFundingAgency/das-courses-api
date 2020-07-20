using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Courses.Domain.ImportTypes
{
    public class Framework
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("FrameworkName")]
        public string FrameworkName { get; set; }

        [JsonProperty("PathwayName")]
        public string PathwayName { get; set; }

        [JsonProperty("ProgType")]
        public int ProgType { get; set; }

        [JsonProperty("FrameworkCode")]
        public int FrameworkCode { get; set; }

        [JsonProperty("PathwayCode")]
        public int PathwayCode { get; set; }

        [JsonProperty("Level")]
        public int Level { get; set; }

        [JsonProperty("TypicalLength")]
        public TypicalLength TypicalLength { get; set; }

        [JsonProperty("Duration")]
        public int Duration { get; set; }

        [JsonProperty("CurrentFundingCap")]
        public int CurrentFundingCap { get; set; }

        [JsonProperty("MaxFunding")]
        public int MaxFunding { get; set; }

        [JsonProperty("Ssa1")]
        public double Ssa1 { get; set; }

        [JsonProperty("Ssa2")]
        public double Ssa2 { get; set; }

        [JsonProperty("EffectiveFrom")]
        public DateTime EffectiveFrom { get; set; }

        [JsonProperty("EffectiveTo")]
        public DateTime EffectiveTo { get; set; }

        [JsonProperty("IsActiveFramework")]
        public bool IsActiveFramework { get; set; }

        [JsonProperty("FundingPeriods")]
        public List<FundingPeriod> FundingPeriods { get; set; }

        [JsonProperty("ProgrammeType")]
        public int ProgrammeType { get; set; }

        [JsonProperty("HasSubGroups")]
        public bool HasSubGroups { get; set; }

        [JsonProperty("ExtendedTitle")]
        public string ExtendedTitle { get; set; }
    }
    
    
    public class FundingPeriod
    {
        [JsonProperty("EffectiveFrom")]
        public DateTime EffectiveFrom { get; set; }

        [JsonProperty("EffectiveTo")]
        public DateTime? EffectiveTo { get; set; }

        [JsonProperty("FundingCap")]
        public int FundingCap { get; set; }
 
    }

    public class TypicalLength
    {
        [JsonProperty("From")]
        public int From { get; set; }

        [JsonProperty("To")]
        public int To { get; set; }

        [JsonProperty("Unit")]
        public string Unit { get; set; }
    }
}