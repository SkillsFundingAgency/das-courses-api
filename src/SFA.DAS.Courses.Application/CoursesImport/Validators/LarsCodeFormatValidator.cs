using System.Collections.Generic;
using FluentValidation;
using SFA.DAS.Courses.Domain.Identifiers;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class LarsCodeFormatValidator : ValidatorBase<List<Standard>>
    {
        public LarsCodeFormatValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        if (standard.ApprenticeshipType is not ApprenticeshipType.ApprenticeshipUnit)
                            continue;
                        
                        if (standard.ReferenceNumber.HasInvalidValue || !IdentifierRegexes.ShortCourseLarsCode.IsMatch(standard.LarsCode.Value.Trim()))
                        {
                            context.AddFailure($"E1005: {ReferenceNumber(standard)} version {Version(standard)} of type '{standard.ApprenticeshipType}' has not got larsCode in the correct format.");
                        }
                    }
                });
        }
    }
}
