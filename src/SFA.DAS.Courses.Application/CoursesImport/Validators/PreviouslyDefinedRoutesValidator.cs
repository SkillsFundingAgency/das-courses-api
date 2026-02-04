using System.Collections.Generic;
using FluentValidation;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class PreviouslyDefinedRoutesValidator : ValidatorBase<List<Domain.ImportTypes.SkillsEngland.Standard>>
    {
        public PreviouslyDefinedRoutesValidator(List<Route> currentRoutes)
            : base(ValidationFailureType.Warning)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        // ignoring blank routes as these will not actually be imported
                        if(!string.IsNullOrEmpty(standard.Route.Value) && !currentRoutes.Exists(r => r.Name == standard.Route.Value))
                        {
                            context.AddFailure($"W1004: {standard.ReferenceNumber.Value} version {standard.Version.Value} route '{standard.Route.Value}' has not been imported before");
                        }
                    }
                });
        }
    }
}
