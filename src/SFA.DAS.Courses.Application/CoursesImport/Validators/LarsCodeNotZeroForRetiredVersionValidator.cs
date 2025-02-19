using System.Collections.Generic;
using FluentValidation;
using SFA.DAS.Courses.Application.CoursesImport.Extensions.StringExtensions;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class LarsCodeNotZeroForRetiredVersionValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        public LarsCodeNotZeroForRetiredVersionValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        var parsedVersion = standard.Version.Value.ParseVersion();
                        if (parsedVersion.Major > 1 && standard.LarsCode.Value == 0 && standard.Status.Value.Equals(Domain.Courses.Status.Retired, System.StringComparison.OrdinalIgnoreCase))
                        {
                            context.AddFailure($"S1006: {standard.ReferenceNumber.Value} version {standard.Version.Value} should not have a larsCode 0 when status is '{Domain.Courses.Status.Retired}'");
                        }
                    }
                });
        }
    }
}
