using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class VersionMustMatchVersionNumberValidator : ValidatorBase<List<Standard>>
    {
        public VersionMustMatchVersionNumberValidator()
            : base(ValidationFailureType.StandardError)
        {
            var activeStatuses = new[] { Domain.Courses.Status.ApprovedForDelivery, Domain.Courses.Status.Retired };
            
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        if (standard.ApprenticeshipType is not (ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship))
                            continue;

                        if (activeStatuses.Contains(standard.Status.Value) && standard.Version.Value != standard.VersionNumber.Value)
                        {
                            context.AddFailure($"S1007: {standard.ReferenceNumber.Value} version {standard.Version.Value} should have matching versionNumber");
                        }
                    }
                });
        }
    }
}
