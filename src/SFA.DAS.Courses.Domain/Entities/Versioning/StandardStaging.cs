using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Entities.Versioning
{
    public class StandardStaging
    {
        public string StandardUId { get; set; }
        public int LarsCode { get; set; }
        public string ReferenceNumber { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Version { get; set; }
        public DateTime? EarlierStartDate { get; set; }
        public DateTime? LatestStartDate { get; set; }
        public DateTime? LatestEndDate { get; set; }
        public string OverviewOfRole { get; set; }
        public int? Level { get; set; }
        public int? TypicalDuration { get; set; }
        public decimal? MaxFunding { get; set; }
        public string Route { get; set; }
        public List<string> Keywords { get; set; }
        public string AssessmentPlanUrl { get; set; }
        public string SSA1 { get; set; }
        public string SSA2 { get; set; }
        public string StandardInformation { get; set; }
        public List<string> Knowledges { get; set; }
        public List<string> Behaviours { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Options { get; set; }
        public List<string> OptionsUnstructuredTemplate { get; set; }
        public bool? IntegratedApprenticeship { get; set; }
        public string IntegratedDegree { get; set; }
        public bool? CoreAndOptions { get; set; }
        public List<string> TypicalJobTitles { get; set; }
        public string StandardPageUrl { get; set; }

        public static implicit operator StandardStaging(ImportTypes.Versioning.Standard importedStandard) => new StandardStaging
        {
            StandardUId = GetStandardUId(importedStandard.ReferenceNumber.Trim(), importedStandard.Version.Trim()),
            LarsCode = importedStandard.LarsCode,
            ReferenceNumber = importedStandard.ReferenceNumber.Trim(),
            Title = importedStandard.Title,
            Status = importedStandard.Status,
            Version = importedStandard.Version.Trim(),
            EarlierStartDate = importedStandard.EarlierStartDate,
            LatestStartDate = importedStandard.LatestStartDate,
            LatestEndDate = importedStandard.LatestEndDate,
            OverviewOfRole = importedStandard.OverviewOfRole,
            Level = importedStandard.Level,
            TypicalDuration = importedStandard.TypicalDuration,
            MaxFunding = importedStandard.MaxFunding,
            Route = importedStandard.Route,
            Keywords = importedStandard.Keywords,
            AssessmentPlanUrl = importedStandard.AssessmentPlanUrl,
            SSA1 = importedStandard.SSA1,
            SSA2 = importedStandard.SSA2,
            StandardInformation = importedStandard.StandardInformation,
            Knowledges = importedStandard.Knowledges?.Select(x => x.Detail).ToList() ?? new List<string>(),
            Behaviours = importedStandard.Behaviours?.Select(x => x.Detail).ToList() ?? new List<string>(),
            Skills = importedStandard.Skills?.Select(x => x.Detail).ToList() ?? new List<string>(),
            Options = importedStandard.Options?.Select(x => x.Title).ToList() ?? new List<string>(),
            OptionsUnstructuredTemplate = importedStandard.OptionsUnstructuredTemplate,
            IntegratedApprenticeship = importedStandard.IntegratedApprenticeship,
            IntegratedDegree = importedStandard.IntegratedDegree,
            CoreAndOptions = importedStandard.CoreAndOptions,
            TypicalJobTitles = importedStandard.TypicalJobTitles,
            StandardPageUrl = importedStandard.StandardPageUrl
        };

        static string GetStandardUId(string referenceNumber, string version)
        {
            var derivedVersion = string.IsNullOrWhiteSpace(version) ? "xx" : version;
            return $"{referenceNumber}_{derivedVersion}";
        }
    }
}
