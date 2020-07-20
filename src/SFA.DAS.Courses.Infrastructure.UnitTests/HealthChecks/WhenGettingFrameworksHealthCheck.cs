using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Infrastructure.HealthCheck;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Infrastructure.UnitTests.HealthChecks
{
    public class WhenGettingFrameworksHealthCheck
    {
        [Test, MoqAutoData]
        public async Task Then_The_ImportAudit_Frameworks_Record_Is_Read_From_The_Repository(
            [Frozen] Mock<IImportAuditRepository> repository, 
            HealthCheckContext healthCheckContext, 
            FrameworksHealthCheck healthCheck)
        {
            //Act
            await healthCheck.CheckHealthAsync(healthCheckContext);

            //Assert
            repository.Verify(x=>x.GetLastImportByType(ImportType.FrameworkImport), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Frameworks_Are_Loaded_Then_Shows_As_Degraded(
            [Frozen] Mock<IImportAuditRepository> repository, 
            HealthCheckContext healthCheckContext, 
            FrameworksHealthCheck healthCheck)
        {
            // Arrange
            repository.Setup(x => x.GetLastImportByType(ImportType.FrameworkImport))
                .ReturnsAsync((ImportAudit) null);
                
            // Act
            var actual = await healthCheck.CheckHealthAsync(healthCheckContext);

            // Assert
            actual.Status.Should().Be(HealthStatus.Degraded);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Frameworks_Are_Loaded_Shows_File_Name_Loaded_And_Healthy(
            string fileName,
            [Frozen] Mock<IImportAuditRepository> repository, 
            HealthCheckContext healthCheckContext, 
            FrameworksHealthCheck healthCheck)
        {
            // Arrange
            
            repository.Setup(x => x.GetLastImportByType(ImportType.FrameworkImport))
                .ReturnsAsync(new ImportAudit(DateTime.Now,100,ImportType.FrameworkImport,$"/test/test/some/{fileName}"));
                
            // Act
            var actual = await healthCheck.CheckHealthAsync(healthCheckContext);

            // Assert
            actual.Status.Should().Be(HealthStatus.Healthy);
            Assert.IsTrue(actual.Data["FileName"].Equals(fileName));
        }
    }
}