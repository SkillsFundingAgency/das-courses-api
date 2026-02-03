using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Newtonsoft.Json;
using SFA.DAS.Courses.Application.CoursesImport.Extensions.StringExtensions;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Application.CoursesImport.Validators
{

    public class CourseOptionsPreservedValidator : ValidatorBase<List<Domain.ImportTypes.Standard>>
    {
        public CourseOptionsPreservedValidator(List<Standard> currentStandards)
            : base(ValidationFailureType.StandardError)
        {
            RuleFor(importedStandards => importedStandards)
                .Custom((importedStandards, context) =>
                {
                    foreach (var standard in importedStandards)
                    {
                        var parsedVersion = standard.Version.Value.ParseVersion();
                        var currentStandard = currentStandards.FirstOrDefault(c =>
                            c.IfateReferenceNumber == standard.ReferenceNumber &&
                            c.VersionMajor == parsedVersion.Major &&
                            c.VersionMinor == parsedVersion.Minor);
                    
                        if (currentStandard == null || !currentStandard.Options.Any())
                        {
                            continue;
                        }
                    
                        var importedOptions = standard.Options.Value ?? new List<Domain.ImportTypes.Option>();
                        var currentTitles = JsonConvert.DeserializeObject<List<StandardOption>>(currentStandard.Options)
                            .Select(cso => cso.Title)
                            .Where(title => title != Domain.Courses.StandardOption.CoreTitle)
                            .ToList();
                        var importedTitles = importedOptions
                            .Select(io => io.Title.Value.Trim())
                            .ToList();
                    
                        var removedOptions = currentTitles.Except(importedTitles).ToList();
                        var changedOptions = new List<string>();
                    
                        foreach (var oldTitle in currentTitles)
                        {
                            if (!importedTitles.Contains(oldTitle))
                            {
                                var newTitle = importedTitles.FirstOrDefault(newTitle => currentTitles.IndexOf(oldTitle) == importedTitles.IndexOf(newTitle));
                    
                                if (newTitle != null)
                                {
                                    changedOptions.Add($"{oldTitle} → {newTitle}");
                                    removedOptions.Remove(oldTitle);
                                }
                            }
                        }
                    
                        if (changedOptions.Any())
                        {
                            context.AddFailure($"S1013: {standard.ReferenceNumber.Value} version {standard.Version.Value} has changed option titles: {string.Join("; ", changedOptions)}");
                        }
                    
                        if (removedOptions.Any())
                        {
                            context.AddFailure($"S1014: {standard.ReferenceNumber.Value} version {standard.Version.Value} has removed options: {string.Join(", ", removedOptions)}");
                        }
                    }
                });
        }
    }
}
