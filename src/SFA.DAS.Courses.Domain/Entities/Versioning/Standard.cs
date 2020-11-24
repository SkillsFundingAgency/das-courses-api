using System;
using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Entities.Versioning
{
    public class Standard
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
        public string SSA1 { get; set; }
        public string SSA2 { get; set; }
        public bool? IntegratedApprenticeship { get; set; }
        public string IntegratedDegree { get; set; }
        public bool? CoreAndOptions { get; set; }
        public List<string> TypicalJobTitles { get; set; }
        public string StandardPageUrl { get; set; }

        public static implicit operator Standard(StandardStaging importedStandard) => new Standard
        {
            StandardUId = importedStandard.StandardUId,
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
            SSA1 = importedStandard.SSA1,
            SSA2 = importedStandard.SSA2,
            IntegratedApprenticeship = importedStandard.IntegratedApprenticeship,
            IntegratedDegree = importedStandard.IntegratedDegree,
            CoreAndOptions = importedStandard.CoreAndOptions,
            TypicalJobTitles = importedStandard.TypicalJobTitles,
            StandardPageUrl = importedStandard.StandardPageUrl
        };

        //public void Update(Standard importedStandard)
        //{
        //    LarsCode = importedStandard.LarsCode;
        //    ReferenceNumber = importedStandard.ReferenceNumber;
        //    Title = importedStandard.Title;
        //    Status = importedStandard.Status;
        //    Version = importedStandard.Version;
        //    EarlierStartDate = importedStandard.EarlierStartDate;
        //    LatestStartDate = importedStandard.LatestStartDate;
        //    LatestEndDate = importedStandard.LatestEndDate;
        //    OverviewOfRole = importedStandard.OverviewOfRole;
        //    Level = importedStandard.Level;
        //    TypicalDuration = importedStandard.TypicalDuration;
        //    MaxFunding = importedStandard.MaxFunding;
        //    Route = importedStandard.Route;
        //    Keywords = importedStandard.Keywords;
        //    SSA1 = importedStandard.SSA1;
        //    SSA2 = importedStandard.SSA2;
        //    Knowledges = importedStandard.Knowledges;
        //    Behaviours = importedStandard.Behaviours;
        //    Skills = importedStandard.Skills;
        //    Options = importedStandard.Options;
        //    OptionsUnstructuredTemplate = importedStandard.OptionsUnstructuredTemplate;
        //    IntegratedApprenticeship = importedStandard.IntegratedApprenticeship;
        //    IntegratedDegree = importedStandard.IntegratedDegree;
        //    CoreAndOptions = importedStandard.CoreAndOptions;
        //    TypicalJobTitles = importedStandard.TypicalJobTitles;
        //    StandardPageUrl = importedStandard.StandardPageUrl;
        //}
    }
}
