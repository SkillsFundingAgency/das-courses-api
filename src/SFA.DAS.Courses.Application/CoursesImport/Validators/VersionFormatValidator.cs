using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentValidation;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class VersionFormatValidator : ValidatorBase<List<Standard>>
    {
        private static readonly Regex VersionPattern = new Regex(@"^\d+\.\d+$", RegexOptions.Compiled);

        public VersionFormatValidator()
            : base(ValidationFailureType.Error)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach(var standard in importedStandards)
                    {
                        if (standard.Version.HasInvalidValue || !VersionPattern.IsMatch(standard.Version.Value))
                        { 
                            context.AddFailure($"E1003: {ReferenceNumber(standard)} version {Version(standard)} does not have correct format 'major.minor'");
                        }
                    }
                });
        }
    }
}
