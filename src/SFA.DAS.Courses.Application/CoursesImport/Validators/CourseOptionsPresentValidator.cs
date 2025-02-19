using System.Collections.Generic;
using FluentValidation;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class CourseOptionsPresentValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        public CourseOptionsPresentValidator()
            : base(ValidationFailureType.Warning)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        // only 1 of these is a required field but either may be present but empty
                        var newOptionsPresent = standard.Options.HasValue && standard.Options.Value.Count > 0;
                        var oldOptionsPresent = standard.OptionsUnstructuredTemplate.HasValue && standard.OptionsUnstructuredTemplate.Value.Count > 0;
                        
                        if(standard.CoreAndOptions.Value && !newOptionsPresent && !oldOptionsPresent)
                        {
                            context.AddFailure($"W1003: {standard.ReferenceNumber.Value} version {standard.Version.Value} coreAndOptions is true, both options and optionsUnstructuredTemplate cannot be empty");
                        }
                    }
                });
        }
    }
}
