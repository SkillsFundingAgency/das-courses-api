using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardImport : StandardBase
    {
        public static implicit operator StandardImport(Domain.ImportTypes.Standard standard)
        {
            string coreSkillsCount = null;

            if (standard.Duties.Any() && standard.Skills.Any())
            {
                if (standard.CoreAndOptions)
                {
                    var mappedSkillsList = GetMappedSkillsList(standard);
                    coreSkillsCount = GetSkillDetailFromMappedCoreSkill(standard, mappedSkillsList);
                }
                else
                {
                    coreSkillsCount = string.Join("|", 
                        standard.Skills
                        .Select(s => s.Detail));
                }
            }

            return new StandardImport
            {
                Id = standard.LarsCode,
                CoreSkillsCount = coreSkillsCount,
                IntegratedDegree = standard.IntegratedDegree,
                Level = standard.Level,
                OverviewOfRole = standard.OverviewOfRole,
                StandardPageUrl = standard.StandardPageUrl.AbsoluteUri,
                Title = standard.Title.Trim(),
                TypicalDuration = standard.TypicalDuration,
                TypicalJobTitles = string.Join("|", standard.TypicalJobTitles),
                Version = standard.Version,
                Keywords = standard.Keywords.Any() ? string.Join("|", standard.Keywords) : null,
                RouteId = standard.RouteId
            };
        }

        private static IEnumerable<string> GetMappedSkillsList(Domain.ImportTypes.Standard standard)
        {
            return standard.Duties
                .Where(d => d.IsThisACoreDuty.Equals(1) && d.MappedSkills != null)
                .SelectMany(d => d.MappedSkills)
                .Select(s => s.ToString());
        }

        private static string GetSkillDetailFromMappedCoreSkill(ImportTypes.Standard standard, IEnumerable<string> mappedSkillsList)
        {
            return string.Join("|", standard.Skills
                .Where(s => mappedSkillsList.Contains(s.SkillId))
                .Select(s => s.Detail));
        }
    }
}
