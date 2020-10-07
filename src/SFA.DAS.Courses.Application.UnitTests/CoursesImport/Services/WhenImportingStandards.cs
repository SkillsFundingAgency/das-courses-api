using System;
using System.Collections.Generic;
using System.Linq;
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
    public class WhenImportingStandards
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Standards_Are_Read_From_The_Api(
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            StandardsImportService standardsImportService)
        {
            //Act
            await standardsImportService.ImportStandards();
            
            //Assert
            service.Verify(x=>x.GetStandards(), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Staging_Tables_Are_Emptied(
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<ISectorImportRepository> importSectorRepository,
            StandardsImportService standardsImportService)
        {
            //Act
            await standardsImportService.ImportStandards();
            
            //Assert
            importRepository.Verify(x=>x.DeleteAll(), Times.Once);
            importSectorRepository.Verify(x=>x.DeleteAll(), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Distinct_Sectors_Are_Loaded_Into_The_Import_Table(
            [Frozen] Mock<ISectorImportRepository> sectorImportRepository,
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            List<SFA.DAS.Courses.Domain.ImportTypes.Standard> standardsImport,
            StandardsImportService standardsImportService)
        {
            //Arrange
            standardsImport.ForEach(c=>
            {
                c.Status = "Approved for Delivery";
                c.LarsCode = 10;
            });
            var sectors = standardsImport.Select(s=>s.Route).Distinct().ToList();
            service.Setup(x => x.GetStandards()).ReturnsAsync(standardsImport);
            
            //Act
            await standardsImportService.ImportStandards();
            
            //Assert
            sectorImportRepository.Verify(x=>x
                .InsertMany(It.Is<List<SectorImport>>(
                    c=> c.Count.Equals(sectors.Count())
                    )), Times.Once);
            importRepository.Verify(x=>x.InsertMany(
                It.Is<List<StandardImport>>(std=>std.TrueForAll(c=>c.RouteId != Guid.Empty))));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Data_Is_Loaded_Into_The_Staging_Table_From_The_Api(
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            List<SFA.DAS.Courses.Domain.ImportTypes.Standard> standardsImport,
            StandardsImportService standardsImportService)
        {
            //Arrange
            standardsImport.ForEach(c=>
            {
                c.Status = "Approved for Delivery";
                c.LarsCode = 10;
            });
            service.Setup(x => x.GetStandards()).ReturnsAsync(standardsImport);
            
            //Act
            await standardsImportService.ImportStandards();
            
            //Assert
            importRepository.Verify(x=>
                x.InsertMany(It.Is<List<StandardImport>>(c=>
                    c.Count.Equals(1))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Data_Is_Deleted_From_The_Standards_And_Sector_Tables(
            [Frozen] Mock<IStandardRepository> repository,
            [Frozen] Mock<ISectorRepository> sectorRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            List<SFA.DAS.Courses.Domain.ImportTypes.Standard> standardsImport,
            StandardsImportService standardsImportService)
        {
            //Arrange
            service.Setup(x => x.GetStandards()).ReturnsAsync(standardsImport);
            
            //Act
            await standardsImportService.ImportStandards();
            
            //Assert
            repository.Verify(x=>x.DeleteAll(),Times.Once);
            sectorRepository.Verify(x=>x.DeleteAll(),Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Data_Is_Loaded_From_The_Staging_Tables(
            List<StandardImport> standardImportsEntity,
            List<SectorImport> sectorImportsEntity,
            [Frozen] Mock<IStandardRepository> repository,
            [Frozen] Mock<ISectorRepository> sectorRepository,
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<ISectorImportRepository> sectorImportRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            List<SFA.DAS.Courses.Domain.ImportTypes.Standard> apiImportStandards,
            StandardsImportService standardsImportService)
        {
            //Arrange
            service.Setup(x => x.GetStandards()).ReturnsAsync(apiImportStandards);
            importRepository.Setup(x => x.GetAll()).ReturnsAsync(standardImportsEntity);
            sectorImportRepository.Setup(x => x.GetAll()).ReturnsAsync(sectorImportsEntity);
            
            //Act
            await standardsImportService.ImportStandards();
            
            //Assert
            repository.Verify(x=>x.InsertMany(It.Is<List<Standard>>(c=>c.Count.Equals(standardImportsEntity.Count))), Times.Once);
            sectorRepository.Verify(x=>x.InsertMany(It.Is<List<Sector>>(c=>c.Count.Equals(sectorImportsEntity.Count))), Times.Once);

        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_There_Is_No_Data_In_The_Staging_Table_It_Is_Not_Loaded_Into_Standards_Table_And_Nothing_Is_Deleted(
            List<StandardImport> standardImportsEntity,
            [Frozen] Mock<IImportAuditRepository> auditRepository,
            [Frozen] Mock<IStandardRepository> repository,
            [Frozen] Mock<ISectorRepository> sectorRepository,
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            StandardsImportService standardsImportService)
        {
            //Arrange
            importRepository.Setup(x => x.GetAll()).ReturnsAsync(new List<StandardImport>());
            
            //Act
            await standardsImportService.ImportStandards();
            
            //Assert
            repository.Verify(x=>x.DeleteAll(), Times.Never);
            sectorRepository.Verify(x=>x.DeleteAll(), Times.Never);
            repository.Verify(x=>x.InsertMany(It.IsAny<List<Standard>>()), Times.Never);
            sectorRepository.Verify(x=>x.InsertMany(It.IsAny<List<Sector>>()), Times.Never);
            auditRepository.Verify(x=>
                x.Insert(It.Is<ImportAudit>(c=>c.RowsImported.Equals(0))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Only_ImportedStandards_With_A_LarsCode_And_ApprovedForDelivery_Status_Are_Imported(
            int wrongStatusLarsCode,
            List<StandardImport> standardImportsEntity,
            Domain.ImportTypes.Standard apiStandard1,
            Domain.ImportTypes.Standard apiStandard2,
            [Frozen] Mock<IStandardRepository> repository,
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            List<SFA.DAS.Courses.Domain.ImportTypes.Standard> apiImportStandards,
            StandardsImportService standardsImportService)
        {
            //Arrange
            apiImportStandards.ForEach(c=>
            {
                c.Status = "Approved for Delivery";
            });
            apiStandard1.LarsCode = 0;
            apiStandard1.Status = "Approved for Delivery";
            apiImportStandards.Add(apiStandard1);
            apiStandard2.LarsCode = wrongStatusLarsCode;
            apiStandard2.Status = "Some Other Status";
            apiImportStandards.Add(apiStandard2);
            service.Setup(x => x.GetStandards()).ReturnsAsync(apiImportStandards);
            
            //Act
            await standardsImportService.ImportStandards();
            
            //Assert
            importRepository.Verify(x=>
                x.InsertMany(It.Is<List<StandardImport>>(c=>
                    c.Count.Equals(apiImportStandards.Count-2))), Times.Once);

        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_It_Is_Audited_After_The_Run(
            List<StandardImport> standardImportsEntity,
            [Frozen] Mock<IImportAuditRepository> auditRepository,
            [Frozen] Mock<IStandardRepository> repository,
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            List<SFA.DAS.Courses.Domain.ImportTypes.Standard> apiImportStandards,
            StandardsImportService standardsImportService)
        {
            //Arrange
            service.Setup(x => x.GetStandards()).ReturnsAsync(apiImportStandards);
            importRepository.Setup(x => x.GetHashCode());
            importRepository.Setup(x => x.GetAll()).ReturnsAsync(standardImportsEntity);
            
            //Act
            await standardsImportService.ImportStandards();
            
            //Assert
            auditRepository.Verify(x=>
                x.Insert(It.Is<ImportAudit>(c=>c.RowsImported.Equals(standardImportsEntity.Count))), Times.Once);
        }
    }
}