using System;
using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland
{
    [InitializeSettables]
    public class Standard
    {
        #region Common properties between standards and foundation apprenticeships

        public ApprenticeshipType ApprenticeshipType { get; set; }
        public Settable<DateTime?> ApprovedForDelivery { get; set; }
        public Settable<string> AssessmentPlanUrl { get; set; }
        public Settable<string> Change { get; set; }
        public Settable<DateTime?> ChangedDate { get; set; }
        public CourseType CourseType { get; set; }
        public Settable<DateTime> CreatedDate { get; set; }
        public DurationUnits DurationUnits { get; set; }
        public Settable<EqaProvider> EqaProvider { get; set; }
        public Settable<List<string>> Keywords { get; set; }
        public Settable<string> LarsCode { get; set; }
        public Settable<DateTime?> LastUpdated { get; set; }
        public Settable<int> Level { get; set; }
        public Settable<string> OverviewOfRole { get; set; }
        public Settable<int> ProposedMaxFunding { get; set; }
        public Settable<int> ProposedTypicalDuration { get; set; }
        public Settable<DateTime> PublishDate { get; set; }
        public Settable<List<Qualification>> Qualifications { get; set; }
        public Settable<string> ReferenceNumber { get; set; }
        public Settable<bool> Regulated { get; set; }
        public Settable<string> RegulatedBody { get; set; }
        public Settable<List<RegulationDetail>> RegulationDetail { get; set; }
        public Settable<string> Route { get; set; }
        public Settable<int> RouteCode { get; set; }
        public Settable<string> Status { get; set; }
        public Settable<string> Title { get; set; }
        public Settable<List<string>> TypicalJobTitles { get; set; }
        public Settable<string> Version { get; set; }
        public Settable<DateTime?> VersionEarliestStartDate { get; set; }
        public Settable<DateTime?> VersionLatestEndDate { get; set; }
        public Settable<DateTime?> VersionLatestStartDate { get; set; }
        public Settable<string> VersionNumber { get; set; }


        #endregion Common properties between standards and foundation apprenticeships

        #region Only applicable to standards apprenticeships

        public Settable<List<Behaviour>> Behaviours { get; set; } = new Settable<List<Behaviour>>();
        public Settable<bool> CoreAndOptions { get; set; }
        public Settable<bool> CoronationEmblem { get; set; }
        public Settable<List<Duty>> Duties { get; set; } = new();
        public Settable<bool?> IntegratedApprenticeship { get; set; }
        public Settable<string> IntegratedDegree { get; set; } = new Settable<string>(string.Empty);
        public Settable<List<Knowledge>> Knowledges { get; set; } = new Settable<List<Knowledge>>();
        public Settable<List<Option>> Options { get; set; }
        public Settable<List<string>> OptionsUnstructuredTemplate { get; set; } = new Settable<List<string>>();
        public Settable<List<Skill>> Skills { get; set; } = new Settable<List<Skill>>();
        public Settable<Uri> StandardPageUrl { get; set; }
        public Settable<string> TbMainContact { get; set; } = new Settable<string>(string.Empty);

        #endregion Standards apprenticeships

        #region Only applicable to Foundation Apprenticeships

        public Settable<bool> AssessmentChanged { get; set; } = new();
        public Settable<List<IdDetailPair>> EmployabilitySkillsAndBehaviours { get; set; } = new Settable<List<IdDetailPair>>();
        public Settable<Uri> FoundationApprenticeshipUrl { get; set; } = new();
        public Settable<List<RelatedOccupation>> RelatedOccupations { get; set; } = new Settable<List<RelatedOccupation>>();
        public Settable<List<IdDetailPair>> TechnicalKnowledges { get; set; } = new Settable<List<IdDetailPair>>();
        public Settable<List<IdDetailPair>> TechnicalSkills { get; set; } = new Settable<List<IdDetailPair>>();

        #endregion Foundation Apprenticeships

        #region Only applicable to Apprenticeship Units

        #endregion  Apprenticeship Units

        public static implicit operator Standard(Apprenticeship apprenticeship)
        {
            if (apprenticeship == null)
                return null;

            var standard = new Standard
            {
                ApprenticeshipType = ApprenticeshipType.Apprenticeship,
                ApprovedForDelivery = apprenticeship.ApprovedForDelivery.Clone(),
                AssessmentPlanUrl = apprenticeship.AssessmentPlanUrl.Clone(),
                Behaviours = apprenticeship.Behaviours.MapList(p => new Behaviour
                {
                    BehaviourId = p.BehaviourId.Clone(),
                    Detail = p.Detail.Clone()
                }),
                Change = apprenticeship.Change.Clone(),
                ChangedDate = apprenticeship.ChangedDate.Clone(),
                CoreAndOptions = apprenticeship.CoreAndOptions.Clone(),
                CoronationEmblem = apprenticeship.CoronationEmblem.Clone(),
                CourseType = CourseType.Apprenticeship,
                CreatedDate = apprenticeship.CreatedDate.Clone(),
                DurationUnits = DurationUnits.Months,
                Duties = apprenticeship.Duties.MapList(p => new Duty
                {
                    DutyDetail = p.DutyDetail.Clone(),
                    DutyId = p.DutyId.Clone(),
                    IsThisACoreDuty = p.IsThisACoreDuty.Clone(),
                    MappedBehaviour = p.MappedBehaviour.MapList(p => p),
                    MappedKnowledge = p.MappedKnowledge.MapList(p => p),
                    MappedOptions = p.MappedOptions.MapList(p => p),
                    MappedSkills = p.MappedSkills.MapList(p => p)
                }),
                EqaProvider = apprenticeship.EqaProvider.Map(p => new EqaProvider
                {
                    ContactAddress = p.ContactAddress.Clone(),
                    ContactEmail = p.ContactEmail.Clone(),
                    ContactName = p.ContactName.Clone(),
                    ProviderName = p.ProviderName.Clone(),
                    WebLink = p.WebLink.Clone(),
                }),
                IntegratedApprenticeship = apprenticeship.IntegratedApprenticeship.Clone(),
                IntegratedDegree = apprenticeship.IntegratedDegree.Clone(),
                Keywords = apprenticeship.Keywords.MapList(p => p),
                Knowledges = apprenticeship.Knowledges.MapList(p => new Knowledge
                {
                    Detail = p.Detail.Clone(),
                    KnowledgeId = p.KnowledgeId.Clone()
                }),
                LarsCode = apprenticeship.LarsCode.Map(i => i.ToString()),
                LastUpdated = apprenticeship.LastUpdated.Clone(),
                Level = apprenticeship.Level.Clone(),
                Options = apprenticeship.Options.MapList(p => new Option
                {
                    OptionId = p.OptionId.Clone(),
                    Title = p.Title.Clone()
                }),
                OptionsUnstructuredTemplate = apprenticeship.OptionsUnstructuredTemplate.MapList(p => p),
                OverviewOfRole = apprenticeship.OverviewOfRole.Clone(),
                ProposedMaxFunding = apprenticeship.ProposedMaxFunding.Clone(),
                ProposedTypicalDuration = apprenticeship.ProposedTypicalDuration.Clone(),
                PublishDate = apprenticeship.PublishDate.Clone(),
                Qualifications = apprenticeship.Qualifications.MapList(p => new Qualification
                {
                    AnyAdditionalInformation = p.AnyAdditionalInformation.Clone(),
                    Level = p.Level.Clone(),
                    QualificationId = p.QualificationId.Clone(),
                    Title = p.Title.Clone()
                }),
                ReferenceNumber = apprenticeship.ReferenceNumber.Clone(),
                Regulated = apprenticeship.Regulated.Clone(),
                RegulatedBody = apprenticeship.RegulatedBody.Clone(),
                RegulationDetail = apprenticeship.RegulationDetails.MapList(p => new RegulationDetail
                {
                    Approved = p.Approved.Clone(),
                    Name = p.Name.Clone(),
                    WebLink = p.WebLink.Clone()
                }),
                Route = apprenticeship.Route.Clone(),
                RouteCode = apprenticeship.RouteCode.Clone(),
                Skills = apprenticeship.Skills.MapList(p => new Skill
                {
                    Detail = p.Detail.Clone(),
                    SkillId = p.SkillId.Clone()
                }),
                StandardPageUrl = apprenticeship.StandardPageUrl.Clone(),
                Status = apprenticeship.Status.Clone(),
                TbMainContact = apprenticeship.TbMainContact.Clone(),
                Title = apprenticeship.Title.Clone(),
                TypicalJobTitles = apprenticeship.TypicalJobTitles.MapList(p => p),
                Version = apprenticeship.Version.Clone(),
                VersionEarliestStartDate = apprenticeship.VersionEarliestStartDate.Clone(),
                VersionLatestEndDate = apprenticeship.VersionLatestEndDate.Clone(),
                VersionLatestStartDate = apprenticeship.VersionLatestStartDate.Clone(),
                VersionNumber = apprenticeship.VersionNumber.Clone(),
            };

            InitializeSettablesHelper.InitializeSettableProperties(standard);
            return standard;
        }

        public static implicit operator Standard(FoundationApprenticeship foundationApprenticeship)
        {
            if (foundationApprenticeship == null)
                return null;

            var standard = new Standard
            {
                ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship,
                ApprovedForDelivery = foundationApprenticeship.ApprovedForDelivery.Clone(),
                AssessmentChanged = foundationApprenticeship.AssessmentChanged.Clone(),
                AssessmentPlanUrl = foundationApprenticeship.AssessmentPlanUrl.Clone(),
                Change = foundationApprenticeship.Change.Clone(),
                ChangedDate = foundationApprenticeship.ChangedDate.Clone(),
                CourseType = CourseType.Apprenticeship,
                CreatedDate = foundationApprenticeship.CreatedDate.Clone(),
                DurationUnits = DurationUnits.Months,
                EmployabilitySkillsAndBehaviours = foundationApprenticeship.EmployabilitySkillsAndBehaviours.MapList(p => new IdDetailPair
                {
                    Detail = p.Detail.Clone(),
                    Id = p.Id.Clone()
                }),
                EqaProvider = foundationApprenticeship.EqaProvider.Map(p => new EqaProvider
                {
                    ContactAddress = p.ContactAddress.Clone(),
                    ContactEmail = p.ContactEmail.Clone(),
                    ContactName = p.ContactName.Clone(),
                    ProviderName = p.ProviderName.Clone(),
                    WebLink = p.WebLink.Clone(),
                }),
                FoundationApprenticeshipUrl = foundationApprenticeship.FoundationApprenticeshipUrl.Clone(),
                Keywords = foundationApprenticeship.Keywords.MapList(p => p),
                LarsCode = foundationApprenticeship.LarsCode.Map(i => i.ToString()),
                LastUpdated = foundationApprenticeship.LastUpdated.Clone(),
                Level = foundationApprenticeship.Level.Clone(),
                OverviewOfRole = foundationApprenticeship.OverviewOfRole.Clone(),
                ProposedMaxFunding = foundationApprenticeship.ProposedMaxFunding.Clone(),
                ProposedTypicalDuration = foundationApprenticeship.ProposedTypicalDuration.Clone(),
                PublishDate = foundationApprenticeship.PublishDate.Clone(),
                Qualifications = foundationApprenticeship.Qualifications.MapList(p => new Qualification
                {
                    AnyAdditionalInformation = p.AnyAdditionalInformation.Clone(),
                    Level = p.Level.Clone(),
                    QualificationId = p.QualificationId.Clone(),
                    Title = p.Title.Clone()
                }),
                ReferenceNumber = foundationApprenticeship.ReferenceNumber.Clone(),
                Regulated = foundationApprenticeship.Regulated.Clone(),
                RegulatedBody = foundationApprenticeship.RegulatedBody.Clone(),
                RegulationDetail = foundationApprenticeship.RegulationDetails.MapList(p => new RegulationDetail
                {
                    Approved = p.Approved.Clone(),
                    Name = p.Name.Clone(),
                    WebLink = p.WebLink.Clone()
                }),
                RelatedOccupations = foundationApprenticeship.RelatedOccupations.MapList(p => new RelatedOccupation
                {
                    Name = p.Name.Clone(),
                    Reference = p.Reference.Clone()
                }),
                Route = foundationApprenticeship.Route.Clone(),
                RouteCode = foundationApprenticeship.RouteCode.Clone(),
                Status = foundationApprenticeship.Status.Clone(),
                TechnicalKnowledges = foundationApprenticeship.TechnicalKnowledges.MapList(p => new IdDetailPair
                {
                    Detail = p.Detail.Clone(),
                    Id = p.Id.Clone()
                }),
                TechnicalSkills = foundationApprenticeship.TechnicalSkills.MapList(p => new IdDetailPair
                {
                    Detail = p.Detail.Clone(),
                    Id = p.Id.Clone()
                }),
                Title = foundationApprenticeship.Title.Clone(),
                TypicalJobTitles = foundationApprenticeship.TypicalJobTitles.MapList(p => p),
                Version = foundationApprenticeship.Version.Clone(),
                VersionEarliestStartDate = foundationApprenticeship.VersionEarliestStartDate.Clone(),
                VersionLatestEndDate = foundationApprenticeship.VersionLatestEndDate.Clone(),
                VersionLatestStartDate = foundationApprenticeship.VersionLatestStartDate.Clone(),
                VersionNumber = foundationApprenticeship.VersionNumber.Clone(),
            };

            InitializeSettablesHelper.InitializeSettableProperties(standard);
            return standard;
        }


        public static implicit operator Standard(ApprenticeshipUnit apprenticeshipUnit)
        {
            if (apprenticeshipUnit == null)
                return null;

            var standard = new Standard
            {
                ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                ApprovedForDelivery = apprenticeshipUnit.ApprovedForDelivery.Clone(),
                Change = apprenticeshipUnit.Change.Clone(),
                ChangedDate = apprenticeshipUnit.ChangedDate.Clone(),
                CourseType = CourseType.ShortCourse,
                CreatedDate = apprenticeshipUnit.CreatedDate.Clone(),
                DurationUnits = DurationUnits.Hours,
                Keywords = apprenticeshipUnit.Keywords.MapList(p => p),
                Knowledges = apprenticeshipUnit.Knowledges.MapList(p => new Knowledge
                {
                    Detail = p.Detail.Clone(),
                    KnowledgeId = p.KnowledgeId.Clone()
                }),
                LarsCode = apprenticeshipUnit.LarsCode.Clone(),
                LastUpdated = apprenticeshipUnit.LastUpdated.Clone(),
                Level = apprenticeshipUnit.Level.Clone(),
                OverviewOfRole = apprenticeshipUnit.OverviewOfRole.Clone(),
                ProposedMaxFunding = apprenticeshipUnit.ProposedMaxFunding.Clone(),
                ProposedTypicalDuration = apprenticeshipUnit.LearningHours.Clone(),
                PublishDate = apprenticeshipUnit.PublishDate.Clone(),
                ReferenceNumber = apprenticeshipUnit.ReferenceNumber.Clone(),
                Regulated = apprenticeshipUnit.Regulated.Clone(),
                RegulatedBody = apprenticeshipUnit.RegulatedBody.Clone(),
                RegulationDetail = apprenticeshipUnit.RegulationDetails.MapList(p => new RegulationDetail
                {
                    Approved = p.Approved.Clone(),
                    Name = p.Name.Clone(),
                    WebLink = p.WebLink.Clone()
                }),
                Route = apprenticeshipUnit.Route.Clone(),
                Skills = apprenticeshipUnit.Skills.MapList(p => new Skill
                {
                    Detail = p.Detail.Clone(),
                    SkillId = p.SkillId.Clone()
                }),
                Status = apprenticeshipUnit.Status.Clone(),
                Title = apprenticeshipUnit.Title.Clone(),
                TypicalJobTitles = apprenticeshipUnit.TypicalJobTitles.MapList(p => p),
                StandardPageUrl = apprenticeshipUnit.Url.Clone(),
                Version = apprenticeshipUnit.Version.Clone(),
                VersionEarliestStartDate = apprenticeshipUnit.VersionEarliestStartDate.Clone(),
                VersionLatestEndDate = apprenticeshipUnit.VersionLatestEndDate.Clone(),
                VersionLatestStartDate = apprenticeshipUnit.VersionLatestStartDate.Clone(),
                VersionNumber = apprenticeshipUnit.VersionNumber.Clone(),
            };

            InitializeSettablesHelper.InitializeSettableProperties(standard);
            return standard;
        }
    }

    public class EqaProvider
    {
        public Settable<string> ProviderName { get; set; }
        public Settable<string> ContactName { get; set; }
        public Settable<string> ContactAddress { get; set; }
        public Settable<string> ContactEmail { get; set; }
        public Settable<string> WebLink { get; set; }
    }

    public class Option
    {
        public Settable<Guid> OptionId { get; set; }
        public Settable<string> Title { get; set; }
    }

    public class Skill
    {
        public Settable<Guid> SkillId { get; set; }
        public Settable<string> Detail { get; set; }
    }

    public class Duty
    {
        public Settable<Guid> DutyId { get; set; }
        public Settable<string> DutyDetail { get; set; }
        public Settable<long> IsThisACoreDuty { get; set; }
        public Settable<List<Guid>> MappedBehaviour { get; set; }
        public Settable<List<Guid>> MappedKnowledge { get; set; }
        public Settable<List<Guid>> MappedOptions { get; set; }
        public Settable<List<Guid>> MappedSkills { get; set; }
    }

    public class Behaviour
    {
        public Settable<Guid> BehaviourId { get; set; }
        public Settable<string> Detail { get; set; }
    }

    public class Knowledge
    {
        public Settable<Guid> KnowledgeId { get; set; }
        public Settable<string> Detail { get; set; }
    }

    public class RegulationDetail
    {
        public Settable<string> Name { get; set; }
        public Settable<string> WebLink { get; set; }
        public Settable<bool> Approved { get; set; }
    }

    public class Qualification
    {
        public Settable<string> QualificationId { get; set; }
        public Settable<string> Title { get; set; }
        public Settable<string> Level { get; set; }
        public Settable<string> AnyAdditionalInformation { get; set; }
    }

    public class IdDetailPair
    {
        public Settable<Guid> Id { get; set; }
        public Settable<string> Detail { get; set; }
    }

    public class RelatedOccupation
    {
        public Settable<string> Name { get; set; } = new Settable<string>(string.Empty);
        public Settable<string> Reference { get; set; } = new Settable<string>(string.Empty);
    }
}
