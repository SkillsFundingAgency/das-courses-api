using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using SFA.DAS.Courses.Domain.TestHelper.Extensions;
using SFA.DAS.Courses.Infrastructure.Api;

namespace SFA.DAS.Courses.Infrastructure.UnitTests.Api
{
    public class WhenGettingDataFromSkillsEnglandApi
    {
        private IOptions<CoursesConfiguration> _config;

        [SetUp]
        public void SetUp()
        {
            _config = Options.Create(new CoursesConfiguration
            {
                SkillsEnglandApiConfiguration = new(
                    "https://skillsengland.test.gov.uk",
                    "https://skillsengland.gov.uk/apprenticeships",
                    "https://skillsengland.gov.uk/foundation-apprenticeships",
                    "https://skillsengland.gov.uk/apprenticeship-units")
            });
        }

        [Test]
        public async Task Then_The_Endpoint_Is_Called_And_Standards_Returned()
        {
            // Arrange
            var now = DateTime.UtcNow;

            var apprenticeshipsApiPayload = new[]
            {
                new
                {
                    larsCode = 1,
                    referenceNumber = "ST0101",
                    title = "Standard 1",
                    status = "Approved for delivery",
                    createdDate = now,
                    publishDate = now,
                    version = "1.0",
                    versionNumber = "1.0",
                    assessmentPlanUrl = "http://plan.html",
                    coreAndOptions = false,
                    options = new[]
                    {
                        new { optionId = Guid.NewGuid(), title = "Option 1" }
                    }
                }
            };

            var foundationsApiPayload = new[]
            {
                new
                {
                    larsCode = 10,
                    referenceNumber = "FA0110",
                    title = "Foundation 1",
                    status = "Approved for delivery",
                    createdDate = now,
                    publishDate = now,
                    version = "1.0",
                    versionNumber = "1.0",
                    assessmentPlanUrl = "http://plan.html",
                    technicalKnowledges = new[]
                    {
                        new { id = Guid.NewGuid(), detail = "Knowledge 1" }
                    },
                    technicalSkills = new[]
                    {
                        new { id = Guid.NewGuid(), detail = "Skill 1" }
                    },
                    employabilitySkillsAndBehaviours = new[]
                    {
                        new { id = Guid.NewGuid(), detail = "Skill and Behaviour 1" }
                    },
                    assessmentChanged = false
                }
            };

            var unitsApiPayload = new[]
            {
                new
                {
                    larsCode = "ZSC00123",
                    referenceNumber = "SC0123",
                    title = "Apprenticeship Unit 1",
                    status = "Approved for delivery",
                    createdDate = now,
                    publishDate = now,
                    version = "1.0",
                    versionNumber = "1.0",
                    learningHours = 12,
                    level = 2,
                    maxFunding = 500,
                    overviewOfRole = "Aprenticeship unit overview",
                    url = "https://skillsengland.gov.uk/apprenticeship-units/SC0001"
                }
            };

            var apprenticeshipsJson = JsonConvert.SerializeObject(apprenticeshipsApiPayload);
            var foundationsJson = JsonConvert.SerializeObject(foundationsApiPayload);
            var unitsJson = JsonConvert.SerializeObject(unitsApiPayload);

            var deserialiseSettings = new JsonSerializerSettings
            {
                ContractResolver = new SettableContractResolver(),
                Converters = [new InitializeSettablesJsonConverter()]
            };

            var expectedApprenticeships =
                JsonConvert.DeserializeObject<IEnumerable<Apprenticeship>>(apprenticeshipsJson, deserialiseSettings)!
                    .Select(x => (Standard)x);

            var expectedFoundations =
                JsonConvert.DeserializeObject<IEnumerable<FoundationApprenticeship>>(foundationsJson, deserialiseSettings)!
                    .Select(x => (Standard)x);

            var expectedUnits =
                JsonConvert.DeserializeObject<IEnumerable<ApprenticeshipUnit>>(unitsJson, deserialiseSettings)!
                    .Select(x => (Standard)x);

            var expected = expectedApprenticeships
                .Concat(expectedFoundations)
                .Concat(expectedUnits)
                .ToList();

            var standardsResponse = new HttpResponseMessage
            {
                Content = new StringContent(apprenticeshipsJson),
                StatusCode = HttpStatusCode.Accepted
            };

            var foundationResponse = new HttpResponseMessage
            {
                Content = new StringContent(foundationsJson),
                StatusCode = HttpStatusCode.Accepted
            };

            var unitsResponse = new HttpResponseMessage
            {
                Content = new StringContent(unitsJson),
                StatusCode = HttpStatusCode.Accepted
            };

            Mock<HttpMessageHandler> httpMessageHandler = MessageHandler
                .SetupGetMessageHandlerMock(
                    standardsResponse,
                    new Uri(_config.Value.SkillsEnglandApiConfiguration.StandardsPath))
                .AddHandler(
                    foundationResponse,
                    new Uri(_config.Value.SkillsEnglandApiConfiguration.FoundationApprenticeshipsPath))
                .AddHandler(
                    unitsResponse,
                    new Uri(_config.Value.SkillsEnglandApiConfiguration.ApprenticeshipUnitsPath));

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var sut = new SkillsEnglandService(_config, httpClient);

            // Act
            var actual = (await sut.GetStandards()).ToList();

            // Assert
            actual.ShouldBeEquivalentToWithSettableHandling(
                expected,
                options => options.Excluding(c => c.RouteCode)
            );

            actual.First(s => s.LarsCode == "1").ApprenticeshipType
                .Should().Be(SFA.DAS.Courses.Domain.Entities.ApprenticeshipType.Apprenticeship);

            actual.First(s => s.LarsCode == "10").ApprenticeshipType
                .Should().Be(SFA.DAS.Courses.Domain.Entities.ApprenticeshipType.FoundationApprenticeship);

            actual.First(s => s.LarsCode == "ZSC00123").ApprenticeshipType
                .Should().Be(SFA.DAS.Courses.Domain.Entities.ApprenticeshipType.ApprenticeshipUnit);
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

            var httpMessageHandler = MessageHandler.SetupGetMessageHandlerMock(
                response,
                new Uri(_config.Value.SkillsEnglandApiConfiguration.StandardsPath));

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var sut = new SkillsEnglandService(_config, httpClient);

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(() => sut.GetStandards());
        }
    }
}
