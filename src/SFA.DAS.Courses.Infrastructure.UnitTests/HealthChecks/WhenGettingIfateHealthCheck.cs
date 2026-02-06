using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Infrastructure.HealthCheck;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Infrastructure.UnitTests.HealthChecks
{
    public class WhenGettingIfateHealthCheck
    {
        [Test, MoqAutoData]
        public async Task Then_The_Latest_ImportAudit_Record_is_Read_From_IFATE(
            [Frozen] Mock<IImportAuditRepository> mock, 
            HealthCheckContext healthCheckContext, 
            SkillsEnglandServiceHealthCheck handler)
        {
            //Act
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            //Assert
            mock.Verify(x => x.GetLastImportByType(ImportType.SkillsEnglandImport), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Data_Is_Greater_Than_Twenty_Five_Hours_Then_HealthCheck_Return_Degraded(
            [Frozen] Mock<IImportAuditRepository> mock, 
            HealthCheckContext healthCheckContext, 
            SkillsEnglandServiceHealthCheck handler)
        {
            // Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.SkillsEnglandImport)).ReturnsAsync(new ImportAudit(
            DateTime.UtcNow.AddHours(26), 0));

            // Act
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            // Assert
            Assert.That(actual.Status, Is.EqualTo(HealthStatus.Degraded));

        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Data_Is_Less_Than_Twenty_Five_Hours_Then_HealthCheck_Return_Healthy(
            [Frozen] Mock<IImportAuditRepository> mock, 
            HealthCheckContext healthCheckContext, 
            SkillsEnglandServiceHealthCheck handler)
        {
            // Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.SkillsEnglandImport)).ReturnsAsync(new ImportAudit(
            DateTime.UtcNow.AddHours(10), 10));

            // Act
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            // Assert
            Assert.That(actual.Status, Is.EqualTo(HealthStatus.Healthy));
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Data_Is_Less_Than_Two_Weeks_And_An_Hour_But_Has_No_Imported_Rows_Then_HealthCheck_Return_Degraded(
            [Frozen] Mock<IImportAuditRepository> mock,
            HealthCheckContext healthCheckContext,
             SkillsEnglandServiceHealthCheck handler)
        {
            //Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.SkillsEnglandImport)).ReturnsAsync(new ImportAudit(
                DateTime.UtcNow.AddDays(10), 0));
            //Act
            var actual = await handler.CheckHealthAsync(healthCheckContext);
            //Assert
            Assert.That(actual.Status, Is.EqualTo(HealthStatus.Degraded));
        }
    }
}
