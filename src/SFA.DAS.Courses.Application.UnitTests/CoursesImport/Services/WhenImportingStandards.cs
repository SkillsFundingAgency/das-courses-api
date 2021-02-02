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
            await standardsImportService.ImportDataIntoStaging();
            
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
            await standardsImportService.ImportDataIntoStaging();
            
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
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            sectorImportRepository.Verify(x => x.InsertMany(It.Is<List<SectorImport>>(c => c.Count.Equals(sectors.Count()))), Times.Once);
            importRepository.Verify(x=>x.InsertMany(It.Is<List<StandardImport>>(std=>std.TrueForAll(c=>c.RouteId != Guid.Empty))));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_All_The_Standards_Are_Loaded_Into_The_Staging_Table_From_The_Api(
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> ifateService,
            List<SFA.DAS.Courses.Domain.ImportTypes.Standard> standardsImport,
            StandardsImportService standardsImportService)
        {
            //Arrange
            ifateService.Setup(x => x.GetStandards()).ReturnsAsync(standardsImport);
            
            //Act
            await standardsImportService.ImportDataIntoStaging();
            
            //Assert
            importRepository.Verify(x=>
                x.InsertMany(It.Is<List<StandardImport>>(c=>
                    c.Count.Equals(standardsImport.Count()))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_All_The_Standards_Are_Assigned_StandardUId_And_Loaded_Into_The_Staging_Table(
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> ifateService,
            List<SFA.DAS.Courses.Domain.ImportTypes.Standard> standardsImport,
            StandardsImportService standardsImportService)
        {
            //Arrange
            ifateService.Setup(x => x.GetStandards()).ReturnsAsync(standardsImport);

            //Act
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            importRepository.Verify(x =>
                x.InsertMany(It.Is<List<StandardImport>>(c =>
                    c.TrueForAll(s => s.StandardUId.Equals(GetStandardUId(s.IfateReferenceNumber, s.Version))))), Times.Once);
        }

        private static string GetStandardUId(string ifateReferenceNumber, decimal? version)
        {
            var derivedVersion = version.HasValue && version != 0 ? version.Value : 1;
            return $"{ifateReferenceNumber}_{derivedVersion.ToString("0.0")}";
        }
    }
}
