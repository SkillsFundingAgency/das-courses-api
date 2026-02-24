using System.Collections.Generic;
using FluentValidation;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class LarsCodeIsNumberValidator : ValidatorBase<List<Standard>>
    {
        public LarsCodeIsNumberValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        if (standard.ApprenticeshipType is not (ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship))
                            continue;
                        
                        if (standard.LarsCode.HasInvalidValue || !int.TryParse(standard.LarsCode, out _))
                        {
                            context.AddFailure($"S1005: {standard.ReferenceNumber.Value} version {standard.Version.Value} larsCode is not a number");
                        }
                    }
                });
        }
    }
}
