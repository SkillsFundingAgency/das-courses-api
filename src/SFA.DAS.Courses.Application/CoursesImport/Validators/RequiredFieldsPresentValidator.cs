using System.Collections.Generic;
using System.Linq;
using FluentValidation;

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
                            { "approvedForDelivery", !standard.ApprovedForDelivery.IsSet },
                            { "assessmentPlanUrl", !standard.AssessmentPlanUrl.IsSet },
                            { "behaviours", !standard.Behaviours.IsSet },
                            { "change", !standard.Change.IsSet },
                            { "coreAndOptions", !standard.CoreAndOptions.IsSet },
                            { "coronationEmblem", !standard.CoronationEmblem.IsSet },
                            { "createdDate", !standard.CreatedDate.IsSet },
                            { "duties", !standard.Duties.IsSet },
                            { "earliestStartDate", !standard.VersionEarliestStartDate.IsSet },
                            { "eQAProvider", !standard.EqaProvider.IsSet },
                            { "eQAProvider.contactAddress", standard.EqaProvider.IsSet && standard.EqaProvider.HasValue && !standard.EqaProvider.Value.ContactAddress.IsSet },
                            { "eQAProvider.contactEmail", standard.EqaProvider.IsSet && standard.EqaProvider.HasValue && !standard.EqaProvider.Value.ContactEmail.IsSet },
                            { "eQAProvider.contactName", standard.EqaProvider.IsSet && standard.EqaProvider.HasValue && !standard.EqaProvider.Value.ContactName.IsSet },
                            { "eQAProvider.providerName", standard.EqaProvider.IsSet && standard.EqaProvider.HasValue && !standard.EqaProvider.Value.ProviderName.IsSet },
                            { "eQAProvider.webLink", standard.EqaProvider.IsSet && standard.EqaProvider.HasValue && !standard.EqaProvider.Value.WebLink.IsSet },
                            { "keywords", !standard.Keywords.IsSet },
                            { "knowledges", !standard.Knowledges.IsSet },
                            { "latestEndDate", !standard.VersionLatestEndDate.IsSet },
                            { "latestStartDate", !standard.VersionLatestStartDate.IsSet },
                            { "larsCode", !standard.LarsCode.IsSet },
                            { "level", !standard.Level.IsSet },
                            { "maxFunding", !standard.ProposedMaxFunding.IsSet },
                            { "options or optionsUnstructuredTemplate", !standard.Options.IsSet && !standard.OptionsUnstructuredTemplate.IsSet },
                            { "overviewOfRole", !standard.OverviewOfRole.IsSet },
                            { "publishDate", !standard.PublishDate.IsSet },
                            { "referenceNumber", !standard.ReferenceNumber.IsSet },
                            { "regulatedBody", !standard.RegulatedBody.IsSet },
                            { "route", !standard.Route.IsSet },
                            { "skills", !standard.Skills.IsSet },
                            { "standardPageUrl", !standard.StandardPageUrl.IsSet },
                            { "status", !standard.Status.IsSet },
                            { "tbMainContact", !standard.TbMainContact.IsSet },
                            { "title", !standard.Title.IsSet },
                            { "typicalDuration", !standard.ProposedTypicalDuration.IsSet },
                            { "typicalJobTitles", !standard.TypicalJobTitles.IsSet },
                            { "version", !standard.Version.IsSet },
                            { "versionNumber", !standard.VersionNumber.IsSet }
                        };

                        var missingFields = undefinedFields.Where(uf => uf.Value).Select(uf => uf.Key);
                        if (missingFields.Any())
                        {
                            context.AddFailure($"E1001: {ReferenceNumber(standard)} version {Version(standard)} has missing fields '{string.Join(",", missingFields)}'");
                        }
                    }
                });
        }
    }
}
