using FluentValidation;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class ReferenceNumberFormatValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        private static readonly Regex ReferenceNumberRegex = new Regex(@"^ST\d{4}$", RegexOptions.Compiled);

        public ReferenceNumberFormatValidator()
            : base(ValidationFailureType.Error)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        if (standard.ReferenceNumber.HasInvalidValue || !ReferenceNumberRegex.IsMatch(standard.ReferenceNumber.Value))
                        {
                            context.AddFailure($"E1002: {ReferenceNumber(standard)} version {Version(standard)} referenceNumber is not in the correct format.");
                        }
                    }
                });
        }
    }
}
