using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Extensions;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardImport : StandardBase
    {
        private const string FakeDutyText = ".";
        private const string ProviderName = "Training Provider";
        private const string EPAOName = "EPAO";
        public List<string> OptionsUnstructuredTemplate { get; set; }
        public List<Qualification> Qualifications { get; set; }
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
                CoronationEmblem = standard.CoronationEmblem,
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
                Duties = GetDuties(standard),
                CoreAndOptions = standard.CoreAndOptions,
                CoreDuties = coreDuties,
                IntegratedApprenticeship = SetIsIntegratedApprenticeship(standard),
                Options = CreateStructuredOptionsList(standard),
                OptionsUnstructuredTemplate = standard.OptionsUnstructuredTemplate ?? new List<string>(),
                RouteCode = standard.RouteCode,
                CreatedDate = standard.CreatedDate,
                EPAChanged = IsEPAChanged(standard),
                Qualifications = standard.Qualifications,
                Regulated = standard.Regulated,
                IsRegulatedForProvider = SetIsRegulated(standard, ProviderName),
                IsRegulatedForEPAO = SetIsRegulated(standard, EPAOName)
            };
        }

        public bool QualificationsContainsEpaoMustBeApprovedText()
        {
            var keyStrings = new string[]
            {
                "EPAO must be approved by regulator body",
                "EPAO must be approved by the regulator body",
            };

            return Qualifications.Any(q => q.AnyAdditionalInformation.ContainsSubstringIn(keyStrings, StringComparison.OrdinalIgnoreCase));
        }

        private static List<string> GetDuties(ImportTypes.Standard standard)
            => standard.Duties
                .EmptyEnumerableIfNull()
                .Select(duty => duty.DutyDetail)
                .Where(dutyText => dutyText != FakeDutyText)
                .ToList();

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

        private static IEnumerable<Guid> GetMappedSkillsList(Domain.ImportTypes.Standard standard)
        {
            return standard.Duties
                .Where(d => d.IsThisACoreDuty.Equals(1) && d.MappedSkills != null)
                .SelectMany(d => d.MappedSkills)
                .Select(s => s);
        }

        private static List<string> GetSkillDetailFromMappedCoreSkill(ImportTypes.Standard standard, IEnumerable<Guid> mappedSkillsList)
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
            return standard.CoreAndOptions
                ? CreateStructuredOptionsListWithDutyMapping(standard)
                : CreateStructuredOptionsListWithoutDutyMapping(standard);
        }

        private static List<StandardOption> CreateStructuredOptionsListWithDutyMapping(ImportTypes.Standard standard)
        {
            var options = standard.Options.EmptyEnumerableIfNull();
            var coreDuties = standard.Duties.Where(x => x.IsThisACoreDuty == 1).ToList();

            return options.Select(MapOption).ToList();

            StandardOption MapOption(ImportTypes.Option option)
                => StandardOption.Create(
                    option.OptionId,
                    option.Title?.Trim(),
                    MapKsbs(option));

            List<Ksb> MapKsbs(ImportTypes.Option option)
            {
                var knowledge = MapDuties(option, standard.Knowledge, x => x.MappedKnowledge, x => x.KnowledgeId, x => x.Detail, Ksb.Knowledge);
                var skills = MapDuties(option, standard.Skills, x => x.MappedSkills, x => x.SkillId, x => x.Detail, Ksb.Skill);
                var behaviour = MapDuties(option, standard.Behaviours, x => x.MappedBehaviour, x => x.BehaviourId, x => x.Detail, Ksb.Behaviour);

                return knowledge.Union(skills).Union(behaviour).DistinctBy(x => x.Key).ToList();
            }

            List<Ksb> MapDuties<Tksb>(
                ImportTypes.Option option,
                IEnumerable<Tksb> sequence,
                Func<ImportTypes.Duty, IEnumerable<Guid>> mappedSequence,
                Func<Tksb, Guid> selectId,
                Func<Tksb, string> selectDetail,
                Func<Guid, int, string, Ksb> createKsb)
            {
                return MapCoreDuties(sequence, mappedSequence, selectId)
                    .Union(MapOptionDuties(option, sequence, mappedSequence, selectId))
                    .Select(x => createKsb(selectId(x.ksb), x.index + 1, selectDetail(x.ksb)))
                    .ToList();
            }

            IEnumerable<(Tksb ksb, int index)> MapCoreDuties<Tksb>(
                IEnumerable<Tksb> sequence,
                Func<ImportTypes.Duty, IEnumerable<Guid>> innerSequence,
                Func<Tksb, Guid> selectId)
            {
                return sequence
                    .EmptyEnumerableIfNull()
                    .Select((x, i) => (x, i))
                    .Where(y => coreDuties
                        .SelectMany(x => innerSequence(x).EmptyEnumerableIfNull())
                        .Contains(selectId(y.x)));
            }

            IEnumerable<(Tksb ksb, int index)> MapOptionDuties<Tksb>(
                ImportTypes.Option option,
                IEnumerable<Tksb> sequence,
                Func<ImportTypes.Duty, IEnumerable<Guid>> innerSequence,
                Func<Tksb, Guid> selectId)
            {
                var dutiesForAllOptions = options.Select(x =>
                    (x.OptionId, standard.Duties.Where(y => y.MappedOptions?.Contains(x.OptionId) == true)))
                    .ToList();

                return sequence
                    .EmptyEnumerableIfNull()
                    .Select((x, i) => (x, i))
                    .Where(y => dutiesForAllOptions.Where(z => z.OptionId == option.OptionId)
                        .SelectMany(z => z.Item2)
                        .SelectMany(z => innerSequence(z).EmptyEnumerableIfNull())
                        .Contains(selectId(y.x)));
            }
        }

        private static List<StandardOption> CreateStructuredOptionsListWithoutDutyMapping(ImportTypes.Standard standard)
        {
            return new List<StandardOption>
            {
                StandardOption.CreateCorePseudoOption(
                    standard.Knowledge.Select((x,i) => Ksb.Knowledge(x.KnowledgeId, i + 1, x.Detail))
                        .Union(standard.Skills.Select((x,i) => Ksb.Skill(x.SkillId, i + 1, x.Detail)))
                        .Union(standard.Behaviours.Select((x,i) => Ksb.Behaviour(x.BehaviourId, i + 1, x.Detail)))
                        .DistinctBy(x => x.Key).ToList()
                )
            };
        }

        private static bool SetIsRegulated(Domain.ImportTypes.Standard standard, string name)
        {
            if (standard.RegulationDetail == null)
            {
                return false;
            }

            if (standard.RegulationDetail.Any(r => r.Name == name))
            {
                return standard.RegulationDetail.First(r => r.Name == name).Approved;
            }

            return false;
        }
    }
}
