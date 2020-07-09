using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Infrastructure.Api;
using SFA.DAS.Courses.Infrastructure.HealthCheck;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Infrastructure.UnitTests.Api
{
    public class WhenGettingDataFromLars
    {
        [Test, AutoData]
        public async Task Then_The_File_Is_Downloaded_From_The_Url(string content)
        {
            //Arrange
            var response = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(content))),
                StatusCode = HttpStatusCode.Accepted
            };
            var downloadUrl = "https://test";
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(downloadUrl));
            var client = new HttpClient(httpMessageHandler.Object);
            var larsDataDownloadService = new LarsDataDownloadService(client);

            //Act
            var actual = await larsDataDownloadService.GetFileStream(downloadUrl);

            //Assert
            var reader = new StreamReader(actual);
            var actualContent = reader.ReadToEnd();
            actualContent.Should().Be(content);
        }

        [Test]
        public void Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown()
        {
            //Arrange
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.BadRequest
            };
            var downloadUrl = "https://test.zip";
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(downloadUrl));
            var client = new HttpClient(httpMessageHandler.Object);
            var larsDataDownloadService = new LarsDataDownloadService(client);

            //Act Assert
            Assert.ThrowsAsync<HttpRequestException>(() => larsDataDownloadService.GetFileStream(downloadUrl));
        }

        [Test, MoqAutoData]
        public async Task Then_The_Latest_ImportAudit_Record_is_Read_From_Lars([Frozen] Mock<IImportAuditRepository> mock, HealthCheckContext healthCheckContext, LarsHealthCheck handler)
        {
            //Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.LarsImport)).ReturnsAsync(new ImportAudit(
                DateTime.UtcNow.AddDays(14), 10));

            //Act
            var expect = "LARS Input Health Check";
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            //Assert
            Assert.AreEqual(expect, actual.Description);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Data_Is_Greater_Than_Two_Weeks_And_An_Hour_Then_HealthCheck_Return_Degraded([Frozen] Mock<IImportAuditRepository> mock, HealthCheckContext healthCheckContext, LarsHealthCheck handler)
        {
            //Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.LarsImport)).ReturnsAsync(new ImportAudit(
                DateTime.UtcNow.AddDays(14).AddHours(2), 0));

            //Act
            var expect = HealthStatus.Degraded;
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            //Assert
            Assert.AreEqual(actual.Status, expect);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Data_Is_Less_Than_Two_Weeks_And_An_Hour_Then_HealthCheck_Return_Healthy([Frozen] Mock<IImportAuditRepository> mock, HealthCheckContext healthCheckContext, LarsHealthCheck handler)
        {
            //Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.LarsImport)).ReturnsAsync(new ImportAudit(
                DateTime.UtcNow.AddDays(10), 6));

            //Act
            var expect = HealthStatus.Healthy;
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            //Assert
            Assert.IsTrue(actual.Status.Equals(expect));
        }
    }
}
