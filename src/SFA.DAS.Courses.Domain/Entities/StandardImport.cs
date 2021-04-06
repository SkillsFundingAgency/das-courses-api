using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardImport : StandardBase
    {
        public List<string> OptionsUnstructuredTemplate { get; set; }

        public static implicit operator StandardImport(Domain.ImportTypes.Standard standard)
        {
            var coreDuties = new List<string>();

            if (standard.Duties.Any() && standard.Skills.Any())
            {
                var mappedSkillsList = GetMappedSkillsList(standard);
                coreDuties = GetSkillDetailFromMappedCoreSkill(standard, mappedSkillsList);
            }

            return new StandardImport
            {
                StandardUId = GetStandardUId(standard.ReferenceNumber, standard.Version),
                LarsCode = standard.LarsCode,
                IfateReferenceNumber = standard.ReferenceNumber,
                Status = standard.Status,
                VersionEarliestStartDate = standard.VersionEarliestStartDate,
                VersionLatestStartDate = standard.VersionLatestStartDate,
                VersionLatestEndDate = standard.VersionLatestEndDate,
                IntegratedDegree = standard.IntegratedDegree,
                Level = standard.Level,
                ProposedTypicalDuration = standard.ProposedTypicalDuration,
                ProposedMaxFunding = standard.ProposedMaxFunding,
                OverviewOfRole = standard.OverviewOfRole,
                StandardPageUrl = standard.StandardPageUrl.AbsoluteUri,
                Title = standard.Title.Trim(),
                TypicalJobTitles = string.Join("|", standard.TypicalJobTitles),
                Version = standard.Version ?? 0,
                Keywords = standard.Keywords.Any() ? string.Join("|", standard.Keywords) : null,
                AssessmentPlanUrl = standard.AssessmentPlanUrl,
                ApprovedForDelivery = standard.ApprovedForDelivery,
                TrailBlazerContact = standard.TbMainContact,
                EqaProviderName = standard.EqaProvider?.ProviderName,
                EqaProviderContactName = standard.EqaProvider?.ContactName,
                EqaProviderContactEmail = standard.EqaProvider?.ContactEmail,
                EqaProviderWebLink = standard.EqaProvider?.WebLink,
                RegulatedBody = standard.RegulatedBody,
                Skills = standard.Skills?.Select(x => x.Detail).ToList() ?? new List<string>(),
                Knowledge = standard.Knowledge?.Select(x => x.Detail).ToList() ?? new List<string>(),
                Behaviours = standard.Behaviours?.Select(x => x.Detail).ToList() ?? new List<string>(),
                Duties = standard.Duties?.Select(x => x.DutyDetail).ToList() ?? new List<string>(),
                CoreAndOptions = standard.CoreAndOptions,
                CoreDuties = coreDuties,
                IntegratedApprenticeship = SetIsIntegratedApprenticeship(standard),
                Options = standard.Options?.Select(o => o.Title).ToList() ?? new List<string>(),
                OptionsUnstructuredTemplate = standard.OptionsUnstructuredTemplate ?? new List<string>(),
                RouteCode = standard.RouteCode
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

        private static List<string> GetSkillDetailFromMappedCoreSkill(ImportTypes.Standard standard, IEnumerable<string> mappedSkillsList)
        {
            return standard.Skills
                .Where(s => mappedSkillsList.Contains(s.SkillId))
                .Select(s => s.Detail).ToList();
        }
    }
}
