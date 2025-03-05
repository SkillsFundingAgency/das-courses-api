using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class VersionsSingleApprovedValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        public VersionsSingleApprovedValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    var approvedVersions = importedStandards
                        .Where(s => string.Equals(s.Status.Value, Domain.Courses.Status.ApprovedForDelivery, StringComparison.OrdinalIgnoreCase))
                        .Select(s => s.Version.Value)
                        .ToList();

                    if (approvedVersions.Count > 1)
                    {
                        var referenceNumber = importedStandards[0].ReferenceNumber.Value;
                        var versionsList = string.Join(", ", approvedVersions);

                        context.AddFailure($"S1002: {referenceNumber} versions {versionsList} only 1 of these should be status '{Domain.Courses.Status.ApprovedForDelivery}'");
                    }
                });
        }
    }
}
