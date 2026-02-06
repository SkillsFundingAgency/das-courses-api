using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using FluentValidation;
using J2N.IO;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class RequiredFieldsPresentValidator : ValidatorBase<List<Standard>>
    {
        public RequiredFieldsPresentValidator() : base(ValidationFailureType.Error)
        {
            RuleFor(importedStandards => importedStandards)

                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        var undefinedFields = new Dictionary<string, bool>();
                        CheckValidationRules(undefinedFields, standard);

                        var missingFields = undefinedFields.Where(uf => uf.Value).Select(uf => uf.Key);
                        if (missingFields.Any())
                        {
                            context.AddFailure($"E1001: {ReferenceNumber(standard)} version {Version(standard)} has missing fields '{string.Join(",", missingFields)}'");
                        }
                    }
                });
        }

        private static void CheckValidationRules(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckApprovedForDelivery(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.FoundationApprenticeship)
                CheckAssessmentChanged(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship)
                CheckAssessmentPlanUrl(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship)
                CheckBehaviours(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship)
                CheckChange(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship)
                CheckCoreAndOptions(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship)
                CheckCoronationEmblem(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckCreatedDate(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship)
                CheckDuties(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship)
                CheckEqaProvider(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckVersionEarliestStartDate(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.FoundationApprenticeship)
                CheckEmployabilitySkillsAndBehaviours(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.FoundationApprenticeship)
                CheckFoundationApprenticeshipUrl(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckKeywords(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckKnowledges(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckLarsCode(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.ApprenticeshipUnit)
                CheckLastUpdated(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckVersionLatestEndDate(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckVersionLatestStartDate(undefinedFields, standard);
            
            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckLevel(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckProposedMaxFunding(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship)
                CheckOptionsOrOptionsUnstructuredTemplate(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckOverviewOfRole(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckPublishDate(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckReferenceNumber(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckRegulated(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckRegulatedBody(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckRegulationDetail(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.FoundationApprenticeship)
                CheckRelatedOccupations(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckRoute(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckSkills(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckStandardPageUrl(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckStatus(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.FoundationApprenticeship)
            {
                CheckTechnicalKnowledges(undefinedFields, standard);
                CheckTechnicalSkills(undefinedFields, standard);
            }

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship)
                CheckTbMainContact(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckTitle(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckProposedTypicalDuration(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckTypicalJobTitles(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship or ApprenticeshipType.ApprenticeshipUnit)
                CheckVersion(undefinedFields, standard);

            if (standard.ApprenticeshipType is ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship)
                CheckVersionNumber(undefinedFields, standard);
        }

        private static void CheckApprovedForDelivery(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.ApprovedForDelivery))}", !standard.ApprovedForDelivery.IsSet);
        }

        private static void CheckChange(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Change))}", !standard.Change.IsSet);
        }

        private static void CheckCreatedDate(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.CreatedDate))}", !standard.CreatedDate.IsSet);
        }

        private static void CheckKeywords(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Keywords))}", !standard.Keywords.IsSet);
        }

        private static void CheckLarsCode(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.LarsCode))}", !standard.LarsCode.IsSet);
        }

        private static void CheckLevel(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Level))}", !standard.Level.IsSet);
        }

        private static void CheckProposedTypicalDuration(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.ProposedTypicalDuration))}", !standard.ProposedTypicalDuration.IsSet);
        }

        private static void CheckPublishDate(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.PublishDate))}", !standard.PublishDate.IsSet);
        }

        private static void CheckReferenceNumber(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.ReferenceNumber))}", !standard.ReferenceNumber.IsSet);
        }

        private static void CheckRegulated(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Regulated))}", !standard.Regulated.IsSet);
        }

        private static void CheckRegulatedBody(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.RegulatedBody))}", !standard.RegulatedBody.IsSet);
        }

        private static void CheckRoute(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Route))}", !standard.Route.IsSet);
        }

        private static void CheckStatus(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Status))}", !standard.Status.IsSet);
        }

        private static void CheckTitle(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Title))}", !standard.Title.IsSet);
        }

        private static void CheckTypicalJobTitles(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.TypicalJobTitles))}", !standard.TypicalJobTitles.IsSet);
        }

        private static void CheckVersion(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Version))}", !standard.Version.IsSet);
        }

        private static void CheckVersionEarliestStartDate(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.VersionEarliestStartDate))}", !standard.VersionEarliestStartDate.IsSet);
        }

        private static void CheckLastUpdated(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.LastUpdated))}", !standard.LastUpdated.IsSet);
        }

        private static void CheckVersionLatestEndDate(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.VersionLatestEndDate))}", !standard.VersionLatestEndDate.IsSet);
        }

        private static void CheckVersionLatestStartDate(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.VersionLatestStartDate))}", !standard.VersionLatestStartDate.IsSet);
        }

        private static void CheckRegulationDetail(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.RegulationDetail))}", !standard.RegulationDetail.IsSet);
            if (standard.RegulationDetail.IsSet && standard.RegulationDetail.HasValue)
            {
                foreach (var (regulationDetail, index) in standard.RegulationDetail.Value.Select((s, i) => (s, i)))
                {
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.RegulationDetail))}[{index}].{GetJsonPropertyName<RegulationDetail>(nameof(RegulationDetail.Approved))}", !regulationDetail.Approved.IsSet);
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.RegulationDetail))}[{index}].{GetJsonPropertyName<RegulationDetail>(nameof(RegulationDetail.Name))}", !regulationDetail.Name.IsSet);
                }
            }
        }

        private static void CheckAssessmentPlanUrl(Dictionary<string, bool> undefinedFields, Standard standard)
        {

            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.AssessmentPlanUrl))}", !standard.AssessmentPlanUrl.IsSet);
        }

        private static void CheckEqaProvider(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}", !standard.EqaProvider.IsSet);
            if (standard.EqaProvider.IsSet && standard.EqaProvider.HasValue)
            {
                undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}.{GetJsonPropertyName<EqaProvider>(nameof(EqaProvider.ContactAddress))}", !standard.EqaProvider.Value.ContactAddress.IsSet);
                undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}.{GetJsonPropertyName<EqaProvider>(nameof(EqaProvider.ContactEmail))}", !standard.EqaProvider.Value.ContactEmail.IsSet);
                undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}.{GetJsonPropertyName<EqaProvider>(nameof(EqaProvider.ContactName))}", !standard.EqaProvider.Value.ContactName.IsSet);
                undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}.{GetJsonPropertyName<EqaProvider>(nameof(EqaProvider.ProviderName))}", !standard.EqaProvider.Value.ProviderName.IsSet);
                undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}.{GetJsonPropertyName<EqaProvider>(nameof(EqaProvider.WebLink))}", !standard.EqaProvider.Value.WebLink.IsSet);
            }
        }

        private static void CheckOverviewOfRole(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.OverviewOfRole))}", !standard.OverviewOfRole.IsSet);
        }

        private static void CheckProposedMaxFunding(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.ProposedMaxFunding))}", !standard.ProposedMaxFunding.IsSet);
        }

        private static void CheckVersionNumber(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.VersionNumber))}", !standard.VersionNumber.IsSet);
        }

        private static void CheckBehaviours(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Behaviours))}", !standard.Behaviours.IsSet);
            if (standard.Behaviours.IsSet && standard.Behaviours.HasValue)
            {
                foreach (var (behaviour, index) in standard.Behaviours.Value.Select((s, i) => (s, i)))
                {
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Behaviours))}[{index}].{GetJsonPropertyName<Behaviour>(nameof(Behaviour.BehaviourId))}", !behaviour.BehaviourId.IsSet);
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Behaviours))}[{index}].{GetJsonPropertyName<Behaviour>(nameof(Behaviour.Detail))}", !behaviour.Detail.IsSet);
                }
            }
        }

        private static void CheckCoreAndOptions(Dictionary<string, bool> undefinedFields, Standard standard)
        {

            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.CoreAndOptions))}", !standard.CoreAndOptions.IsSet);
        }

        private static void CheckCoronationEmblem(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.CoronationEmblem))}", !standard.CoronationEmblem.IsSet);
        }

        private static void CheckDuties(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}", !standard.Duties.IsSet);
            if (standard.Duties.IsSet && standard.Duties.HasValue)
            {
                foreach (var (duty, index) in standard.Duties.Value.Select((s, i) => (s, i)))
                {
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.DutyId))}", !duty.DutyId.IsSet);
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.DutyDetail))}", !duty.DutyDetail.IsSet);
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.IsThisACoreDuty))}", !duty.IsThisACoreDuty.IsSet);
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.MappedBehaviour))}", !duty.MappedBehaviour.IsSet);
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.MappedKnowledge))}", !duty.MappedKnowledge.IsSet);
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.MappedOptions))}", !duty.MappedOptions.IsSet);
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.MappedSkills))}", !duty.MappedSkills.IsSet);
                }
            }
        }

        private static void CheckKnowledges(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Knowledges))}", !standard.Knowledges.IsSet);
            if (standard.Knowledges.IsSet && standard.Knowledges.HasValue)
            {
                foreach (var (knowledge, index) in standard.Knowledges.Value.Select((s, i) => (s, i)))
                {
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Knowledges))}[{index}].{GetJsonPropertyName<Knowledge>(nameof(Knowledge.KnowledgeId))}", !knowledge.KnowledgeId.IsSet);
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Knowledges))}[{index}].{GetJsonPropertyName<Knowledge>(nameof(Knowledge.Detail))}", !knowledge.Detail.IsSet);
                }
            }
        }

        private static void CheckOptionsOrOptionsUnstructuredTemplate(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Options))} or {GetJsonPropertyName<Standard>(nameof(Standard.OptionsUnstructuredTemplate))}", !standard.Options.IsSet && !standard.OptionsUnstructuredTemplate.IsSet);
            if (standard.Options.IsSet && standard.Options.HasValue)
            {
                foreach (var (option, index) in standard.Options.Value.Select((s, i) => (s, i)))
                {
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Options))}[{index}].{GetJsonPropertyName<Option>(nameof(Option.OptionId))}", !option.OptionId.IsSet);
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Options))}[{index}].{GetJsonPropertyName<Option>(nameof(Option.Title))}", !option.Title.IsSet);
                }
            }
        }

        private static void CheckSkills(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Skills))}", !standard.Skills.IsSet);
            if (standard.Skills.IsSet && standard.Skills.HasValue)
            {
                foreach (var (skill, index) in standard.Skills.Value.Select((s, i) => (s, i)))
                {
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Skills))}[{index}].{GetJsonPropertyName<Skill>(nameof(Skill.SkillId))}", !skill.SkillId.IsSet);
                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Skills))}[{index}].{GetJsonPropertyName<Skill>(nameof(Skill.Detail))}", !skill.Detail.IsSet);
                }
            }
        }

        private static void CheckStandardPageUr(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.StandardPageUrl))}", !standard.StandardPageUrl.IsSet);
        }

        private static void CheckTbMainContact(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.TbMainContact))}", !standard.TbMainContact.IsSet);
        }

        private static void CheckAssessmentChanged(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.AssessmentChanged))}", !standard.AssessmentChanged.IsSet);
        }

        private static void CheckEmployabilitySkillsAndBehaviours(Dictionary<string, bool> undefinedFields, Standard standard)
        {

            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.EmployabilitySkillsAndBehaviours))}", !standard.EmployabilitySkillsAndBehaviours.IsSet);
            if (standard.EmployabilitySkillsAndBehaviours.HasValue)
            {
                foreach (var (idDetailPair, index) in standard.EmployabilitySkillsAndBehaviours.Value.Select((s, i) => (s, i)))
                {
                    AddMissingChildProperties(nameof(Standard.EmployabilitySkillsAndBehaviours), undefinedFields, idDetailPair, index);
                }
            }
        }

        private static void CheckFoundationApprenticeshipUrl(Dictionary<string, bool> undefinedFields, Standard standard)
        {

            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.FoundationApprenticeshipUrl))}", !standard.FoundationApprenticeshipUrl.IsSet);
        }

        private static void CheckTechnicalKnowledges(Dictionary<string, bool> undefinedFields, Standard standard)
        {

            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.TechnicalKnowledges))}", !standard.TechnicalKnowledges.IsSet);
            if (standard.TechnicalKnowledges.HasValue)
            {
                foreach (var (idDetailPair, index) in standard.TechnicalKnowledges.Value.Select((s, i) => (s, i)))
                {
                    AddMissingChildProperties(nameof(Standard.TechnicalKnowledges), undefinedFields, idDetailPair, index);
                }
            }
        }

        private static void CheckTechnicalSkills(Dictionary<string, bool> undefinedFields, Standard standard)
        {

            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.TechnicalSkills))}", !standard.TechnicalSkills.IsSet);
            if (standard.TechnicalSkills.HasValue)
            {
                foreach (var (idDetailPair, index) in standard.TechnicalSkills.Value.Select((s, i) => (s, i)))
                {
                    AddMissingChildProperties(nameof(Standard.TechnicalSkills), undefinedFields, idDetailPair, index);
                }
            }
        }

        private static void CheckRelatedOccupations(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.RelatedOccupations))}", !standard.RelatedOccupations.IsSet);
            if (standard.RelatedOccupations.HasValue)
            {
                foreach (var (relatedOccupation, index) in standard.RelatedOccupations.Value.Select((s, i) => (s, i)))
                {
                    if (!relatedOccupation.Name.IsSet)
                    {
                        undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.RelatedOccupations))}[{index}].{GetJsonPropertyName<RelatedOccupation>(nameof(RelatedOccupation.Name))}", true);
                    }

                    if (!relatedOccupation.Reference.IsSet)
                    {
                        undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.RelatedOccupations))}[{index}].{GetJsonPropertyName<RelatedOccupation>(nameof(RelatedOccupation.Reference))}", true);
                    }
                }
            }
        }

        private static void CheckStandardPageUrl(Dictionary<string, bool> undefinedFields, Standard standard)
        {
            undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.StandardPageUrl))}", !standard.StandardPageUrl.IsSet);
        }

        private static void AddMissingChildProperties(string rootPropertyName, Dictionary<string, bool> undefinedFields, IdDetailPair idDetailPair, int index)
        {
            if (!idDetailPair.Id.IsSet)
            {
                undefinedFields.Add($"{GetJsonPropertyName<Standard>(rootPropertyName)}[{index}].{GetJsonPropertyName<IdDetailPair>(nameof(IdDetailPair.Id))}", true);
            }

            if (!idDetailPair.Detail.IsSet)
            {
                undefinedFields.Add($"{GetJsonPropertyName<Standard>(rootPropertyName)}[{index}].{GetJsonPropertyName<IdDetailPair>(nameof(IdDetailPair.Detail))}", true);
            }
        }

        public static string GetJsonPropertyName<T>(string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'.");
            }

            var jsonPropertyAttribute = property.GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                                                .Cast<JsonPropertyAttribute>()
                                                .FirstOrDefault();

            return jsonPropertyAttribute?.PropertyName ?? propertyName;
        }
    }
}
