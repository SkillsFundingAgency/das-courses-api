using System;
using System.Collections.Generic;
using FluentValidation;
using SFA.DAS.Courses.Domain.Identifiers;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class ReferenceNumberFormatValidator : ValidatorBase<List<Standard>>
    {
        public ReferenceNumberFormatValidator()
            : base(ValidationFailureType.Error)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        var regex = standard.ApprenticeshipType switch
                        {
                            ApprenticeshipType.Apprenticeship =>
                                IdentifierRegexes.StandardReferenceNumber,

                            ApprenticeshipType.FoundationApprenticeship =>
                                IdentifierRegexes.FoundationReferenceNumber,

                            ApprenticeshipType.ApprenticeshipUnit =>
                                IdentifierRegexes.ShortCourseReferenceNumber,
                            
                            _ => throw new NotImplementedException()
                        };

                        if (standard.ReferenceNumber.HasInvalidValue || !regex.IsMatch(standard.ReferenceNumber.Value.Trim()))
                        {
                            context.AddFailure($"E1002: {ReferenceNumber(standard)} version {Version(standard)} of type '{standard.ApprenticeshipType}' has not got referenceNumber in the correct format.");
                        }
                    }
                });
        }
    }
}
