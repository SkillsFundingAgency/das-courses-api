using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class RequiredFieldsForApprenticeshipPresentValidator : ValidatorBase<List<Apprenticeship>>
    {
        public RequiredFieldsForApprenticeshipPresentValidator() : base(ValidationFailureType.Error)
        {
            RuleFor(x => x).Custom((importedStandards, context) =>
            {
                foreach (var apprenticeship in importedStandards)
                {
                    var undefinedFields = new Dictionary<string, bool>();

                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.ApprovedForDelivery);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.AssessmentPlanUrl);

                    RequiredFieldChecks.RequireSetList(undefinedFields, apprenticeship, x => x.Behaviours, (dict, behaviour, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, behaviour, x => x.BehaviourId);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, behaviour, x => x.Detail);
                    });

                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.Change);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.CoreAndOptions);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.CoronationEmblem);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.CreatedDate);

                    RequiredFieldChecks.RequireSetList(undefinedFields, apprenticeship, x => x.Duties, (dict, duty, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, duty, x => x.DutyId);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, duty, x => x.DutyDetail);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, duty, x => x.IsThisACoreDuty);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, duty, x => x.MappedBehaviour);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, duty, x => x.MappedKnowledge);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, duty, x => x.MappedOptions);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, duty, x => x.MappedSkills);
                    });

                    RequiredFieldChecks.RequireSetObject(undefinedFields, apprenticeship, x => x.EqaProvider, (dict, eqa, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, eqa, x => x.ContactAddress);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, eqa, x => x.ContactEmail);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, eqa, x => x.ContactName);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, eqa, x => x.ProviderName);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, eqa, x => x.WebLink);
                    });

                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.VersionEarliestStartDate);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.Keywords);

                    RequiredFieldChecks.RequireSetList(undefinedFields, apprenticeship, x => x.Knowledges, (dict, knowledge, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, knowledge, x => x.KnowledgeId);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, knowledge, x => x.Detail);
                    });

                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.LarsCode);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.VersionLatestEndDate);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.VersionLatestStartDate);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.Level);

                    RequiredFieldChecks.RequireEitherSet(undefinedFields, apprenticeship,
                        x => x.Options,
                        x => x.OptionsUnstructuredTemplate);

                    RequiredFieldChecks.ValidateListIfSet(undefinedFields, apprenticeship, x => x.Options,
                        (dict, option, prefix) =>
                        {
                            RequiredFieldChecks.RequireSetAt(dict, prefix, option, x => x.OptionId);
                            RequiredFieldChecks.RequireSetAt(dict, prefix, option, x => x.Title);
                        });
                    
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.OverviewOfRole);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.ProposedMaxFunding);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.PublishDate);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.ProposedTypicalDuration);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.ReferenceNumber);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.Regulated);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.RegulatedBody);
                    
                    RequiredFieldChecks.RequireSetList(undefinedFields, apprenticeship, x => x.RegulationDetails, (dict, regulationDetail, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, regulationDetail, x => x.Approved);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, regulationDetail, x => x.Name);
                    });

                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.Route);
                    
                    RequiredFieldChecks.RequireSetList(undefinedFields, apprenticeship, x => x.Skills, (dict, skill, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, skill, x => x.SkillId);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, skill, x => x.Detail);
                    });

                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.StandardPageUrl);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.Status);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.TbMainContact);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.Title);

                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.TypicalJobTitles);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.Version);
                    RequiredFieldChecks.RequireSet(undefinedFields, apprenticeship, x => x.VersionNumber);

                    var missingFields = undefinedFields.Where(uf => uf.Value).Select(uf => uf.Key);
                    if (missingFields.Any())
                    {
                        context.AddFailure($"E1001: {ReferenceNumber(apprenticeship)} version {Version(apprenticeship)} has missing fields '{string.Join(",", missingFields)}'");
                    }
                }
            });
        }
    }
}
