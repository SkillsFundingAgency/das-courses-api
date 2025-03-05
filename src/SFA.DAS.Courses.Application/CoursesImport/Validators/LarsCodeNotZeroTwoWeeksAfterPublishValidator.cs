using System;
using System.Collections.Generic;
using FluentValidation;
using SFA.DAS.Courses.Application.CoursesImport.Extensions.StringExtensions;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class LarsCodeNotZeroTwoWeeksAfterPublishValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        public LarsCodeNotZeroTwoWeeksAfterPublishValidator()
            : base(ValidationFailureType.Warning)
        {
            RuleFor(standardImports => standardImports)
                .Custom((standardImports, context) =>
                {
                    foreach (var standard in standardImports)
                    {
                        var parsedVersion = standard.Version.Value.ParseVersion();
                        if (parsedVersion.Major == 1 && parsedVersion.Minor == 0 && standard.LarsCode.Value == 0 && 
                            standard.PublishDate.HasValue && standard.PublishDate.Value < DateTime.Now.AddDays(-14))
                        {
                            context.AddFailure($"W1001: {standard.ReferenceNumber.Value} version {standard.Version.Value} has a larsCode 0 more than 2 weeks after its publishDate");
                        }
                    }
                });
        }
    }
}
