using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Services;
using SFA.DAS.Courses.Data;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Services
{
    public class WhenLoadingStandardsFromStaging : StandardsImportServiceTestBase
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Data_Is_Deleted_From_The_Standard_And_Route_Tables(
            DateTime importStartTime,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            StandardsImportService standardsImportService)
        {
            // Arrange
            SetupTransaction(mockCoursesDataContext);

            var routesImports = new List<Domain.Entities.RouteImport>
            {
                new Domain.Entities.RouteImport { Id = 1, Name = "Route 1", Active = true },
            };

            var standardsImports = new List<Domain.Entities.StandardImport>
            {
                GetValidStandardImport("101", "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
            };

            routeImportRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(routesImports);

            standardImportRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(standardsImports);

            standardRepository
                .Setup(s => s.InsertMany(It.IsAny<List<Domain.Entities.Standard>>()))
                .ReturnsAsync(1);

            // Act
            await standardsImportService.LoadDataFromStaging(importStartTime);

            // Assert
            standardRepository.Verify(x => x.DeleteAll(), Times.Once);
            routeRepository.Verify(x => x.DeleteAll(), Times.Once);

            mockCoursesDataContext.Verify(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Data_Is_Loaded_From_The_Staging_Tables(
            DateTime importStartTime,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            StandardsImportService standardsImportService)
        {
            // Arrange
            SetupTransaction(mockCoursesDataContext);

            var routesImports = new List<Domain.Entities.RouteImport>
            {
                new Domain.Entities.RouteImport { Id = 1, Name = "Route 1", Active = true },
            };

            var standardsImports = new List<Domain.Entities.StandardImport>
            {
                GetValidStandardImport("101", "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
                GetValidStandardImport("102", "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, 1, "Option 2"),
            };

            routeImportRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(routesImports);

            standardImportRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(standardsImports);

            standardRepository
                .Setup(s => s.InsertMany(It.IsAny<List<Domain.Entities.Standard>>()))
                .ReturnsAsync(2);

            // Act
            await standardsImportService.LoadDataFromStaging(importStartTime);

            // Assert
            standardRepository.Verify(x => x.InsertMany(It.Is<List<Domain.Entities.Standard>>(c => c.Count.Equals(2))), Times.Once);
            routeRepository.Verify(x => x.InsertMany(It.Is<List<Domain.Entities.Route>>(c => c.Count.Equals(1))), Times.Once);

            mockCoursesDataContext.Verify(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_There_Is_No_Data_In_The_Staging_Table_It_Is_Not_Loaded_Into_Standards_Table_And_Nothing_Is_Deleted(
            DateTime importStartTime,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<IImportAuditRepository> importAuditRepository,
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            StandardsImportService standardsImportService)
        {
            // Arrange
            SetupTransaction(mockCoursesDataContext);

            routeImportRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.RouteImport>());

            standardImportRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.StandardImport>());

            // Act
            await standardsImportService.LoadDataFromStaging(importStartTime);

            // Assert
            standardRepository.Verify(x => x.DeleteAll(), Times.Never);
            standardRepository.Verify(x => x.InsertMany(It.IsAny<List<Domain.Entities.Standard>>()), Times.Never);
            routeRepository.Verify(x => x.DeleteAll(), Times.Never);
            routeRepository.Verify(x => x.InsertMany(It.IsAny<List<Domain.Entities.Route>>()), Times.Never);
            importAuditRepository.Verify(x => x.Insert(It.Is<Domain.Entities.ImportAudit>(c => c.RowsImported.Equals(0))), Times.Once);

            mockCoursesDataContext.Verify(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_It_Is_Audited_After_The_Run(
            DateTime importStartTime,
            [Frozen] Mock<ICoursesDataContext> mockCoursesDataContext,
            [Frozen] Mock<IImportAuditRepository> importAuditRepository,
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            StandardsImportService standardsImportService)
        {
            // Arrange
            SetupTransaction(mockCoursesDataContext);

            var routesImports = new List<Domain.Entities.RouteImport>
            {
                new Domain.Entities.RouteImport { Id = 1, Name = "Route 1", Active = true },
            };

            var standardsImports = new List<Domain.Entities.StandardImport>
            {
                GetValidStandardImport("101", "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
                GetValidStandardImport("102", "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, 1, "Option 2"),
            };

            routeImportRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(routesImports);

            standardImportRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(standardsImports);

            standardRepository
                .Setup(s => s.InsertMany(It.IsAny<List<Domain.Entities.Standard>>()))
                .ReturnsAsync(2);

            // Act
            await standardsImportService.LoadDataFromStaging(importStartTime);

            // Assert
            importAuditRepository.Verify(x =>
                x.Insert(It.Is<Domain.Entities.ImportAudit>(c => c.RowsImported.Equals(2))), Times.Once);

            mockCoursesDataContext.Verify(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private static void SetupTransaction(
            Mock<ICoursesDataContext> mockCoursesDataContext)
        {
            mockCoursesDataContext
                .Setup(x => x.ExecuteInTransactionAsync(
                    It.IsAny<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .Returns<Func<Task>, CancellationToken>(
                    async (operation, _) => await operation());
        }
    }
}
