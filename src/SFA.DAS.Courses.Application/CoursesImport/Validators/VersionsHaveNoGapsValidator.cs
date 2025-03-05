using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using SFA.DAS.Courses.Application.CoursesImport.Extensions.StringExtensions;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class VersionsHaveNoGapsValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        public VersionsHaveNoGapsValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    var referenceNumber = importedStandards[0].ReferenceNumber.Value;
                    var versions = importedStandards
                        .Select(s => s.Version.Value.ParseVersion())
                        .OrderBy(v => v.Major)
                        .ThenBy(v => v.Minor)
                        .ToList();

                    for (int i = 1; i < versions.Count; i++)
                    {
                        var previous = versions[i - 1];
                        var current = versions[i];

                        // expecting the next version to be either (same major, minor+1) or (major+1, minor=0)
                        if (!(current.Major == previous.Major && current.Minor == previous.Minor + 1) &&
                            !(current.Major == previous.Major + 1 && current.Minor == 0))
                        {
                            context.AddFailure($"S1003: {referenceNumber} version {previous.Major}.{previous.Minor} should not be followed by version {current.Major}.{current.Minor}");
                        }
                    }
                });
        }
    }
}
