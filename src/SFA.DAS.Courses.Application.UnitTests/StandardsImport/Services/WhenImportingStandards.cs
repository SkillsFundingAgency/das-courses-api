using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.StandardsImport.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.StandardsImport.Services
{
    public class WhenImportingStandards
    {
        [Test, MoqAutoData]
        public async Task Then_The_Standards_Are_Read_From_The_Api(
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            StandardsImportService standardsImportService)
        {
            //Act
            await standardsImportService.ImportStandards();
            
            //Assert
            service.Verify(x=>x.GetStandards(), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Staging_Table_Is_Emptied(
            [Frozen] Mock<IStandardImportRepository> importRepository,
            StandardsImportService standardsImportService)
        {
            //Act
            await standardsImportService.ImportStandards();
            
            //Assert
            importRepository.Verify(x=>x.DeleteAll(), Times.Once);
        }

        [Test, MoqAutoData]
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
                    c.Count.Equals(standardsImport.Count))), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Data_Is_Deleted_From_The_Standards_Table(
            [Frozen] Mock<IStandardRepository> repository,
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
        }

        [Test, MoqAutoData]
        public async Task Then_The_Data_Is_Loaded_From_The_Staging_Table_Into_The_Standards_Table(
            List<StandardImport> standardImportsEntity,
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
            repository.Verify(x=>x.InsertMany(standardImportsEntity), Times.Once);

        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Data_In_The_Staging_Table_It_Is_Not_Loaded_Into_Standards_Table_And_Nothing_Is_Deleted(
            List<StandardImport> standardImportsEntity,
            [Frozen] Mock<IStandardRepository> repository,
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
            repository.Verify(x=>x.InsertMany(It.IsAny<List<Standard>>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Only_ImportedStandards_With_A_LarsCode_And_ApprovedForDelivery_Status_Are_Imported(
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
                c.LarsCode = 10;
            });
            apiStandard1.LarsCode = 0;
            apiStandard1.Status = "Approved for Delivery";
            apiImportStandards.Add(apiStandard1);
            apiStandard2.LarsCode = 10;
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
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Data_Is_Not_Loaded_From_The_Staging_Table_Into_The_Standards_Table_An_Error_Is_Logged()
        {
            
        }
    }
}