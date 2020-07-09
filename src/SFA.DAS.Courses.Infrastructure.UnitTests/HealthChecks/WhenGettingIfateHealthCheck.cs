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
        public async Task Then_The_Latest_ImportAudit_Record_is_Read_From_IFATE([Frozen] Mock<IImportAuditRepository> mock, HealthCheckContext healthCheckContext, InstituteOfApprenticeshipServiceHealthCheckHealthCheck handler)
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
        public async Task Then_If_The_Data_Is_Greater_Than_Twenty_Five_Hours_Then_HealthCheck_Return_Degraded([Frozen] Mock<IImportAuditRepository> mock, HealthCheckContext healthCheckContext, InstituteOfApprenticeshipServiceHealthCheckHealthCheck handler)
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
        public async Task Then_If_The_Data_Is_Less_Than_Twenty_Five_Hours_Then_HealthCheck_Return_Healthy([Frozen] Mock<IImportAuditRepository> mock, HealthCheckContext healthCheckContext, InstituteOfApprenticeshipServiceHealthCheckHealthCheck handler)
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
