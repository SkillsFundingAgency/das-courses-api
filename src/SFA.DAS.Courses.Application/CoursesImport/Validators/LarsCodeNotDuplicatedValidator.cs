using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class LarsCodeNotDuplicatedValidator : ValidatorBase<List<StandardImport>>
    {
        public LarsCodeNotDuplicatedValidator(Dictionary<string, List<StandardImport>> allStandardImports)
            : base(ValidationFailureType.Error)
        {
            RuleFor(standardImports => standardImports)
                .Custom((standardImports, context) =>
                {
                    foreach (var standard in standardImports.Where(si => si.LarsCode != 0))
                    {
                        foreach (var kv in allStandardImports)
                        {
                            if (kv.Key == standard.IfateReferenceNumber)
                                continue;

                            foreach(var duplicate in kv.Value.Where(si => si.LarsCode == standard.LarsCode))
                            {
                                context.AddFailure($"E1004: {standard.IfateReferenceNumber} version {standard.Version} has duplicated larsCode {duplicate.LarsCode} with standard {duplicate.IfateReferenceNumber} version {duplicate.Version}");   
                            }
                        }
                    }
                });
        }
    }
}
