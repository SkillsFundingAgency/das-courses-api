using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{
    public class StatusValidValidator : ValidatorBase<List<Domain.ImportTypes.SkillsEngland.Standard>>
    {
        public StatusValidValidator()
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    var validStatuses = new[] { Status.ApprovedForDelivery, Status.Retired, Status.Withdrawn, Status.ProposalInDevelopment, Status.InDevelopment};
                    
                    foreach (var standard in importedStandards)
                    {
                        if(!validStatuses.Contains(standard.Status.Value))
                        {
                            context.AddFailure($"S1009: {standard.ReferenceNumber.Value} version {standard.Version.Value} has invalid status '{standard.Status.Value}'");
                        }
                    }
                });
        }
    }
}
