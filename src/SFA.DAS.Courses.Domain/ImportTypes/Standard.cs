﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Domain.ImportTypes
{
    [InitializeSettables]
    public class Standard
    {
        #region Common properties between standards and foundation apprenticeships

        public ApprenticeshipType ApprenticeshipType { get; set; }

        [JsonProperty("approvedForDelivery")]
        public Settable<DateTime?> ApprovedForDelivery { get; set; }

        [JsonProperty("assessmentPlanUrl")]
        public Settable<string> AssessmentPlanUrl { get; set; }

        [JsonProperty("change")]
        public Settable<string> Change { get; set; }

        [JsonProperty("createdDate")]
        public Settable<DateTime> CreatedDate { get; set; }

        [JsonProperty("eQAProvider")]
        public Settable<EqaProvider> EqaProvider { get; set; }

        [JsonProperty("earliestStartDate")]
        public Settable<DateTime?> VersionEarliestStartDate { get; set; }

        [JsonProperty("keywords")]
        public Settable<List<string>> Keywords { get; set; }

        [JsonProperty("larsCode")]
        public Settable<int> LarsCode { get; set; }

        [JsonProperty("latestEndDate")]
        public Settable<DateTime?> VersionLatestEndDate { get; set; }

        [JsonProperty("latestStartDate")]
        public Settable<DateTime?> VersionLatestStartDate { get; set; }

        [JsonProperty("level")]
        public Settable<int> Level { get; set; }

        [JsonProperty("maxFunding")]
        public Settable<int> ProposedMaxFunding { get; set; }

        [JsonProperty("overviewOfRole")]
        public Settable<string> OverviewOfRole { get; set; }

        [JsonProperty("publishDate")]
        public Settable<DateTime> PublishDate { get; set; }

        [JsonProperty("qualifications")]
        public Settable<List<Qualification>> Qualifications { get; set; }

        [JsonProperty("referenceNumber")]
        public Settable<string> ReferenceNumber { get; set; }

        [JsonProperty("regulatedBody")]
        public Settable<string> RegulatedBody { get; set; }

        [JsonProperty("regulated")]
        public Settable<bool> Regulated { get; set; }

        [JsonProperty("regulationDetail")]
        public Settable<List<RegulationDetail>> RegulationDetail { get; set; }

        [JsonProperty("route")]
        public Settable<string> Route { get; set; }

        [JsonIgnore]
        public Settable<int> RouteCode { get; set; }

        [JsonProperty("status")]
        public Settable<string> Status { get; set; }

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

        #endregion Common properties between standards and foundation apprenticeships

        #region Only applicable to standards apprenticeships

        [JsonProperty("behaviours")]
        public Settable<List<Behaviour>> Behaviours { get; set; } = new Settable<List<Behaviour>>();

        [JsonProperty("coreAndOptions")]
        public Settable<bool> CoreAndOptions { get; set; }

        [JsonProperty("coronationEmblem")]
        public Settable<bool> CoronationEmblem { get; set; }

        [JsonProperty("duties")]
        public Settable<List<Duty>> Duties { get; set; } = new();

        [JsonProperty("integratedApprenticeship")]
        public Settable<bool?> IntegratedApprenticeship { get; set; }

        [JsonProperty("integratedDegree")]
        public Settable<string> IntegratedDegree { get; set; } = new Settable<string>(string.Empty);

        [JsonProperty("knowledges")]
        public Settable<List<Knowledge>> Knowledges { get; set; } = new Settable<List<Knowledge>>();

        [JsonProperty("options")]
        public Settable<List<Option>> Options { get; set; }

        [JsonProperty("optionsUnstructuredTemplate")]
        public Settable<List<string>> OptionsUnstructuredTemplate { get; set; } = new Settable<List<string>>();

        [JsonProperty("skills")]
        public Settable<List<Skill>> Skills { get; set; } = new Settable<List<Skill>>();

        [JsonProperty("standardPageUrl")]
        public Settable<Uri> StandardPageUrl { get; set; }

        [JsonProperty("tbMainContact")]
        public Settable<string> TbMainContact { get; set; } = new Settable<string>(string.Empty);

        #endregion Standards apprenticeships

        #region Only applicable to Foundation Apprenticeships

        [JsonProperty("technicalKnowledges")]
        public Settable<List<IdDetailPair>> TechnicalKnowledges { get; set; } = new Settable<List<IdDetailPair>>();

        [JsonProperty("technicalSkills")]
        public Settable<List<IdDetailPair>> TechnicalSkills { get; set; } = new Settable<List<IdDetailPair>>();

        [JsonProperty("employabilitySkillsAndBehaviours")]
        public Settable<List<IdDetailPair>> EmployabilitySkillsAndBehaviours { get; set; } = new Settable<List<IdDetailPair>>();

        [JsonProperty("foundationApprenticeshipUrl")]
        public Settable<Uri> FoundationApprenticeshipUrl { get; set; } = new();

        [JsonProperty("assessmentChanged")]
        public Settable<bool> AssessmentChanged { get; set; } = new();

        [JsonProperty("relatedOccupations")]
        public Settable<List<RelatedOccupation>> RelatedOccupations { get; set; } = new Settable<List<RelatedOccupation>>();

        #endregion Foundation Apprenticeships
    }

    public class EqaProvider
    {
        [JsonProperty("providerName")]
        public Settable<string> ProviderName { get; set; }

        [JsonProperty("contactName")]
        public Settable<string> ContactName { get; set; }

        [JsonProperty("contactAddress")]
        public Settable<string> ContactAddress { get; set; }

        [JsonProperty("contactEmail")]
        public Settable<string> ContactEmail { get; set; }

        [JsonProperty("webLink")]
        public Settable<string> WebLink { get; set; }
    }

    public class Option
    {
        [JsonProperty("optionId")]
        public Settable<Guid> OptionId { get; set; }

        [JsonProperty("title")]
        public Settable<string> Title { get; set; }
    }

    public class Skill
    {
        [JsonProperty("skillId")]
        public Settable<Guid> SkillId { get; set; }

        [JsonProperty("detail")]
        public Settable<string> Detail { get; set; }
    }

    public class Duty
    {
        [JsonProperty("dutyID")]
        public Settable<Guid> DutyId { get; set; }

        [JsonProperty("dutyDetail")]
        public Settable<string> DutyDetail { get; set; }

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

    public class Behaviour
    {
        [JsonProperty("behaviourId")]
        public Settable<Guid> BehaviourId { get; set; }

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
        [JsonProperty("name")]
        public Settable<string> Name { get; set; }

        [JsonProperty("webLink")]
        public Settable<string> WebLink { get; set; }

        [JsonProperty("approved")]
        public Settable<bool> Approved { get; set; }
    }

    public class IdDetailPair
    {
        [JsonProperty("id")]
        public Settable<Guid> Id { get; set; }
        [JsonProperty("detail")]
        public Settable<string> Detail { get; set; }
    }

    public class RelatedOccupation
    {
        [JsonProperty("name")]
        public Settable<string> Name { get; set; } = new Settable<string>(string.Empty);
        [JsonProperty("reference")]
        public Settable<string> Reference { get; set; } = new Settable<string>(string.Empty);
    }
}
