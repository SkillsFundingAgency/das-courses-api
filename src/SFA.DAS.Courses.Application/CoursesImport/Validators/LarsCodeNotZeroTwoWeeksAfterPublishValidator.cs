using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using SFA.DAS.Courses.Application.CoursesImport.Extensions.StringExtensions;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class LarsCodeNotZeroTwoWeeksAfterPublishValidator : ValidatorBase<List<Standard>>
    {
        public LarsCodeNotZeroTwoWeeksAfterPublishValidator()
            : base(ValidationFailureType.Warning)
        {
            RuleFor(standardImports => standardImports)
                .Custom((standardImports, context) =>
                {
                    foreach (var standard in standardImports)
                    {
                        if (standard.ApprenticeshipType is not (ApprenticeshipType.Apprenticeship or ApprenticeshipType.FoundationApprenticeship))
                            continue;

                        var parsedVersion = standard.Version.Value.ParseVersion();
                        if (parsedVersion.Major == 1 && parsedVersion.Minor == 0 
                            && int.TryParse(standard.LarsCode.Value, out int larsCode) && larsCode == 0 
                            && standard.PublishDate.HasValue && standard.PublishDate.Value < DateTime.Now.AddDays(-14))
                        {
                            context.AddFailure($"W1001: {standard.ReferenceNumber.Value} version {standard.Version.Value} has a larsCode 0 more than 2 weeks after its publishDate");
                        }
                    }
                });
        }
    }
}
