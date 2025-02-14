using System;
using System.Collections.Generic;
using FluentValidation;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class StartDatesValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        public StartDatesValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    var hasAddedFailures = false;

                    foreach (var standard in importedStandards)
                    {
                        if(standard.VersionEarliestStartDate.HasInvalidValue)
                        {
                            context.AddFailure($"S1010: {standard.ReferenceNumber.Value} version {standard.Version.Value} the earliestStartDate '{standard.VersionEarliestStartDate.InvalidValue}' is not a valid date");
                            hasAddedFailures = true;
                        }

                        if (standard.VersionLatestEndDate.HasInvalidValue)
                        {
                            context.AddFailure($"S1011: {standard.ReferenceNumber.Value} version {standard.Version.Value} the latestStartDate '{standard.VersionEarliestStartDate.InvalidValue}' is not a valid date");
                            hasAddedFailures = true;
                        }
                    }

                    if (!hasAddedFailures)
                    {
                        for (int i = 1; i < importedStandards.Count; i++)
                        {
                            var previous = importedStandards[i - 1];
                            var current = importedStandards[i];

                            if ((previous.VersionLatestStartDate.Value?.AddDays(1) ?? DateTime.MaxValue) != (current.VersionEarliestStartDate.Value ?? DateTime.MinValue))
                            {
                                context.AddFailure($"S1004: {previous.ReferenceNumber.Value} version {previous.Version.Value} and {current.Version.Value} do not have a contiguous latestStartDate and earliestStartDate");
                            }
                        }
                    }
                });
        }
    }
}
