using System;
using System.Collections.Generic;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Services
{
    public class StandardsImportServiceTestBase
    {
        protected StandardsImportServiceTestBase()
        {
        }

        public static Apprenticeship CreateValidImportedApprenticeship(int larsCode, string referenceNumber, string version, string title, string status, string route, string optionTitle)
        {
            return new Apprenticeship
            {
                ApprovedForDelivery = new Settable<DateTime?>(DateTime.UtcNow),
                AssessmentPlanUrl = new Settable<string>("http://example.com"),
                Behaviours = new Settable<List<Apprenticeship.Behaviour>>(new List<Apprenticeship.Behaviour>()),
                Change = new Settable<string>("Some change"),
                ChangedDate = new Settable<DateTime?>(null),
                CoreAndOptions = new Settable<bool>(true),
                CoronationEmblem = new Settable<bool>(false),
                CreatedDate = new Settable<DateTime>(DateTime.UtcNow),
                Duties = new Settable<List<Apprenticeship.Duty>>(new List<Apprenticeship.Duty>()),
                EqaProvider = new Settable<Apprenticeship.ApprenticeshipEqaProvider>(new Apprenticeship.ApprenticeshipEqaProvider
                {
                    ContactAddress = new Settable<string>("Address"),
                    ContactEmail = new Settable<string>("email@example.com"),
                    ContactName = new Settable<string>("Contact Name"),
                    ProviderName = new Settable<string>("Provider"),
                    WebLink = new Settable<string>("http://provider.com")
                }),
                IntegratedApprenticeship = new Settable<bool?>(null),
                IntegratedDegree = new Settable<string>(),
                Keywords = new Settable<List<string>>(new List<string>()),
                Knowledges = new Settable<List<Apprenticeship.Knowledge>>(new List<Apprenticeship.Knowledge>()),
                LarsCode = larsCode,
                LastUpdated = new Settable<DateTime?>(null),
                Level = new Settable<int>(5),
                Options = new List<Apprenticeship.Option> { new Apprenticeship.Option { OptionId = Guid.NewGuid(), Title = optionTitle } },
                OptionsUnstructuredTemplate = new Settable<List<string>>(new List<string>()),
                OverviewOfRole = new Settable<string>("Overview"),
                ProposedMaxFunding = new Settable<int>(5000),
                ProposedTypicalDuration = new Settable<int>(12),
                PublishDate = new Settable<DateTime>(DateTime.UtcNow),
                ReferenceNumber = referenceNumber,
                Regulated = new Settable<bool>(false),
                RegulatedBody = new Settable<string>("Regulator"),
                RegulationDetails = new Settable<List<Apprenticeship.RegulationDetail>>(new List<Apprenticeship.RegulationDetail>()),
                Route = route,
                RouteCode = new Settable<int>(0),
                Skills = new Settable<List<Apprenticeship.Skill>>(new List<Apprenticeship.Skill>()),
                StandardPageUrl = new Settable<Uri>(new Uri("http://standard.com")),
                Status = status,
                TbMainContact = new Settable<string>("Main Contact"),
                Title = title,
                TypicalJobTitles = new Settable<List<string>>(new List<string>()),
                Version = version,
                VersionEarliestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionLatestEndDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionLatestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionNumber = new Settable<string>("1.0"),
            };
        }

        public static FoundationApprenticeship CreateValidImportedFoundationApprenticeship(int larsCode, string referenceNumber, string version, string title, string status, string route)
        {
            return new FoundationApprenticeship
            {
                ApprovedForDelivery = new Settable<DateTime?>(DateTime.UtcNow),
                AssessmentChanged = new Settable<bool>(false),
                AssessmentPlanUrl = new Settable<string>("http://example.com"),
                Change = new Settable<string>("Some change"),
                ChangedDate = new Settable<DateTime?>(null),
                CreatedDate = new Settable<DateTime>(DateTime.UtcNow),
                EmployabilitySkillsAndBehaviours = new Settable<List<FoundationApprenticeship.IdDetailPair>>(new List<FoundationApprenticeship.IdDetailPair>()),
                EqaProvider = new Settable<FoundationApprenticeship.FoundationApprenticeshipEqaProvider>(new FoundationApprenticeship.FoundationApprenticeshipEqaProvider
                {
                    ContactAddress = new Settable<string>("Address"),
                    ContactEmail = new Settable<string>("email@example.com"),
                    ContactName = new Settable<string>("Contact Name"),
                    ProviderName = new Settable<string>("Provider"),
                    WebLink = new Settable<string>("http://provider.com")
                }),
                FoundationApprenticeshipUrl = new Settable<Uri>(new Uri("http://foundation.com")),
                Keywords = new Settable<List<string>>(new List<string>()),
                LarsCode = larsCode,
                LastUpdated = new Settable<DateTime?>(null),
                Level = new Settable<int>(5),
                OverviewOfRole = new Settable<string>("Overview"),
                ProposedMaxFunding = new Settable<int>(5000),
                ProposedTypicalDuration = new Settable<int>(12),
                PublishDate = new Settable<DateTime>(DateTime.UtcNow),
                ReferenceNumber = referenceNumber,
                Regulated = new Settable<bool>(false),
                RegulatedBody = new Settable<string>("Regulator"),
                RegulationDetails = new Settable<List<FoundationApprenticeship.RegulationDetail>>(new List<FoundationApprenticeship.RegulationDetail>()),
                RelatedOccupations = new Settable<List<FoundationApprenticeship.RelatedOccupation>>(new List<FoundationApprenticeship.RelatedOccupation>()),
                Route = route,
                RouteCode = new Settable<int>(0),
                Status = status,
                TechnicalKnowledges = new Settable<List<FoundationApprenticeship.IdDetailPair>>(new List<FoundationApprenticeship.IdDetailPair>()),
                TechnicalSkills = new Settable<List<FoundationApprenticeship.IdDetailPair>>(new List<FoundationApprenticeship.IdDetailPair>()),
                Title = title,
                TypicalJobTitles = new Settable<List<string>>(new List<string>()),
                Version = version,
                VersionEarliestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionLatestEndDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionLatestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionNumber = new Settable<string>("1.0"),
            };
        }

        public static ApprenticeshipUnit CreateValidImportedApprenticeshipUnit(string learningAimClassCode, string referenceNumber, string version, string title, string status, string route)
        {
            return new ApprenticeshipUnit
            {
                ApprovedForDelivery = new Settable<DateTime?>(DateTime.UtcNow),
                Change = new Settable<string>("Some change"),
                ChangedDate = new Settable<DateTime?>(null),
                CreatedDate = new Settable<DateTime>(DateTime.UtcNow),
                Keywords = new Settable<List<string>>(new List<string>()),
                LearningAimClassCode = learningAimClassCode,
                LastUpdated = new Settable<DateTime?>(null),
                Level = new Settable<int>(5),
                MinimumHoursForCompliance = new Settable<int>(1),
                ProposedMaxFunding = new Settable<int>(5000),
                PublishDate = new Settable<DateTime>(DateTime.UtcNow),
                ReferenceNumber = referenceNumber,
                Regulated = new Settable<bool>(false),
                RegulatedBody = new Settable<string>("Regulator"),
                RegulationDetails = new Settable<List<ApprenticeshipUnit.RegulationDetail>>(new List<ApprenticeshipUnit.RegulationDetail>()),
                Route = route,
                TechnicalKnowledges = new Settable<List<ApprenticeshipUnit.IdDetailPair>>(new List<ApprenticeshipUnit.IdDetailPair>()),
                TechnicalSkills = new Settable<List<ApprenticeshipUnit.IdDetailPair>>(new List<ApprenticeshipUnit.IdDetailPair>()),
                Status = status,
                Title = title,
                TypicalJobTitles = new Settable<List<string>>(new List<string>()),
                ApprenticeshipUnitUrl = new Settable<Uri>(new Uri("http://standard.com")),
                Version = version,
                VersionEarliestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionLatestEndDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionLatestStartDate = new Settable<DateTime?>(DateTime.UtcNow),
                VersionNumber = new Settable<string>("1.0"),
                WhoIsItFor = new Settable<string>("WhoIsItFor")
            };
        }

        public static Domain.Entities.StandardImport GetValidStandardImport(string larsCode, string referenceNumber, string version, string title, string status, int routeCode, string optionTitle)
        {
            return new Domain.Entities.StandardImport
            {
                LarsCode = larsCode,
                IfateReferenceNumber = referenceNumber,
                Title = title,
                Status = status,
                CreatedDate = DateTime.Now,
                PublishDate = DateTime.Now,
                Version = version,
                VersionEarliestStartDate = null,
                VersionLatestStartDate = null,
                VersionLatestEndDate = null,
                OverviewOfRole = null,
                Level = 1,
                ProposedTypicalDuration = 1,
                ProposedMaxFunding = 1,
                RouteCode = routeCode,
                Keywords = null,
                AssessmentPlanUrl = "http://plan.html",
                EqaProviderName = null,
                EqaProviderContactName = null,
                EqaProviderContactEmail = null,
                EqaProviderWebLink = null,
                ApprovedForDelivery = new Settable<DateTime?>(null),
                TrailBlazerContact = null,
                Duties = null,
                RegulatedBody = null,
                TypicalJobTitles = null,
                StandardPageUrl = null,
                CoreAndOptions = false,
                Options = new List<Domain.Entities.StandardOption> { Domain.Entities.StandardOption.Create(Guid.NewGuid(), optionTitle, new List<Domain.Entities.Ksb>()) },
                CoronationEmblem = false
            };
        }

        public static Domain.Entities.Standard GetValidStandard(string larsCode, string referenceNumber, string version, string title, string status, int routeCode, string optionTitle)
        {
            return new Domain.Entities.Standard
            {
                ApprenticeshipType = Domain.Entities.ApprenticeshipType.Apprenticeship,
                LarsCode = larsCode,
                IfateReferenceNumber = referenceNumber,
                Title = title,
                Status = status,
                CreatedDate = DateTime.Now,
                PublishDate = DateTime.Now,
                Version = version,
                VersionEarliestStartDate = null,
                VersionLatestStartDate = null,
                VersionLatestEndDate = null,
                OverviewOfRole = null,
                Level = 1,
                ProposedTypicalDuration = 1,
                ProposedMaxFunding = 1,
                RouteCode = routeCode,
                Keywords = null,
                AssessmentPlanUrl = "http://plan.html",
                EqaProviderName = null,
                EqaProviderContactName = null,
                EqaProviderContactEmail = null,
                EqaProviderWebLink = null,
                ApprovedForDelivery = new Settable<DateTime?>(null),
                TrailBlazerContact = null,
                Duties = null,
                RegulatedBody = null,
                TypicalJobTitles = null,
                StandardPageUrl = null,
                CoreAndOptions = false,
                Options = new List<Domain.Entities.StandardOption> { Domain.Entities.StandardOption.Create(Guid.NewGuid(), optionTitle, new List<Domain.Entities.Ksb>()) },
                CoronationEmblem = false
            };
        }
    }
}
