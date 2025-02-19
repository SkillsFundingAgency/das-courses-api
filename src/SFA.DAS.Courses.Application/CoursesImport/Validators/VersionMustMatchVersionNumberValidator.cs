using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class VersionMustMatchVersionNumberValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
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
                        if (activeStatuses.Contains(standard.Status.Value) && standard.Version.Value != standard.VersionNumber.Value)
                        {
                            context.AddFailure($"S1007: {standard.ReferenceNumber.Value} version {standard.Version.Value} should have matching versionNumber");
                        }
                    }
                });
        }
    }
}
