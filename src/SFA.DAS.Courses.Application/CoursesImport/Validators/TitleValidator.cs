using System.Collections.Generic;
using FluentValidation;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class TitleValidator : ValidatorBase<List<Standard>>
    {
        public TitleValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach(var standard in importedStandards)
                    {
                        if (standard.Title.Value.Length < 1 || standard.Title.Value.StartsWith(' '))
                        {
                            context.AddFailure($"S1008: {standard.ReferenceNumber.Value} version {standard.Version.Value} title must not start with a space and be greater than 1 character");
                        }
                    }
                });
        }
    }
}
