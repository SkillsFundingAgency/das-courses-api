using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using SFA.DAS.Courses.Application.CoursesImport.Extensions.StringExtensions;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class StatusRecommendedValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        public StatusRecommendedValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    var validStatuses = new[] { Status.ApprovedForDelivery, Status.Retired, Status.Withdrawn};
                    
                    foreach (var standard in importedStandards)
                    {
                        var parsedVersion = standard.Version.Value.ParseVersion();
                        if (parsedVersion.Major == 1 && parsedVersion.Minor == 0 && !validStatuses.Contains(standard.Status.Value))
                        {
                            context.AddFailure($"W1002: {standard.ReferenceNumber.Value} version {standard.Version.Value} has status '{standard.Status.Value}'");
                        }
                    }
                });
        }
    }
}
