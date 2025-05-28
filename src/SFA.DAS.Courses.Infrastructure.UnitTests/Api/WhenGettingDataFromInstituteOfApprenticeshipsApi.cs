using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.ImportTypes;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.TestHelper.Extensions;
using SFA.DAS.Courses.Infrastructure.Api;


namespace SFA.DAS.Courses.Infrastructure.UnitTests.Api
{
    public class WhenGettingDataFromInstituteOfApprenticeshipsApi
    {
        private IOptions<CoursesConfiguration> _config;

        [SetUp]
        public void SetUp()
        {
            _config = Options.Create(new CoursesConfiguration
            {
                InstituteOfApprenticeshipsApiConfiguration = new("https://ifate.org", "https://ifate.org/standards", "https://ifate.org/foundation-apprenticeships")
            });
        }

        [Test]
        public async Task Then_The_Endpoint_Is_Called_And_Standards_Returned()
        {
            // Arrange
            var importedStandards = new List<Domain.ImportTypes.Standard>()
            {
                new Domain.ImportTypes.Standard
                {
                    LarsCode = 1,
                    ReferenceNumber = "ST0101",
                    Title = "Title 1",
                    Status = Status.ApprovedForDelivery,
                    CreatedDate = DateTime.Now,
                    PublishDate = DateTime.Now,
                    Version = "1.0",
                    VersionNumber = "1.0",
                    Change = new Settable<string>(null),
                    VersionEarliestStartDate = new Settable<DateTime?>(null),
                    VersionLatestStartDate = new Settable<DateTime?>(null),
                    VersionLatestEndDate = new Settable<DateTime?>(null),
                    OverviewOfRole = new Settable<string>(null),
                    Level = 1,
                    ProposedTypicalDuration = 1,
                    ProposedMaxFunding = 1,
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
                    TypicalJobTitles = new Settable<List<string>>(null),
                    StandardPageUrl = new Settable<Uri>(null),
                    CoreAndOptions = false,
                    Options = new List<Option> { new Option { OptionId = Guid.NewGuid(), Title = "Option 1" } },
                    OptionsUnstructuredTemplate = new Settable<List<string>>(null),
                    CoronationEmblem = false
                }
            };

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new SettableContractResolver()
            };

            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(importedStandards, settings)),
                StatusCode = HttpStatusCode.Accepted
            };

            var httpMessageHandler = MessageHandler.SetupGetMessageHandlerMock(response, new Uri(_config.Value.InstituteOfApprenticeshipsApiConfiguration.StandardsPath));
            var httpClient = new HttpClient(httpMessageHandler.Object);
            var sut = new InstituteOfApprenticeshipService(_config, httpClient);

            // Act
            var standards = await sut.GetStandards();

            // Assert
            standards.ShouldBeEquivalentToWithSettableHandling(importedStandards, options => options.Excluding(c => c.RouteCode));
        }

        [Test]
        public void Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.BadRequest
            };

            var httpMessageHandler = MessageHandler.SetupGetMessageHandlerMock(response, new Uri(_config.Value.InstituteOfApprenticeshipsApiConfiguration.StandardsPath));
            var httpClient = new HttpClient(httpMessageHandler.Object);
            var sut = new InstituteOfApprenticeshipService(_config, httpClient);

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(() => sut.GetStandards());
        }
    }
}
