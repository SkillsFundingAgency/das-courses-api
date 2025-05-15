using System.Collections.Generic;
using FluentValidation;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class LarsCodeIsNumberValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        public LarsCodeIsNumberValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach(var standard in importedStandards)
                    {
                        if (standard.LarsCode.HasInvalidValue)
                        {
                            context.AddFailure($"S1005: {standard.ReferenceNumber.Value} version {standard.Version.Value} larsCode is not a number");
                        }
                    }
                });
        }
    }
}
