using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class RequiredFieldsPresentValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        public RequiredFieldsPresentValidator()
            : base(ValidationFailureType.Error)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        var undefinedFields = new Dictionary<string, bool>
                        {
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.ApprovedForDelivery))}", !standard.ApprovedForDelivery.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.AssessmentPlanUrl))}", !standard.AssessmentPlanUrl.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Behaviours))}", !standard.Behaviours.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Change))}", !standard.Change.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.CoreAndOptions))}", !standard.CoreAndOptions.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.CoronationEmblem))}", !standard.CoronationEmblem.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.CreatedDate))}", !standard.CreatedDate.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}", !standard.Duties.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.VersionEarliestStartDate))}", !standard.VersionEarliestStartDate.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}", !standard.EqaProvider.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}.{GetJsonPropertyName<EqaProvider>(nameof(EqaProvider.ContactAddress))}", standard.EqaProvider.IsSet && standard.EqaProvider.HasValue && !standard.EqaProvider.Value.ContactAddress.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}.{GetJsonPropertyName<EqaProvider>(nameof(EqaProvider.ContactEmail))}", standard.EqaProvider.IsSet && standard.EqaProvider.HasValue && !standard.EqaProvider.Value.ContactEmail.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}.{GetJsonPropertyName<EqaProvider>(nameof(EqaProvider.ContactName))}", standard.EqaProvider.IsSet && standard.EqaProvider.HasValue && !standard.EqaProvider.Value.ContactName.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}.{GetJsonPropertyName<EqaProvider>(nameof(EqaProvider.ProviderName))}", standard.EqaProvider.IsSet && standard.EqaProvider.HasValue && !standard.EqaProvider.Value.ProviderName.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.EqaProvider))}.{GetJsonPropertyName<EqaProvider>(nameof(EqaProvider.WebLink))}", standard.EqaProvider.IsSet && standard.EqaProvider.HasValue && !standard.EqaProvider.Value.WebLink.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Keywords))}", !standard.Keywords.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Knowledges))}", !standard.Knowledges.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.VersionLatestEndDate))}", !standard.VersionLatestEndDate.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.VersionLatestStartDate))}", !standard.VersionLatestStartDate.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.LarsCode))}", !standard.LarsCode.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Level))}", !standard.Level.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.ProposedMaxFunding))}", !standard.ProposedMaxFunding.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Options))} or {GetJsonPropertyName<Standard>(nameof(Standard.OptionsUnstructuredTemplate))}", !standard.Options.IsSet && !standard.OptionsUnstructuredTemplate.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.OverviewOfRole))}", !standard.OverviewOfRole.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.PublishDate))}", !standard.PublishDate.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.ReferenceNumber))}", !standard.ReferenceNumber.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.RegulatedBody))}", !standard.RegulatedBody.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Regulated))}", !standard.Regulated.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.RegulationDetail))}", !standard.RegulationDetail.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Route))}", !standard.Route.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Skills))}", !standard.Skills.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.StandardPageUrl))}", !standard.StandardPageUrl.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Status))}", !standard.Status.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.TbMainContact))}", !standard.TbMainContact.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Title))}", !standard.Title.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.ProposedTypicalDuration))}", !standard.ProposedTypicalDuration.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.TypicalJobTitles))}", !standard.TypicalJobTitles.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.Version))}", !standard.Version.IsSet },
                            { $"{GetJsonPropertyName<Standard>(nameof(Standard.VersionNumber))}", !standard.VersionNumber.IsSet }
                        };

                        if (standard.Options.IsSet && standard.Options.HasValue)
                        {
                            foreach (var (option, index) in standard.Options.Value.Select((s, i) => (s, i)))
                            {
                                if (!option.OptionId.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Options))}[{index}].{GetJsonPropertyName<Option>(nameof(Option.OptionId))}", true);
                                }

                                if (!option.Title.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Options))}[{index}].{GetJsonPropertyName<Option>(nameof(Option.Title))}", true);
                                }
                            }
                        }

                        if (standard.Skills.IsSet && standard.Skills.HasValue)
                        {
                            foreach (var (skill, index) in standard.Skills.Value.Select((s, i) => (s, i)))
                            {
                                if (!skill.SkillId.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Skills))}[{index}].{GetJsonPropertyName<Skill>(nameof(Skill.SkillId))}", true);
                                }

                                if (!skill.Detail.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Skills))}[{index}].{GetJsonPropertyName<Skill>(nameof(Skill.Detail))}", true);
                                }
                            }
                        }

                        if (standard.Duties.IsSet && standard.Duties.HasValue)
                        {
                            foreach (var (duty, index) in standard.Duties.Value.Select((s, i) => (s, i)))
                            {
                                if (!duty.DutyId.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.DutyId))}", true);
                                }

                                if (!duty.DutyDetail.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.DutyDetail))}", true);
                                }

                                if (!duty.IsThisACoreDuty.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.IsThisACoreDuty))}", true);
                                }

                                if (!duty.MappedBehaviour.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.MappedBehaviour))}", true);
                                }

                                if (!duty.MappedKnowledge.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.MappedKnowledge))}", true);
                                }
                                
                                if (!duty.MappedOptions.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.MappedOptions))}", true);
                                }

                                if (!duty.MappedSkills.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Duties))}[{index}].{GetJsonPropertyName<Duty>(nameof(Duty.MappedSkills))}", true);
                                }
                            }
                        }

                        if (standard.Behaviours.IsSet && standard.Behaviours.HasValue)
                        {
                            foreach (var (behaviour, index) in standard.Behaviours.Value.Select((s, i) => (s, i)))
                            {
                                if (!behaviour.BehaviourId.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Behaviours))}[{index}].{GetJsonPropertyName<Behaviour>(nameof(Behaviour.BehaviourId))}", true);
                                }

                                if (!behaviour.Detail.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Behaviours))}[{index}].{GetJsonPropertyName<Behaviour>(nameof(Behaviour.Detail))}", true);
                                }
                            }
                        }

                        if (standard.Knowledges.IsSet && standard.Knowledges.HasValue)
                        {
                            foreach (var (knowledge, index) in standard.Knowledges.Value.Select((s, i) => (s, i)))
                            {
                                if (!knowledge.KnowledgeId.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Knowledges))}[{index}].{GetJsonPropertyName<Knowledge>(nameof(Knowledge.KnowledgeId))}", true);
                                }

                                if (!knowledge.Detail.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.Knowledges))}[{index}].{GetJsonPropertyName<Knowledge>(nameof(Knowledge.Detail))}", true);
                                }
                            }
                        }

                        if (standard.RegulationDetail.IsSet && standard.RegulationDetail.HasValue)
                        {
                            foreach (var (regulationDetail, index) in standard.RegulationDetail.Value.Select((s, i) => (s, i)))
                            {
                                if (!regulationDetail.Name.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.RegulationDetail))}[{index}].{GetJsonPropertyName<RegulationDetail>(nameof(RegulationDetail.Name))}", true);
                                }

                                if (!regulationDetail.Approved.IsSet)
                                {
                                    undefinedFields.Add($"{GetJsonPropertyName<Standard>(nameof(Standard.RegulationDetail))}[{index}].{GetJsonPropertyName<RegulationDetail>(nameof(RegulationDetail.Approved))}", true);
                                }
                            }
                        }

                        var missingFields = undefinedFields.Where(uf => uf.Value).Select(uf => uf.Key);
                        if (missingFields.Any())
                        {
                            context.AddFailure($"E1001: {ReferenceNumber(standard)} version {Version(standard)} has missing fields '{string.Join(",", missingFields)}'");
                        }
                    }
                });
        }

        private static string GetJsonPropertyName<T>(string propertyName)
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
