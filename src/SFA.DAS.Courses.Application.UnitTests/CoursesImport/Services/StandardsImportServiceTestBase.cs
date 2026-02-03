using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Services
{
    public class StandardsImportServiceTestBase
    {
        protected static Standard GetValidImportedStandard(int larsCode, string referenceNumber, string version, string title, string status, string route, string optionTitle)
        {
            return new Standard
            {
                LarsCode = larsCode,
                ReferenceNumber = referenceNumber,
                Title = title,
                Status = status,
                CreatedDate = DateTime.Now,
                PublishDate = DateTime.Now,
                Version = version,
                VersionNumber = version,
                Change = new Settable<string>(null),
                VersionEarliestStartDate = new Settable<DateTime?>(null),
                VersionLatestStartDate = new Settable<DateTime?>(null),
                VersionLatestEndDate = new Settable<DateTime?>(null),
                OverviewOfRole = new Settable<string>(null),
                Level = 1,
                ProposedTypicalDuration = 1,
                ProposedMaxFunding = 1,
                Route = route,
                Keywords = new Settable<List<string>>(null),
                AssessmentPlanUrl = "http://plan.html",
                EqaProvider = new Domain.ImportTypes.EqaProvider
                {
                    ProviderName = new Settable<string>(null),
                    ContactName = new Settable<string>(null),
                    ContactAddress = new Settable<string>(null),
                    ContactEmail = new Settable<string>(null),
                    WebLink = new Settable<string>(null),
                },
                ApprovedForDelivery = new Settable<DateTime?>(null),
                TbMainContact = new Settable<string>(null),
                Knowledges = new Settable<List<Knowledge>>(null),
                Behaviours = new Settable<List<Behaviour>>(null),
                Skills = new Settable<List<Skill>>(null),
                Duties = new Settable<List<Duty>>(null),
                RegulatedBody = new Settable<string>(null),
                Regulated = new Settable<bool>(true),
                RegulationDetail = new Settable<List<RegulationDetail>>(null),
                TypicalJobTitles = new Settable<List<string>>(null),
                StandardPageUrl = new Settable<Uri>(null),
                CoreAndOptions = false,
                Options = new List<Option> { new Option { OptionId = Guid.NewGuid(), Title = optionTitle } },
                OptionsUnstructuredTemplate = new Settable<List<string>>(null),
                CoronationEmblem = false
            };
        }

        protected static Domain.Entities.StandardImport GetValidStandardImport(int larsCode, string referenceNumber, string version, string title, string status, int routeCode, string optionTitle)
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
                Options = JsonConvert.SerializeObject(new List<Domain.Entities.StandardOption> { Domain.Entities.StandardOption.Create(Guid.NewGuid(), optionTitle, new List<Domain.Entities.Ksb>()) }),
                CoronationEmblem = false
            };
        }

        protected static Domain.Entities.Standard GetValidStandard(int larsCode, string referenceNumber, string version, string title, string status, int routeCode, string optionTitle)
        {
            return new Domain.Entities.Standard
            {
                ApprenticeshipType = Domain.Entities.ApprenticeshipType.Apprenticeship.ToString(),
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
                Options = JsonConvert.SerializeObject(new List<Domain.Entities.StandardOption> { Domain.Entities.StandardOption.Create(Guid.NewGuid(), optionTitle, new List<Domain.Entities.Ksb>()) }),
                CoronationEmblem = false
            };
        }
    }
}
