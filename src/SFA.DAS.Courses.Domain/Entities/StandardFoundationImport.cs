using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Domain.Entities;

public class StandardFoundationImport
{
   [JsonProperty("standardUId")]
        public string StandardUId { get; set; }

        [JsonProperty("ifateReferenceNumber")]
        public string IfateReferenceNumber { get; set; }

        [JsonProperty("larsCode")]
        public int LarsCode { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("searchScore")]
        public object SearchScore { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("coronationEmblem")]
        public bool CoronationEmblem { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("overviewOfRole")]
        public string OverviewOfRole { get; set; }

        [JsonProperty("keywords")]
        public string Keywords { get; set; }

        [JsonProperty("route")]
        public string Route { get; set; }

        [JsonProperty("routeCode")]
        public int RouteCode { get; set; }

        [JsonProperty("assessmentPlanUrl")]
        public string AssessmentPlanUrl { get; set; }

        [JsonProperty("trailBlazerContact")]
        public string TrailBlazerContact { get; set; }

        [JsonProperty("typicalJobTitles")]
        public string TypicalJobTitles { get; set; }

        [JsonProperty("skills")]
        public List<string> Skills { get; set; }

        [JsonProperty("ksbs")]
        public List<string> Ksbs { get; set; }

        [JsonProperty("standardPageUrl")]
        public string StandardPageUrl { get; set; }

        [JsonProperty("integratedDegree")]
        public string IntegratedDegree { get; set; }

        [JsonProperty("sectorSubjectAreaTier2")]
        public decimal SectorSubjectAreaTier2 { get; set; }

        [JsonProperty("sectorSubjectAreaTier2Description")]
        public string SectorSubjectAreaTier2Description { get; set; }

        [JsonProperty("sectorSubjectAreaTier1")]
        public int SectorSubjectAreaTier1 { get; set; }

        [JsonProperty("sectorSubjectAreaTier1Description")]
        public string SectorSubjectAreaTier1Description { get; set; }

        [JsonProperty("apprenticeshipFunding")]
        public ApprenticeshipFunding ApprenticeshipFunding { get; set; }

        [JsonProperty("standardDates")]
        public StandardDates StandardDates { get; set; }

        [JsonProperty("versionDetail")]
        public VersionDetail VersionDetail { get; set; }

        [JsonProperty("eqaProvider")]
        public EqaProvider EqaProvider { get; set; }

        [JsonProperty("otherBodyApprovalRequired")]
        public bool OtherBodyApprovalRequired { get; set; }

        [JsonProperty("approvalBody")]
        public string ApprovalBody { get; set; }

        [JsonProperty("duties")]
        public List<string> Duties { get; set; }

        [JsonProperty("coreAndOptions")]
        public bool CoreAndOptions { get; set; }

        [JsonProperty("coreDuties")]
        public List<string> CoreDuties { get; set; }

        [JsonProperty("integratedApprenticeship")]
        public bool IntegratedApprenticeship { get; set; }

        [JsonProperty("options")]
        public List<string> Options { get; set; }

        [JsonProperty("sectorCode")]
        public int SectorCode { get; set; }

        [JsonProperty("epaChanged")]
        public bool EpaChanged { get; set; }

        [JsonProperty("versionMajor")]
        public int VersionMajor { get; set; }

        [JsonProperty("versionMinor")]
        public int VersionMinor { get; set; }

        [JsonProperty("epaoMustBeApprovedByRegulatorBody")]
        public bool EpaoMustBeApprovedByRegulatorBody { get; set; }

        [JsonProperty("isRegulatedForProvider")]
        public bool IsRegulatedForProvider { get; set; }

        [JsonProperty("isRegulatedForEPAO")]
        public bool IsRegulatedForEpao { get; set; }

        [JsonProperty("apprenticeshipType")]
        public string ApprenticeshipType { get; set; }
}

public class EqaProvider
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("contactName")]
    public string ContactName { get; set; }

    [JsonProperty("contactEmail")]
    public string ContactEmail { get; set; }

    [JsonProperty("webLink")]
    public Uri WebLink { get; set; }
}

public class VersionDetail
{
    [JsonProperty("earliestStartDate")]
    public DateTimeOffset EarliestStartDate { get; set; }

    [JsonProperty("latestStartDate")]
    public object LatestStartDate { get; set; }

    [JsonProperty("latestEndDate")]
    public object LatestEndDate { get; set; }

    [JsonProperty("approvedForDelivery")]
    public DateTimeOffset ApprovedForDelivery { get; set; }

    [JsonProperty("proposedTypicalDuration")]
    public long ProposedTypicalDuration { get; set; }

    [JsonProperty("proposedMaxFunding")]
    public long ProposedMaxFunding { get; set; }
}
