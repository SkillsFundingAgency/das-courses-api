﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardImport : StandardBase
    {
        public static implicit operator StandardImport(Domain.ImportTypes.Standard standard)
        {
            string coreDuties = null;

            if (standard.Duties.Any() && standard.Skills.Any())
            {
                var mappedSkillsList = GetMappedSkillsList(standard);
                coreDuties = GetSkillDetailFromMappedCoreSkill(standard, mappedSkillsList);
            }

            return new StandardImport
            {
                Id = standard.LarsCode,
                StandardUId = GetStandardUId(standard.ReferenceNumber, standard.Version),
                LarsCode = standard.LarsCode,
                IfateReferenceNumber = standard.ReferenceNumber,
                Status = standard.Status,
                IntegratedDegree = standard.IntegratedDegree,
                Level = standard.Level,
                OverviewOfRole = standard.OverviewOfRole,
                StandardPageUrl = standard.StandardPageUrl.AbsoluteUri,
                Title = standard.Title.Trim(),
                TypicalJobTitles = string.Join("|", standard.TypicalJobTitles),
                Version = standard.Version ?? 0,
                Keywords = standard.Keywords.Any() ? string.Join("|", standard.Keywords) : null,
                RouteId = standard.RouteId,
                RegulatedBody = standard.RegulatedBody,
                Skills = standard.Skills?.Select(x => x.Detail).ToList() ?? new List<string>(),
                Knowledge = standard.Knowledge?.Select(x => x.Detail).ToList() ?? new List<string>(),
                Behaviours = standard.Behaviours?.Select(x => x.Detail).ToList() ?? new List<string>(),
                Duties = standard.Duties?.Select(x => x.DutyDetail).ToList() ?? new List<string>(),
                CoreAndOptions = standard.CoreAndOptions,
                CoreDuties = coreDuties,
                IntegratedApprenticeship = SetIsIntegratedApprenticeship(standard)
            };
        }

        private static string GetStandardUId(string ifateReferenceNumber, decimal? version)
        {
            var derivedVersion = version.HasValue && version != 0 ? version.Value : 1;
            return $"{ifateReferenceNumber}_{derivedVersion.ToString("0.0")}";
        }

        private static bool SetIsIntegratedApprenticeship(Domain.ImportTypes.Standard standard)
        {
            if (standard.Level >= 6)
            {
                return standard.IntegratedDegree.Equals("integrated degree", StringComparison.CurrentCultureIgnoreCase);
            }

            if (standard.Level <= 5 && standard.IntegratedApprenticeship.HasValue)
            {
                return standard.IntegratedApprenticeship.Value;
            }

            return false;
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
