using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Extensions;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardImport : StandardBase
    {
        private const string FakeDutyText = ".";

        public static implicit operator StandardImport(ImportTypes.Standard standard)
        {
            var coreDuties = new List<string>();

            if ((standard.Duties.Value?.Any() ?? false) && (standard.Skills.Value?.Any() ?? false))
            {
                coreDuties = GetSkillDetailFromMappedCoreSkill(standard);
            }

            return new StandardImport
            {
                ApprenticeshipType = standard.ApprenticeshipType.ToString(),
                ApprovedForDelivery = standard.ApprovedForDelivery.Value,
                AssessmentPlanUrl = standard.AssessmentPlanUrl.Value,
                CoreAndOptions = standard.CoreAndOptions.Value,
                CoreDuties = coreDuties,
                CoronationEmblem = standard.CoronationEmblem.Value,
                CreatedDate = standard.CreatedDate.Value,
                Duties = GetDuties(standard),
                EPAChanged = IsEPAChanged(standard),
                EpaoMustBeApprovedByRegulatorBody = QualificationsContainsEpaoMustBeApprovedText(standard.Qualifications?.Value),
                EqaProviderContactEmail = standard.EqaProvider.Value?.ContactEmail.Value?.Trim(),
                EqaProviderContactName = standard.EqaProvider.Value?.ContactName.Value?.Trim(),
                EqaProviderName = standard.EqaProvider.Value?.ProviderName.Value?.Trim(),
                EqaProviderWebLink = standard.EqaProvider.Value?.WebLink.Value,
                IfateReferenceNumber = standard.ReferenceNumber.Value?.Trim(),
                IntegratedApprenticeship = SetIsIntegratedApprenticeship(standard),
                IntegratedDegree = standard.IntegratedDegree?.Value,
                IsRegulatedForProvider = GetIsRegulated(standard, Constants.ProviderRegulationType),
                IsRegulatedForEPAO = GetIsRegulated(standard, Constants.EPAORegulationType),
                Keywords = (standard.Keywords.Value?.Any() ?? false) ? string.Join("|", standard.Keywords.Value) : null,
                LarsCode = standard.LarsCode.Value,
                Level = standard.Level.Value,
                Options = CreateStructuredOptionsList(standard),
                OverviewOfRole = standard.OverviewOfRole.Value,
                ProposedMaxFunding = standard.ProposedMaxFunding.Value,
                ProposedTypicalDuration = standard.ProposedTypicalDuration.Value,
                PublishDate = standard.PublishDate.Value,
                RegulatedBody = standard.RegulatedBody.Value?.Trim(),
                RouteCode = standard.RouteCode.Value,
                StandardPageUrl = GetStandardPageUrl(standard),
                StandardUId = standard.ReferenceNumber.Value?.ToStandardUId(standard.Version?.Value),
                Status = standard.Status.Value?.Trim(),
                Title = standard.Title.Value?.Trim(),
                TrailBlazerContact = standard.TbMainContact.Value?.Trim(),
                TypicalJobTitles = (standard.TypicalJobTitles.Value?.Any() ?? false) ? string.Join("|", standard.TypicalJobTitles.Value) : string.Empty,
                Version = (standard.Version?.Value).ToBaselineVersion(),
                VersionEarliestStartDate = standard.VersionEarliestStartDate.Value,
                VersionLatestEndDate = standard.VersionLatestEndDate.Value,
                VersionLatestStartDate = standard.VersionLatestStartDate.Value,
                VersionMajor = GetVersionPart(standard.Version?.Value, VersionPart.Major),
                VersionMinor = GetVersionPart(standard.Version?.Value, VersionPart.Minor),
                RelatedOccupations = GetRelatedOccupationsStandardCodes(standard)
            };
        }

        private static List<string> GetRelatedOccupationsStandardCodes(ImportTypes.Standard standard)
        {
            if (standard.ApprenticeshipType == Entities.ApprenticeshipType.FoundationApprenticeship)
            {
                return standard.RelatedOccupations?.Value.Select(r => $"ST{r.Reference.Value.Substring(3, 4)}").Distinct().ToList();
            }
            return [];
        }

        private static string GetStandardPageUrl(ImportTypes.Standard standard)
            => standard.ApprenticeshipType switch
            {
                Entities.ApprenticeshipType.FoundationApprenticeship => standard.FoundationApprenticeshipUrl?.Value?.AbsoluteUri,
                _ => standard.StandardPageUrl?.Value?.AbsoluteUri
            };

        public static implicit operator StandardImport(Standard standard)
        {
            return new StandardImport
            {
                ApprovedForDelivery = standard.ApprovedForDelivery,
                AssessmentPlanUrl = standard.AssessmentPlanUrl,
                CoreAndOptions = standard.CoreAndOptions,
                CoreDuties = standard.CoreDuties,
                CoronationEmblem = standard.CoronationEmblem,
                CreatedDate = standard.CreatedDate,
                Duties = standard.Duties,
                EPAChanged = standard.EPAChanged,
                EpaoMustBeApprovedByRegulatorBody = standard.EpaoMustBeApprovedByRegulatorBody,
                EqaProviderContactEmail = standard.EqaProviderContactEmail,
                EqaProviderContactName = standard.EqaProviderContactName,
                EqaProviderName = standard.EqaProviderName,
                EqaProviderWebLink = standard.EqaProviderWebLink,
                IfateReferenceNumber = standard.IfateReferenceNumber,
                IntegratedApprenticeship = standard.IntegratedApprenticeship,
                IntegratedDegree = standard.IntegratedDegree,
                IsRegulatedForProvider = standard.IsRegulatedForProvider,
                IsRegulatedForEPAO = standard.IsRegulatedForEPAO,
                Keywords = standard.Keywords,
                LarsCode = standard.LarsCode,
                Level = standard.Level,
                Options = standard.Options,
                OverviewOfRole = standard.OverviewOfRole,
                ProposedMaxFunding = standard.ProposedMaxFunding,
                ProposedTypicalDuration = standard.ProposedTypicalDuration,
                PublishDate = standard.PublishDate,
                RegulatedBody = standard.RegulatedBody,
                Route = standard.Route,
                RouteCode = standard.RouteCode,
                StandardPageUrl = standard.StandardPageUrl,
                StandardUId = standard.StandardUId,
                Status = standard.Status,
                Title = standard.Title,
                TrailBlazerContact = standard.TrailBlazerContact,
                TypicalJobTitles = standard.TypicalJobTitles,
                Version = standard.Version,
                VersionEarliestStartDate = standard.VersionEarliestStartDate,
                VersionLatestEndDate = standard.VersionLatestEndDate,
                VersionLatestStartDate = standard.VersionLatestStartDate,
                VersionMajor = standard.VersionMajor,
                VersionMinor = standard.VersionMinor
            };

        }

        public static bool QualificationsContainsEpaoMustBeApprovedText(List<Qualification> qualifications)
        {
            var keyStrings = new string[]
            {
                "EPAO must be approved by regulator body",
                "EPAO must be approved by the regulator body",
            };

            return qualifications
                .EmptyEnumerableIfNull()
                .Any(q => q.AnyAdditionalInformation?.ContainsSubstringIn(keyStrings, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        private static List<string> GetDuties(ImportTypes.Standard standard)
            => standard.Duties.Value
                .EmptyEnumerableIfNull()
                .Select(duty => duty.DutyDetail?.Value)
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

        private static bool SetIsIntegratedApprenticeship(ImportTypes.Standard standard)
        {
            if (standard.Level >= 6)
            {
                return standard.IntegratedDegree?.Value?.Equals("integrated degree", StringComparison.CurrentCultureIgnoreCase) ?? false;
            }

            if (standard.Level <= 5 && (standard.IntegratedApprenticeship?.Value.HasValue ?? false))
            {
                return standard.IntegratedApprenticeship.Value.Value;
            }

            return false;
        }

        private static List<string> GetSkillDetailFromMappedCoreSkill(ImportTypes.Standard standard)
        {
            var mappedSkillsList = standard.Duties.Value
                .EmptyEnumerableIfNull()
                .Where(d => (d.IsThisACoreDuty?.Value.Equals(1) ?? false) && d.MappedSkills?.Value != null)
                .SelectMany(d => d.MappedSkills.Value);

            return standard.Skills.Value
                .EmptyEnumerableIfNull()
                .Where(s => mappedSkillsList.Contains(s.SkillId.Value))
                .Select(s => s.Detail.Value).ToList();
        }

        private static bool IsEPAChanged(ImportTypes.Standard standard)
        {
            if (standard.ApprenticeshipType == Entities.ApprenticeshipType.FoundationApprenticeship)
            {
                return standard.AssessmentChanged.Value;
            }
            if (string.IsNullOrWhiteSpace(standard.Change.Value)) return false;

            return standard.Change.Value.Contains("End-point assessment plan revised", StringComparison.OrdinalIgnoreCase);
        }

        private static List<StandardOption> CreateStructuredOptionsList(ImportTypes.Standard standard)
        {
            var standardOptions = standard.CoreAndOptions.Value
                ? CreateStructuredOptionsListWithDutyMapping(standard)
                : CreateStructuredOptionsListWithoutDutyMapping(standard);

            if (standardOptions.Any())
            {
                return standardOptions;
            }
            else if (standard.OptionsUnstructuredTemplate?.Value?.Any() ?? false)
            {
                return standard.OptionsUnstructuredTemplate.Value.Select(StandardOption.Create).ToList();
            }

            return [];
        }

        private static List<StandardOption> CreateStructuredOptionsListWithDutyMapping(ImportTypes.Standard standard)
        {
            var options = (standard.Options?.Value)
                .EmptyEnumerableIfNull();

            var coreDuties = standard.Duties.Value
                .EmptyEnumerableIfNull()
                .Where(x => x.IsThisACoreDuty?.Value == 1)
                .ToList();

            return options.Select(MapOption).ToList();

            StandardOption MapOption(ImportTypes.Option option)
                => StandardOption.Create(
                    option.OptionId.Value,
                    option.Title?.Value?.Trim(),
                    MapKsbs(option));

            List<Ksb> MapKsbs(ImportTypes.Option option)
            {
                var knowledge = MapDuties(option, standard.Knowledges.Value, x => x.MappedKnowledge?.Value, x => x.KnowledgeId.Value, x => x.Detail.Value, Ksb.Knowledge);
                var skills = MapDuties(option, standard.Skills.Value, x => x.MappedSkills?.Value, x => x.SkillId.Value, x => x.Detail.Value, Ksb.Skill);
                var behaviour = MapDuties(option, standard.Behaviours.Value, x => x.MappedBehaviour?.Value, x => x.BehaviourId.Value, x => x.Detail.Value, Ksb.Behaviour);

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
                    (x.OptionId, standard.Duties?.Value.Where(y => y.MappedOptions?.Value?.Contains(x.OptionId.Value) == true)))
                    .ToList();

                return sequence
                    .EmptyEnumerableIfNull()
                    .Select((x, i) => (x, i))
                    .Where(y => dutiesForAllOptions.Where(z => z.OptionId.Value == option.OptionId.Value)
                        .SelectMany(z => z.Item2)
                        .SelectMany(z => innerSequence(z).EmptyEnumerableIfNull())
                        .Contains(selectId(y.x)));
            }
        }

        private static List<StandardOption> CreateStructuredOptionsListWithoutDutyMapping(ImportTypes.Standard standard)
        {
            var ksbs = standard.ApprenticeshipType == Entities.ApprenticeshipType.Apprenticeship
                ? standard.Knowledges.Value?.Select((x, i) => Ksb.Knowledge(x.KnowledgeId.Value, i + 1, x.Detail.Value))
                    .Union(standard.Skills.Value?.Select((x, i) => Ksb.Skill(x.SkillId.Value, i + 1, x.Detail.Value)))
                    .Union(standard.Behaviours.Value?.Select((x, i) => Ksb.Behaviour(x.BehaviourId.Value, i + 1, x.Detail.Value)))
                : standard.TechnicalKnowledges.Value?.Select((x, i) => Ksb.TechnicalKnowledge(x.Id.Value, i + 1, x.Detail.Value))
                    .Union(standard.TechnicalSkills.Value?.Select((x, i) => Ksb.TechnicalSkill(x.Id.Value, i + 1, x.Detail.Value)))
                    .Union(standard.EmployabilitySkillsAndBehaviours.Value?.Select((x, i) => Ksb.EmployabilitySkillsAndBehaviour(x.Id.Value, i + 1, x.Detail.Value)));

            return new List<StandardOption>
            {
                StandardOption.CreateCorePseudoOption(ksbs?.DistinctBy(x => x.Key).ToList())
            };
        }

        private static bool GetIsRegulated(ImportTypes.Standard standard, string name)
        {
            if (standard.RegulationDetail.Value == null || !standard.Regulated.Value || string.IsNullOrEmpty(standard.RegulatedBody.Value))
            {
                return false;
            }

            if (standard.RegulationDetail.Value.Any(r => r.Name == name))
            {
                return standard.RegulationDetail.Value.First(r => r.Name == name).Approved;
            }

            return false;
        }
    }
}
