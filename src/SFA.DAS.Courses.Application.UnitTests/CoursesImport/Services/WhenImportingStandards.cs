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

        //[Test, RecursiveMoqAutoData] TODO
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
            await standardsImportService.ImportDataIntoStaging();
            
            //Assert
            importRepository.Verify(x=>
                x.InsertMany(It.Is<List<StandardImport>>(c=>
                    c.Count.Equals(1))), Times.Once);
        }

        //[Test, RecursiveMoqAutoData] TODO
        public async Task Then_Only_ImportedStandards_With_A_LarsCode_Are_Imported(
            int wrongStatusLarsCode,
            Domain.ImportTypes.Standard apiStandard1,
            Domain.ImportTypes.Standard apiStandard2,
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
            await standardsImportService.ImportDataIntoStaging();
            
            //Assert
            importRepository.Verify(x=>
                x.InsertMany(It.Is<List<StandardImport>>(c=>
                    c.Count.Equals(apiImportStandards.Count-1))), Times.Once);
        }
        
        //[Test, RecursiveMoqAutoData] TODO
        public async Task Then_Only_The_Latest_Version_Of_ImportedStandards_With_A_LarsCode_Are_Imported(
            int wrongStatusLarsCode,
            Domain.ImportTypes.Standard apiStandard1,
            Domain.ImportTypes.Standard apiStandard2,
            Domain.ImportTypes.Standard apiStandard3,
            Domain.ImportTypes.Standard apiStandard4,
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            List<SFA.DAS.Courses.Domain.ImportTypes.Standard> apiImportStandards,
            StandardsImportService standardsImportService)
        {
            //Arrange
            apiImportStandards.ForEach(c=>
            {
                c.Status = "Approved for Delivery";
                c.Version = 1.1m;
            });
            apiStandard1.LarsCode = 0;
            apiStandard1.Status = "Approved for Delivery";
            apiImportStandards.Add(apiStandard1);
            apiStandard2.LarsCode = wrongStatusLarsCode;
            apiStandard2.Status = "Some Other Status";
            apiStandard2.Version = 1.1m;
            apiImportStandards.Add(apiStandard2);
            apiStandard3.LarsCode = apiStandard4.LarsCode;
            apiStandard3.Status = "Approved for Delivery";
            apiStandard4.Status = "Approved for Delivery";
            apiStandard4.Version = 1.1m;
            apiStandard3.Version = 1.0m;
            apiImportStandards.Add(apiStandard3);
            apiImportStandards.Add(apiStandard4);
            service.Setup(x => x.GetStandards()).ReturnsAsync(apiImportStandards);
            
            //Act
            await standardsImportService.ImportDataIntoStaging();
            
            //Assert
            importRepository.Verify(x=>
                x.InsertMany(It.Is<List<StandardImport>>(c=>
                    c.Count.Equals(apiImportStandards.Count-2) && c.TrueForAll(c=>c.Version.Equals(1.1m)))), Times.Once);
        }
    }
}
