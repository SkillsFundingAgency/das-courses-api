using System.Collections.Generic;
using FluentValidation;
using SFA.DAS.Courses.Application.CoursesImport.Extensions.StringExtensions;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class LarsCodeNotZeroForNewVersionValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        public LarsCodeNotZeroForNewVersionValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        var parsedVersion = standard.Version.Value.ParseVersion();
                        if (parsedVersion.Major >= 1 && parsedVersion.Minor > 0 && standard.LarsCode == 0)
                        {
                            context.AddFailure($"S1001: {standard.ReferenceNumber.Value} version {standard.Version.Value} has larsCode 0");
                        }
                    }
                });
        }
    }
}
