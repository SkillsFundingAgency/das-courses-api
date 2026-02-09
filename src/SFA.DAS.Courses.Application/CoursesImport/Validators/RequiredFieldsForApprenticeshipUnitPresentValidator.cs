using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class RequiredFieldsForApprenticeshipUnitPresentValidator : ValidatorBase<List<ApprenticeshipUnit>>
    {
        public RequiredFieldsForApprenticeshipUnitPresentValidator() : base(ValidationFailureType.Error)
        {
            RuleFor(importedStandards => importedStandards)

                .Custom((importedStandards, context) =>
                {
                    foreach (var apprenticeshipUnit in importedStandards)
                    {
                        var undefinedFields = new Dictionary<string, bool>();

                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.ApprovedForDelivery);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.CreatedDate);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.VersionEarliestStartDate);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.Keywords);

                        RequiredFieldChecks.RequireSetList(undefinedFields, apprenticeshipUnit, x => x.Knowledges, (dict, knowledge, prefix) =>
                        {
                            RequiredFieldChecks.RequireSetAt(dict, prefix, knowledge, x => x.KnowledgeId);
                            RequiredFieldChecks.RequireSetAt(dict, prefix, knowledge, x => x.Detail);
                        });

                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.LarsCode);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.LastUpdated);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.VersionLatestEndDate);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.VersionLatestStartDate);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.LearningHours);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.Level);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.ProposedMaxFunding);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.OverviewOfRole);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.PublishDate);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.ReferenceNumber);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.Regulated);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.RegulatedBody);

                        RequiredFieldChecks.RequireSetList(undefinedFields, apprenticeshipUnit, x => x.RegulationDetails, (dict, regulationDetail, prefix) =>
                        {
                            RequiredFieldChecks.RequireSetAt(dict, prefix, regulationDetail, x => x.Approved);
                            RequiredFieldChecks.RequireSetAt(dict, prefix, regulationDetail, x => x.Name);
                        });

                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.Route);

                        RequiredFieldChecks.RequireSetList(undefinedFields, apprenticeshipUnit, x => x.Skills, (dict, skill, prefix) =>
                        {
                            RequiredFieldChecks.RequireSetAt(dict, prefix, skill, x => x.SkillId);
                            RequiredFieldChecks.RequireSetAt(dict, prefix, skill, x => x.Detail);
                        });

                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.Status);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.Title);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.TypicalJobTitles);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.Url);
                        RequiredFieldChecks.RequireSet(undefinedFields, apprenticeshipUnit, x => x.Version);

                        var missingFields = undefinedFields.Where(uf => uf.Value).Select(uf => uf.Key);
                        if (missingFields.Any())
                        {
                            context.AddFailure($"E1001: {ReferenceNumber(apprenticeshipUnit)} version {Version(apprenticeshipUnit)} has missing fields '{string.Join(",", missingFields)}'");
                        }
                    }
                });
        }
    }
}
