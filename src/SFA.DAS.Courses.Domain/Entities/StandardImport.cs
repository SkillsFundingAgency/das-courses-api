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

        public static implicit operator StandardImport(ImportTypes.Standard source)
        {
            if (source == null)
                return null;

            var coreDuties = new List<string>();

            if ((source.Duties.Value?.Any() ?? false) && (source.Skills.Value?.Any() ?? false))
            {
                coreDuties = GetSkillDetailFromMappedCoreSkill(source);
            }

            return new StandardImport
            {
                ApprenticeshipType = source.ApprenticeshipType.ToString(),
                ApprovedForDelivery = source.ApprovedForDelivery.Value,
                AssessmentPlanUrl = source.AssessmentPlanUrl.Value,
                CoreAndOptions = source.CoreAndOptions.Value,
                CoreDuties = coreDuties,
                CoronationEmblem = source.CoronationEmblem.Value,
                CreatedDate = source.CreatedDate.Value,
                Duties = GetDuties(source),
                EPAChanged = IsEPAChanged(source),
                EpaoMustBeApprovedByRegulatorBody = QualificationsContainsEpaoMustBeApprovedText(source.Qualifications?.Value),
                EqaProviderContactEmail = source.EqaProvider.Value?.ContactEmail.Value?.Trim(),
                EqaProviderContactName = source.EqaProvider.Value?.ContactName.Value?.Trim(),
                EqaProviderName = source.EqaProvider.Value?.ProviderName.Value?.Trim(),
                EqaProviderWebLink = source.EqaProvider.Value?.WebLink.Value,
                IfateReferenceNumber = source.ReferenceNumber.Value?.Trim(),
                IntegratedApprenticeship = SetIsIntegratedApprenticeship(source),
                IntegratedDegree = source.IntegratedDegree?.Value,
                IsRegulatedForProvider = GetIsRegulated(source, Constants.ProviderRegulationType),
                IsRegulatedForEPAO = GetIsRegulated(source, Constants.EPAORegulationType),
                Keywords = (source.Keywords.Value?.Any() ?? false) ? string.Join("|", source.Keywords.Value) : null,
                LarsCode = source.LarsCode.Value.ToString(),
                Level = source.Level.Value,
                Options = CreateStructuredOptionsList(source),
                OverviewOfRole = source.OverviewOfRole.Value,
                ProposedMaxFunding = source.ProposedMaxFunding.Value,
                ProposedTypicalDuration = source.ProposedTypicalDuration.Value,
                PublishDate = source.PublishDate.Value,
                RegulatedBody = source.RegulatedBody.Value?.Trim(),
                RouteCode = source.RouteCode.Value,
                StandardPageUrl = GetStandardPageUrl(source),
                StandardUId = source.ReferenceNumber.Value?.ToStandardUId(source.Version?.Value),
                Status = source.Status.Value?.Trim(),
                Title = source.Title.Value?.Trim(),
                TrailBlazerContact = source.TbMainContact.Value?.Trim(),
                TypicalJobTitles = (source.TypicalJobTitles.Value?.Any() ?? false) ? string.Join("|", source.TypicalJobTitles.Value) : string.Empty,
                Version = (source.Version?.Value).ToBaselineVersion(),
                VersionEarliestStartDate = source.VersionEarliestStartDate.Value,
                VersionLatestEndDate = source.VersionLatestEndDate.Value,
                VersionLatestStartDate = source.VersionLatestStartDate.Value,
                VersionMajor = GetVersionPart(source.Version?.Value, VersionPart.Major),
                VersionMinor = GetVersionPart(source.Version?.Value, VersionPart.Minor),
                RelatedOccupations = GetRelatedOccupationsStandardCodes(source)
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

        public static implicit operator StandardImport(Standard source)
        {
            if (source == null)
                return null;

            return new StandardImport
            {
                ApprovedForDelivery = source.ApprovedForDelivery,
                AssessmentPlanUrl = source.AssessmentPlanUrl,
                CoreAndOptions = source.CoreAndOptions,
                CoreDuties = source.CoreDuties,
                CoronationEmblem = source.CoronationEmblem,
                CreatedDate = source.CreatedDate,
                Duties = source.Duties,
                EPAChanged = source.EPAChanged,
                EpaoMustBeApprovedByRegulatorBody = source.EpaoMustBeApprovedByRegulatorBody,
                EqaProviderContactEmail = source.EqaProviderContactEmail,
                EqaProviderContactName = source.EqaProviderContactName,
                EqaProviderName = source.EqaProviderName,
                EqaProviderWebLink = source.EqaProviderWebLink,
                IfateReferenceNumber = source.IfateReferenceNumber,
                IntegratedApprenticeship = source.IntegratedApprenticeship,
                IntegratedDegree = source.IntegratedDegree,
                IsRegulatedForProvider = source.IsRegulatedForProvider,
                IsRegulatedForEPAO = source.IsRegulatedForEPAO,
                Keywords = source.Keywords,
                LarsCode = source.LarsCode,
                Level = source.Level,
                Options = source.Options,
                OverviewOfRole = source.OverviewOfRole,
                ProposedMaxFunding = source.ProposedMaxFunding,
                ProposedTypicalDuration = source.ProposedTypicalDuration,
                PublishDate = source.PublishDate,
                RegulatedBody = source.RegulatedBody,
                Route = source.Route,
                RouteCode = source.RouteCode,
                StandardPageUrl = source.StandardPageUrl,
                StandardUId = source.StandardUId,
                Status = source.Status,
                Title = source.Title,
                TrailBlazerContact = source.TrailBlazerContact,
                TypicalJobTitles = source.TypicalJobTitles,
                Version = source.Version,
                VersionEarliestStartDate = source.VersionEarliestStartDate,
                VersionLatestEndDate = source.VersionLatestEndDate,
                VersionLatestStartDate = source.VersionLatestStartDate,
                VersionMajor = source.VersionMajor,
                VersionMinor = source.VersionMinor,
                RelatedOccupations = source.RelatedOccupations,
                ApprenticeshipType = source.ApprenticeshipType.ToString()
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
