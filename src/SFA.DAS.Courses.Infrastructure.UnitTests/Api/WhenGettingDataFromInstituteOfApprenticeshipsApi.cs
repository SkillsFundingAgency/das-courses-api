using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Courses.Data.Configuration;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Infrastructure.Api;
using SFA.DAS.Courses.Infrastructure.HealthCheck;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Infrastructure.UnitTests.Api
{
    public class WhenGettingDataFromInstituteOfApprenticeshipsApi
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called_And_Standards_Returned(List<Domain.ImportTypes.Standard> importStandards)
        {
            //Arrange
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(importStandards)),
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(Constants.InstituteOfApprenticeshipsStandardsUrl));
            var client = new HttpClient(httpMessageHandler.Object);
            var apprenticeshipService = new InstituteOfApprenticeshipService(client);
            
            //Act
            var standards = await apprenticeshipService.GetStandards();
            
            //Assert
            standards.Should().BeEquivalentTo(importStandards, options => options.Excluding(c=>c.RouteId));
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
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri(Constants.InstituteOfApprenticeshipsStandardsUrl));
            var client = new HttpClient(httpMessageHandler.Object);
            var apprenticeshipService = new InstituteOfApprenticeshipService(client);
            
            //Act Assert
            Assert.ThrowsAsync<HttpRequestException>(() => apprenticeshipService.GetStandards());
        }

        [Test, MoqAutoData]
        public async Task Then_The_Latest_ImportAudit_Record_is_Read_From_IFATE([Frozen] Mock<IImportAuditRepository> mock, HealthCheckContext healthCheckContext, iFateHealthCheck handler)
        {
            //Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.IFATEImport)).ReturnsAsync(new Domain.Entities.ImportAudit(
                DateTime.UtcNow.AddHours(1), 10));

            //Act
            var expect = "iFate Input Health Check";
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            //Assert
            Assert.AreEqual(expect, actual.Description);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Data_Is_Greater_Than_Twenty_Five_Hours_Then_HealthCheck_Return_Degraded([Frozen] Mock<IImportAuditRepository> mock, HealthCheckContext healthCheckContext, iFateHealthCheck handler)
        {
            // Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.IFATEImport)).ReturnsAsync(new Domain.Entities.ImportAudit(
            DateTime.UtcNow.AddHours(26), 0));

            // Act
            var expect = HealthStatus.Degraded;
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            // Assert
            Assert.AreEqual(actual.Status, expect);

        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Data_Is_Less_Than_Twenty_Five_Hours_Then_HealthCheck_Return_Healthy([Frozen] Mock<IImportAuditRepository> mock, HealthCheckContext healthCheckContext, iFateHealthCheck handler)
        {
            // Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.IFATEImport)).ReturnsAsync(new Domain.Entities.ImportAudit(
            DateTime.UtcNow.AddHours(10), 10));

            // Act
            var expect = HealthStatus.Healthy;
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            // Assert
            Assert.AreEqual(actual.Status, expect);
        }
    }
}
