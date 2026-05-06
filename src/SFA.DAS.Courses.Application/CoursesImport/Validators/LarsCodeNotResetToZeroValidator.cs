using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class LarsCodeNotResetToZeroValidator : ValidatorBase<List<Standard>>
    {
        public LarsCodeNotResetToZeroValidator(List<Domain.Entities.Standard> currentStandards)
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    if (importedStandards == null)
                    {
                        return;
                    }

                    foreach (var standard in importedStandards)
                    {
                        if (standard.ApprenticeshipType is not (ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship))
                        {
                            continue;
                        }

                        if (!int.TryParse(standard.LarsCode.Value, out var importedLarsCode) || importedLarsCode != 0)
                        {
                            continue;
                        }

                        var hasCurrentStandardWithNonZeroLarsCode = currentStandards.Any(currentStandard =>
                            currentStandard.IfateReferenceNumber == standard.ReferenceNumber.Value &&
                            int.TryParse(currentStandard.LarsCode, out int currentStandardLarsCode) && currentStandardLarsCode != 0);

                        if (hasCurrentStandardWithNonZeroLarsCode)
                        {
                            context.AddFailure($"S1001: {standard.ReferenceNumber.Value} version {standard.Version.Value} has larsCode 0 but an existing version of this standard has a non-zero larsCode");
                        }
                    }
                });
        }
    }
}
