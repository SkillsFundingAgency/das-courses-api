using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Infrastructure.Api;
using SFA.DAS.Courses.Infrastructure.Exceptions;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System;
using SFA.DAS.Courses.Infrastructure.UnitTests.Helper;
using System.Collections.Generic;
using FluentAssertions;

namespace SFA.DAS.Courses.Infrastructure.UnitTests.Api
{
    [TestFixture]
    public class WhenUploadingFileToSlack
    {
        private SlackNotificationConfiguration _config;
        private IOptions<SlackNotificationConfiguration> _configOptions;

        [SetUp]
        public void SetUp()
        {
            _config = new SlackNotificationConfiguration
            {
                BotUserOAuthToken = "test-token",
                Channel = "test-channel",
                User = "@test-user"
            };
            _configOptions = Options.Create(_config);
        }

        [Test]
        public async Task UploadFile_Should_Return_Without_Calling_Api_If_Disabled()
        {
            // Arrang
            var config = Options.Create(new SlackNotificationConfiguration());
            var handlerMock = new Mock<HttpMessageHandler>();
            var client = new HttpClient(handlerMock.Object);
            var sut = new SlackNotificationService(config, client);

            // Act
            await sut.UploadFile(new List<string> { "test" }, "test.txt", "msg");

            // Assert
            handlerMock.Protected()
                .Verify("SendAsync", Times.Never(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task UploadFile_Should_Complete_Successfully()
        {
            // Arrange
            var uploadStartResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ok = true,
                    upload_url = "https://upload.slack.com/files.upload",
                    file_id = "F12345"
                }))
            };

            var fileUploadResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var completeUploadResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { ok = true }))
            };

            var handler = new SequentialHttpMessageHandler(new List<HttpResponseMessage>
            {
                uploadStartResponse,
                fileUploadResponse,
                completeUploadResponse
            });

            var client = new HttpClient(handler);
            var sut = new SlackNotificationService(_configOptions, client);

            // Act 
            await sut.UploadFile(new List<string> { "line1", "line2" }, "file.txt", "upload message");

            // Assert - does not throw then success
            Assert.Pass("Upload completed without error.");
        }

        [Test]
        public async Task UploadFile_Should_Throw_If_FileUpload_Fails()
        {
            // Arrange
            var uploadStartResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ok = true,
                    upload_url = "https://upload.slack.com/files.upload",
                    file_id = "F12345"
                }))
            };

            var failedUpload = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var handler = new SequentialHttpMessageHandler(new List<HttpResponseMessage>
            {
                uploadStartResponse,
                failedUpload
            });

            var client = new HttpClient(handler);
            var sut = new SlackNotificationService(_configOptions, client);

            // Act
            Func<Task> act = async () => await sut.UploadFile(new List<string> { "test" }, "test.txt", "fail");

            // Assert
            await act.Should().ThrowAsync<SlackNotificationException>()
                .WithMessage("File upload to Slack failed.");
        }

        [Test]
        public async Task UploadFile_Should_Throw_If_CompleteUpload_Fails()
        {
            // Arrange
            var uploadStartResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ok = true,
                    upload_url = "https://upload.slack.com/files.upload",
                    file_id = "F12345"
                }))
            };

            var fileUploadResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var failComplete = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { ok = false, error = "failed_final" }))
            };

            var handler = new SequentialHttpMessageHandler(new List<HttpResponseMessage>
            {
                uploadStartResponse,
                fileUploadResponse,
                failComplete
            });

            var client = new HttpClient(handler);
            var sut = new SlackNotificationService(_configOptions, client);

            // Act
            Func<Task> act = async () => await sut.UploadFile(new List<string> { "x" }, "x.txt", "x");

            // Assert
            await act.Should().ThrowAsync<SlackNotificationException>()
                .WithMessage("Failed to complete Slack file upload: failed_final");
        }

        [Test]
        public void FormattedTag_Should_Return_User_Tag()
        {
            var httpClient = new HttpClient();
            var sut = new SlackNotificationService(_configOptions, httpClient);

            var result = sut.FormattedTag();

            result.Should().Be("<@test-user>");
        }
    }
}
