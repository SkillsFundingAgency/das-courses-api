using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Extensions;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardImport : StandardBase
    {
        public List<string> OptionsUnstructuredTemplate { get; set; }
        public DateTime? CreatedDate { get; set; }

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
                StandardUId = standard.ReferenceNumber.ToStandardUId(standard.Version),
                LarsCode = standard.LarsCode,
                IfateReferenceNumber = standard.ReferenceNumber?.Trim(),
                Status = standard.Status?.Trim(),
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
                Version = standard.Version.ToBaselineVersion(),
                VersionMajor = GetVersionPart(standard.Version, VersionPart.Major),
                VersionMinor = GetVersionPart(standard.Version, VersionPart.Minor),
                Keywords = standard.Keywords.Any() ? string.Join("|", standard.Keywords) : null,
                AssessmentPlanUrl = standard.AssessmentPlanUrl,
                ApprovedForDelivery = standard.ApprovedForDelivery,
                TrailBlazerContact = standard.TbMainContact?.Trim(),
                EqaProviderName = standard.EqaProvider?.ProviderName?.Trim(),
                EqaProviderContactName = standard.EqaProvider?.ContactName?.Trim(),
                EqaProviderContactEmail = standard.EqaProvider?.ContactEmail?.Trim(),
                EqaProviderWebLink = standard.EqaProvider?.WebLink,
                RegulatedBody = standard.RegulatedBody?.Trim(),
                Duties = standard.Duties?.Select(x => x.DutyDetail).ToList() ?? new List<string>(),
                CoreAndOptions = standard.CoreAndOptions,
                CoreDuties = coreDuties,
                IntegratedApprenticeship = SetIsIntegratedApprenticeship(standard),
                Options = CreateStructuredOptionsList(standard),
                OptionsUnstructuredTemplate = standard.OptionsUnstructuredTemplate ?? new List<string>(),
                RouteCode = standard.RouteCode,
                CreatedDate = standard.CreatedDate,
                EPAChanged = IsEPAChanged(standard)
            };
        }

        private static int GetVersionPart(string version, VersionPart part)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return 0;
            }

            var versionParts = version.Split('.', StringSplitOptions.RemoveEmptyEntries);
            var versionPart = string.Empty;

            if (versionParts.Length != 2)
            {
                return 0;
            }
            else if (part == VersionPart.Major)
            {
                versionPart = versionParts[0];
            }
            else if (part == VersionPart.Minor)
            {
                versionPart = versionParts[1];
            }

            if (int.TryParse(versionPart, out var intVersion))
            {
                return intVersion;
            }

            return 0;
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

        private static bool IsEPAChanged(ImportTypes.Standard standard)
        {
            if (string.IsNullOrWhiteSpace(standard.Change)) return false;

            return standard.Change.Contains("End-point assessment plan revised", StringComparison.OrdinalIgnoreCase);
        }

        private static List<StandardOption> CreateStructuredOptionsList(ImportTypes.Standard standard)
        {
            if(!standard.CoreAndOptions)
            {
                return CreateStructuredOptionsListWithoutMapping(standard);
            }

            var options = standard.Options ?? new List<ImportTypes.Option>();
            var od = options.Select(x => (x.OptionId, standard.Duties.Where(y => y.MappedOptions?.Contains(x.OptionId) == true)));
            var coreDuties = standard.Duties.Where(x => x.IsThisACoreDuty == 1);

            var coreKnowledge = standard.Knowledge?
                .Where(y => coreDuties.SelectMany(x => x.MappedKnowledge.EmptyEnumerableIfNull()).Contains(y.KnowledgeId));

            return options?.Select(x => new StandardOption
            {
                OptionId = x.OptionId,
                Title = x.Title?.Trim(),
                Knowledge = standard.Knowledge?
                    .Where(y => od
                                .Where(z => z.OptionId == x.OptionId)
                                .SelectMany(z => z.Item2)
                                .SelectMany(z => z.MappedKnowledge.EmptyEnumerableIfNull())
                                .Contains(y.KnowledgeId))
                    .Union(coreKnowledge)
                    .Select(x => x.Detail)
                    .ToList(),
                Skills = standard.Skills?
                    .Where(y => od
                                .Where(z => z.OptionId == x.OptionId)
                                .SelectMany(z => z.Item2)
                                .SelectMany(z => z.MappedSkills.EmptyEnumerableIfNull(), (_, id) => id.ToString())
                                .Contains(y.SkillId))
                    .Select(x => x.Detail)
                    .ToList(),
                Behaviours = standard.Behaviours?
                    .Where(y => od
                                .Where(z => z.OptionId == x.OptionId)
                                .SelectMany(z => z.Item2)
                                .SelectMany(z => z.MappedBehaviour.EmptyEnumerableIfNull(), (_, id) => id.ToString())
                                .Contains(y.BehaviourId))
                    .Select(x => x.Detail)
                    .ToList(),
            }).ToList();
        }

        private static List<StandardOption> CreateStructuredOptionsListWithoutMapping(ImportTypes.Standard standard)
        {
            return new List<StandardOption>
            {
                new StandardOption
                {
                    Knowledge = standard.Knowledge.Select(x => x.Detail).ToList(),
                    Skills = standard.Skills.Select(x => x.Detail).ToList(),
                    Behaviours = standard.Behaviours.Select(x => x.Detail).ToList(),
                }
            };
        }
    }
}
