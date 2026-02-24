using System.Collections.Generic;
using FluentValidation;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class CreatedDateValidator : ValidatorBase<List<Standard>>
    {
        public CreatedDateValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        if(standard.CreatedDate.HasInvalidValue)
                        {
                            context.AddFailure($"S1012: {standard.ReferenceNumber.Value} version {standard.Version.Value} the createdDate '{standard.CreatedDate.InvalidValue}' is not a valid date");
                        }
                    }
                });
        }
    }
}
