using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class RequiredFieldsForFoundationApprenticeshipPresentValidator : ValidatorBase<List<FoundationApprenticeship>>
    {
        public RequiredFieldsForFoundationApprenticeshipPresentValidator() : base(ValidationFailureType.Error)
        {
            RuleFor(x => x).Custom((importedStandards, context) =>
            {
                foreach (var foundationApprenticeship in importedStandards)
                {
                    var undefinedFields = new Dictionary<string, bool>();

                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.ApprovedForDelivery);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.AssessmentChanged);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.AssessmentPlanUrl);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.Change);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.CreatedDate);

                    RequiredFieldChecks.RequireSetObject(undefinedFields, foundationApprenticeship, x => x.EqaProvider, (dict, eqa, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, eqa, x => x.ContactAddress);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, eqa, x => x.ContactEmail);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, eqa, x => x.ContactName);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, eqa, x => x.ProviderName);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, eqa, x => x.WebLink);
                    });

                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.VersionEarliestStartDate);

                    RequiredFieldChecks.RequireSetList(undefinedFields, foundationApprenticeship, x => x.EmployabilitySkillsAndBehaviours, (dict, employabilitySkillsAndBehaviour, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, employabilitySkillsAndBehaviour, x => x.Id);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, employabilitySkillsAndBehaviour, x => x.Detail);
                    });

                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.FoundationApprenticeshipUrl);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.Keywords);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.LarsCode);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.VersionLatestEndDate);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.VersionLatestStartDate);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.Level);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.ProposedMaxFunding);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.OverviewOfRole);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.PublishDate);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.ProposedTypicalDuration);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.ReferenceNumber);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.Regulated);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.RegulatedBody);

                    RequiredFieldChecks.RequireSetList(undefinedFields, foundationApprenticeship, x => x.RegulationDetails, (dict, regulationDetail, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, regulationDetail, x => x.Approved);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, regulationDetail, x => x.Name);
                    });

                    RequiredFieldChecks.RequireSetList(undefinedFields, foundationApprenticeship, x => x.RelatedOccupations, (dict, relatedOccupation, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, relatedOccupation, x => x.Name);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, relatedOccupation, x => x.Reference);
                    });

                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.Route);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.Status);

                    RequiredFieldChecks.RequireSetList(undefinedFields, foundationApprenticeship, x => x.TechnicalKnowledges, (dict, technicalKnowledge, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, technicalKnowledge, x => x.Id);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, technicalKnowledge, x => x.Detail);
                    });

                    RequiredFieldChecks.RequireSetList(undefinedFields, foundationApprenticeship, x => x.TechnicalSkills, (dict, technicalSkill, prefix) =>
                    {
                        RequiredFieldChecks.RequireSetAt(dict, prefix, technicalSkill, x => x.Id);
                        RequiredFieldChecks.RequireSetAt(dict, prefix, technicalSkill, x => x.Detail);
                    });

                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.Title);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.TypicalJobTitles);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.Version);
                    RequiredFieldChecks.RequireSet(undefinedFields, foundationApprenticeship, x => x.VersionNumber);

                    var missingFields = undefinedFields.Where(uf => uf.Value).Select(uf => uf.Key);
                    if (missingFields.Any())
                    {
                        context.AddFailure($"E1001: {ReferenceNumber(foundationApprenticeship)} version {Version(foundationApprenticeship)} has missing fields '{string.Join(",", missingFields)}'");
                    }
                }
            });
        }
    }
}
