using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Azure;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Infrastructure.Api;
using SFA.DAS.Courses.Infrastructure.Exceptions;

namespace SFA.DAS.Courses.Infrastructure.UnitTests.Api
{
    [TestFixture]
    public class WhenNotifiyingValidationFailuresViaSlackPost
    {
        private IOptions<SlackNotificationConfiguration> _config;

        [SetUp]
        public void SetUp()
        {
            _config = Options.Create(new SlackNotificationConfiguration
            {
                BotUserOAuthToken = "test-token",
                Channel = "test-channel",
                User = "test-user"
            });
        }

        [Test]
        public async Task UploadFile_Should_Throw_Exception_When_Slack_Start_Upload_Fails()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new { ok = false, error = "upload_failed" }))
            };

            var httpMessageHandler = MessageHandler.SetupPostMessageHandlerMock(response, new Uri(Constants.SlackStartUploadUrl));
            var httpClient = new HttpClient(httpMessageHandler.Object);
            var sut = new SlackNotificationService(_config, httpClient);

            var content = new List<string> { "Test content" };

            // Act
            Func<Task> act = async () => await sut.UploadFile(content, "test.txt", "Test message");

            // Assert
            await act.Should().ThrowAsync<SlackNotificationException>()
                .WithMessage("Failed to get Slack upload URL: upload_failed");
        }

        [Test]
        public void FormattedUser_Should_Return_Correct_Format()
        {
            // Arrange
            var httpClient = new Mock<HttpClient>();
            var sut = new SlackNotificationService(_config, httpClient.Object);

            // Act
            var result = sut.FormattedUser();

            // Assert
            result.Should().Be("<@test-user>");
        }
    }
}
