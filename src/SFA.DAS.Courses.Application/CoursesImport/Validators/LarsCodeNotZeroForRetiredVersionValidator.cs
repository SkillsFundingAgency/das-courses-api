using System.Collections.Generic;
using FluentValidation;
using SFA.DAS.Courses.Application.CoursesImport.Extensions.StringExtensions;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class LarsCodeNotZeroForRetiredVersionValidator : ValidatorBase<List<Standard>>
    {
        public LarsCodeNotZeroForRetiredVersionValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        if (standard.ApprenticeshipType is not (ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship))
                            continue;

                        var parsedVersion = standard.Version.Value.ParseVersion();
                        if ((parsedVersion.Major >= 1 || (parsedVersion.Major == 1 && parsedVersion.Minor > 0))
                            && int.TryParse(standard.LarsCode.Value, out int larsCode) && larsCode == 0
                            && standard.Status.Value.Equals(Domain.Courses.Status.Retired, System.StringComparison.OrdinalIgnoreCase))
                        {
                            context.AddFailure($"S1006: {standard.ReferenceNumber.Value} version {standard.Version.Value} should not have a larsCode 0 when status is '{Domain.Courses.Status.Retired}'");
                        }
                    }
                });
        }
    }
}
