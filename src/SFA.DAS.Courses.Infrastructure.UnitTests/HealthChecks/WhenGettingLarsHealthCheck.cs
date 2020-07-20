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
    public class WhenGettingLarsHealthCheck
    {
        [Test, MoqAutoData]
        public async Task Then_The_Latest_ImportAudit_Record_is_Read_From_Lars(
            [Frozen] Mock<IImportAuditRepository> mock, 
            HealthCheckContext healthCheckContext, 
            LarsHealthCheck handler)
        {
            //Act
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            //Assert
            mock.Verify(x => x.GetLastImportByType(ImportType.LarsImport), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Data_Is_Greater_Than_Two_Weeks_And_An_Hour_Then_HealthCheck_Return_Degraded(
            [Frozen] Mock<IImportAuditRepository> mock, 
            HealthCheckContext healthCheckContext, 
            LarsHealthCheck handler)
        {
            //Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.LarsImport)).ReturnsAsync(new ImportAudit(
                 DateTime.UtcNow.AddDays(14).AddHours(1).AddMinutes(1), 0));

            //Act
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            //Assert
            Assert.AreEqual(HealthStatus.Degraded, actual.Status);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Data_Is_Less_Than_Two_Weeks_And_An_Hour_Then_HealthCheck_Return_Healthy_With_File_Name(
            string fileName,
            [Frozen] Mock<IImportAuditRepository> mock, 
            HealthCheckContext healthCheckContext, 
            LarsHealthCheck handler)
        {
            //Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.LarsImport)).ReturnsAsync(new ImportAudit(
                DateTime.UtcNow.AddDays(10), 6,ImportType.LarsImport, $"test/test/test/{fileName}"));

            //Act
            var actual = await handler.CheckHealthAsync(healthCheckContext);

            //Assert
            Assert.AreEqual(HealthStatus.Healthy, actual.Status);
            Assert.IsTrue(actual.Data["FileName"].Equals(fileName));
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Data_Is_Less_Than_Two_Weeks_And_An_Hour_But_Has_No_Imported_Rows_Then_HealthCheck_Return_Degraded(
            [Frozen] Mock<IImportAuditRepository> mock,
            HealthCheckContext healthCheckContext,
            LarsHealthCheck handler)
        {
            //Arrange
            mock.Setup(x => x.GetLastImportByType(ImportType.LarsImport)).ReturnsAsync(new ImportAudit(
                DateTime.UtcNow.AddDays(10), 0));
            //Act
            var actual = await handler.CheckHealthAsync(healthCheckContext);
            //Assert
            Assert.AreEqual(HealthStatus.Degraded, actual.Status);
        }
    }
}
