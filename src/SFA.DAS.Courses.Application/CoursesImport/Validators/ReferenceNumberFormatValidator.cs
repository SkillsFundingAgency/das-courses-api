using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentValidation;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class ReferenceNumberFormatValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        private static readonly Regex StandardReferenceNumberRegex = new Regex(@"^ST\d{4}$", RegexOptions.Compiled);
        private static readonly Regex FoundationReferenceNumberRegex = new Regex(@"^FA\d{4}$", RegexOptions.Compiled);

        public ReferenceNumberFormatValidator()
            : base(ValidationFailureType.Error)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        var regex = standard.ApprenticeshipType == Domain.Entities.ApprenticeshipType.Apprenticeship ? StandardReferenceNumberRegex : FoundationReferenceNumberRegex;
                        if (standard.ReferenceNumber.HasInvalidValue || !regex.IsMatch(standard.ReferenceNumber.Value.Trim()))
                        {
                            context.AddFailure($"E1002: {ReferenceNumber(standard)} version {Version(standard)} of type '{standard.ApprenticeshipType}' has not got referenceNumber in the correct format.");
                        }
                    }
                });
        }
    }
}
