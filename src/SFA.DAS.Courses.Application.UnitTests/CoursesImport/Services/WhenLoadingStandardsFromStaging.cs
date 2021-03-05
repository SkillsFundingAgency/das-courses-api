using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Services
{
    public class WhenLoadingStandardsFromStaging
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Data_Is_Deleted_From_The_Standards_And_Sector_And_Route_Tables(
            DateTime importStartTime,
            [Frozen] Mock<IStandardRepository> repository,
            [Frozen] Mock<ISectorRepository> sectorRepository,
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            List<SFA.DAS.Courses.Domain.ImportTypes.Standard> standardsImport,
            StandardsImportService standardsImportService)
        {
            //Arrange
            service.Setup(x => x.GetStandards()).ReturnsAsync(standardsImport);

            //Act
            await standardsImportService.LoadDataFromStaging(importStartTime);

            //Assert
            repository.Verify(x => x.DeleteAll(), Times.Once);
            sectorRepository.Verify(x => x.DeleteAll(), Times.Once);
            routeRepository.Verify(x => x.DeleteAll(), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Data_Is_Loaded_From_The_Staging_Tables(
            DateTime importStartTime,
            List<StandardImport> standardImportsEntity,
            List<SectorImport> sectorImportsEntity,
            List<RouteImport> routeImportsEntity,
            [Frozen] Mock<IStandardRepository> repository,
            [Frozen] Mock<ISectorRepository> sectorRepository,
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<ISectorImportRepository> sectorImportRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            StandardsImportService standardsImportService)
        {
            //Arrange
            importRepository.Setup(x => x.GetAll()).ReturnsAsync(standardImportsEntity);
            sectorImportRepository.Setup(x => x.GetAll()).ReturnsAsync(sectorImportsEntity);
            routeImportRepository.Setup(x => x.GetAll()).ReturnsAsync(routeImportsEntity);

            //Act
            await standardsImportService.LoadDataFromStaging(importStartTime);

            //Assert
            repository.Verify(x => x.InsertMany(It.Is<List<Standard>>(c => c.Count.Equals(standardImportsEntity.Count))), Times.Once);
            sectorRepository.Verify(x => x.InsertMany(It.Is<List<Sector>>(c => c.Count.Equals(sectorImportsEntity.Count))), Times.Once);
            routeRepository.Verify(x => x.InsertMany(It.Is<List<Route>>(c => c.Count.Equals(routeImportsEntity.Count))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_There_Is_No_Data_In_The_Staging_Table_It_Is_Not_Loaded_Into_Standards_Table_And_Nothing_Is_Deleted(
            DateTime importStartTime,
            List<StandardImport> standardImportsEntity,
            [Frozen] Mock<IImportAuditRepository> auditRepository,
            [Frozen] Mock<IStandardRepository> repository,
            [Frozen] Mock<ISectorRepository> sectorRepository,
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IRouteRepository> routeRepository,
            StandardsImportService standardsImportService)
        {
            //Arrange
            importRepository.Setup(x => x.GetAll()).ReturnsAsync(new List<StandardImport>());

            //Act
            await standardsImportService.LoadDataFromStaging(importStartTime);

            //Assert
            repository.Verify(x => x.DeleteAll(), Times.Never);
            sectorRepository.Verify(x => x.DeleteAll(), Times.Never);
            repository.Verify(x => x.InsertMany(It.IsAny<List<Standard>>()), Times.Never);
            sectorRepository.Verify(x => x.InsertMany(It.IsAny<List<Sector>>()), Times.Never);
            routeRepository.Verify(x => x.InsertMany(It.IsAny<List<Route>>()), Times.Never);
            auditRepository.Verify(x =>
                x.Insert(It.Is<ImportAudit>(c => c.RowsImported.Equals(0))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_It_Is_Audited_After_The_Run(
            DateTime importStartTime,
            List<StandardImport> standardImportsEntity,
            [Frozen] Mock<IImportAuditRepository> auditRepository,
            [Frozen] Mock<IStandardImportRepository> importRepository,
            List<SFA.DAS.Courses.Domain.ImportTypes.Standard> apiImportStandards,
            StandardsImportService standardsImportService)
        {
            //Arrange
            importRepository.Setup(x => x.GetHashCode());
            importRepository.Setup(x => x.GetAll()).ReturnsAsync(standardImportsEntity);

            //Act
            await standardsImportService.LoadDataFromStaging(importStartTime);

            //Assert
            auditRepository.Verify(x =>
                x.Insert(It.Is<ImportAudit>(c => c.RowsImported.Equals(standardImportsEntity.Count))), Times.Once);
        }
    }
}
