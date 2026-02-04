using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland
{
    [InitializeSettables]
    public class ApprenticeshipUnit
    {
        [JsonProperty("approvedForDelivery")]
        public Settable<DateTime?> ApprovedForDelivery { get; set; }

        [JsonProperty("change")]
        public Settable<string> Change { get; set; }

        [JsonProperty("changedDate")]
        public Settable<DateTime> ChangedDate { get; set; }

        [JsonProperty("createdDate")]
        public Settable<DateTime> CreatedDate { get; set; }

        [JsonProperty("earliestStartDate")]
        public Settable<DateTime?> VersionEarliestStartDate { get; set; }

        [JsonProperty("entryRequirements")]
        public Settable<string> EntryRequirements { get; set; } = new Settable<string>(string.Empty);

        [JsonProperty("keywords")]
        public Settable<List<string>> Keywords { get; set; }

        [JsonProperty("knowledges")]
        public Settable<List<Knowledge>> Knowledges { get; set; } = new Settable<List<Knowledge>>();

        [JsonProperty("larsCode")]
        public Settable<string> LarsCode { get; set; }

        [JsonProperty("lastUpdated")]
        public Settable<DateTime> LastUpdated { get; set; }

        [JsonProperty("latestEndDate")]
        public Settable<DateTime?> VersionLatestEndDate { get; set; }

        [JsonProperty("latestStartDate")]
        public Settable<DateTime?> VersionLatestStartDate { get; set; }

        [JsonProperty("learningHours")]
        public Settable<int> LearningHours { get; set; }

        [JsonProperty("level")]
        public Settable<int> Level { get; set; }

        [JsonProperty("maxFunding")]
        public Settable<int> ProposedMaxFunding { get; set; }

        [JsonProperty("overviewOfRole")]
        public Settable<string> OverviewOfRole { get; set; }

        [JsonProperty("publishDate")]
        public Settable<DateTime> PublishDate { get; set; }

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

        [JsonProperty("skills")]
        public Settable<List<Skill>> Skills { get; set; } = new Settable<List<Skill>>();

        [JsonProperty("status")]
        public Settable<string> Status { get; set; }

        [JsonProperty("title")]
        public Settable<string> Title { get; set; }

        [JsonProperty("typicalJobTitles")]
        public Settable<List<string>> TypicalJobTitles { get; set; }

        [JsonProperty("url")]
        public Settable<Uri> Url { get; set; }

        [JsonProperty("version")]
        public Settable<string> Version { get; set; }

        [JsonProperty("versionNumber")] // Not Used
        public Settable<string> VersionNumber { get; set; }

        public class Skill
        {
            [JsonProperty("skillId")]
            public Settable<Guid> SkillId { get; set; }

            [JsonProperty("detail")]
            public Settable<string> Detail { get; set; }
        }

        public class Knowledge
        {
            [JsonProperty("knowledgeId")]
            public Settable<Guid> KnowledgeId { get; set; }

            [JsonProperty("detail")]
            public Settable<string> Detail { get; set; }
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
    }
}
