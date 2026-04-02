using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
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
        public Settable<DateTime?> ChangedDate { get; set; }

        [JsonProperty("createdDate")]
        public Settable<DateTime> CreatedDate { get; set; }

        [JsonProperty("earliestStartDate")]
        public Settable<DateTime?> VersionEarliestStartDate { get; set; }

        [JsonProperty("entryRequirements")]
        public Settable<string> EntryRequirements { get; set; }

        [JsonProperty("greenJobTitles")]
        public Settable<List<string>> GreenJobTitles { get; set; }

        [JsonProperty("keywords")]
        public Settable<List<string>> Keywords { get; set; }

        [JsonProperty("lastUpdated")]
        public Settable<DateTime?> LastUpdated { get; set; }

        [JsonProperty("latestEndDate")]
        public Settable<DateTime?> VersionLatestEndDate { get; set; }

        [JsonProperty("latestStartDate")]
        public Settable<DateTime?> VersionLatestStartDate { get; set; }

        [JsonProperty("learningAimClassCode")]
        public Settable<string> LearningAimClassCode { get; set; }

        [JsonProperty("minimumHoursForCompliance")]
        public Settable<int> MinimumHoursForCompliance { get; set; }

        [JsonProperty("level")]
        public Settable<int> Level { get; set; }

        [JsonProperty("maxFunding")]
        public Settable<int> ProposedMaxFunding { get; set; }

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

        [JsonProperty("status")]
        public Settable<string> Status { get; set; }

        [JsonProperty("technicalKnowledges")]
        public Settable<List<IdDetailPair>> TechnicalKnowledges { get; set; }

        [JsonProperty("technicalSkills")]
        public Settable<List<IdDetailPair>> TechnicalSkills { get; set; }

        [JsonProperty("title")]
        public Settable<string> Title { get; set; }

        [JsonProperty("typicalJobTitles")]
        public Settable<List<string>> TypicalJobTitles { get; set; }

        [JsonProperty("apprenticeshipUnitUrl")]
        public Settable<Uri> ApprenticeshipUnitUrl { get; set; }

        [JsonProperty("version")]
        public Settable<string> Version { get; set; }

        [JsonProperty("versionNumber")]
        public Settable<string> VersionNumber { get; set; }

        [JsonProperty("whoIsItFor")]
        public Settable<string> WhoIsItFor { get; set; }

        [InitializeSettables]
        public class IdDetailPair
        {
            [JsonProperty("id")]
            public Settable<Guid> Id { get; set; }

            [JsonProperty("detail")]
            public Settable<string> Detail { get; set; }
        }

        [InitializeSettables]
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
