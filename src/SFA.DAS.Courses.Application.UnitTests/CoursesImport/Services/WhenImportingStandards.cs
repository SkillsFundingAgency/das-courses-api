using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Services;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Extensions;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using Standard = SFA.DAS.Courses.Domain.ImportTypes.Standard;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Services
{
    public class WhenImportingStandards : StandardsImportServiceTestBase
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Distinct_Routes_Are_Loaded_Into_The_Route_Import_Table_And_Mapped_To_Standards(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                GetValidImportedStandard(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                GetValidImportedStandard(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                GetValidImportedStandard(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3"),
                GetValidImportedStandard(104, "ST0104", "0.1", "Title 4", Status.InDevelopment, "Route 3", "Option 4") // Route not loaded, standard loaded but route mapped to 0
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards())
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service.Setup(x => x.GetStandards()).ReturnsAsync(importedStandards);

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            routeImportRepository.Verify(x => x.InsertMany(It.Is<List<RouteImport>>(c => c.Count.Equals(2))), Times.Once);

            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0101" && p.RouteCode == 1))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0102" && p.RouteCode == 1))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0103" && p.RouteCode == 2))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0104" && p.RouteCode == 0))));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_In_Developement_Standards_From_1_0_Are_Not_loaded(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                GetValidImportedStandard(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                GetValidImportedStandard(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                GetValidImportedStandard(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3"),
                GetValidImportedStandard(104, "ST0104", "1.0", "Title 4", Status.InDevelopment, "Route 3", "Option 4") // Route and standard not loaded
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards())
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service.Setup(x => x.GetStandards()).ReturnsAsync(importedStandards);

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            routeImportRepository.Verify(x => x.InsertMany(It.Is<List<RouteImport>>(c => c.Count.Equals(2))), Times.Once);

            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0101" && p.RouteCode == 1))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0102" && p.RouteCode == 1))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0103" && p.RouteCode == 2))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => !s.Exists(p => p.IfateReferenceNumber == "ST0104"))));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Duplicate_Standards_Are_Removed_And_LatestStandard_Is_Kept(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                GetValidImportedStandard(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                GetValidImportedStandard(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                GetValidImportedStandard(103, "ST0103", "1.0", "Title 3", Status.Retired, "Route 2", "Option 3"),
                GetValidImportedStandard(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3")
            };

            importedStandards[2].CreatedDate = DateTime.Now; // this duplicate will be used as it was created last
            importedStandards[3].CreatedDate = DateTime.Now.AddDays(-1); 

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards())
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service.Setup(x => x.GetStandards()).ReturnsAsync(importedStandards);

            //Act
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0101" && p.Status == Status.ApprovedForDelivery))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0102" && p.Status == Status.ApprovedForDelivery))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0103" && p.Status == Status.Retired))));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_New_Routes_Are_Loaded_And_Unused_Existing_Routes_Are_Retained_And_Become_Inactive(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                GetValidImportedStandard(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                GetValidImportedStandard(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                GetValidImportedStandard(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 3", "Option 3")
            };

            var currentRoutes = new List<Domain.Entities.Route>
            {
                new Domain.Entities.Route { Id = 1, Name = "Route 1", Active = true},
                new Domain.Entities.Route { Id = 2, Name = "Route 2", Active = true}
            };

            var currentStandards = new List<Domain.Entities.Standard>
            {
                GetValidStandard(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
                GetValidStandard(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, 1, "Option 2"),
                GetValidStandard(101, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, 2, "Option 1")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(currentRoutes);

            standardRepository
                .Setup(s => s.GetStandards())
                .ReturnsAsync(currentStandards);

            service.Setup(x => x.GetStandards()).ReturnsAsync(importedStandards);

            //Act
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            routeImportRepository.Verify(x => x.InsertMany(It.Is<List<RouteImport>>(s => s.Exists(p => p.Id == 1 && p.Active))));
            routeImportRepository.Verify(x => x.InsertMany(It.Is<List<RouteImport>>(s => s.Exists(p => p.Id == 2 && !p.Active))));
            routeImportRepository.Verify(x => x.InsertMany(It.Is<List<RouteImport>>(s => s.Exists(p => p.Id == 3 && p.Active))));

            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0101" && p.RouteCode == 1))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0102" && p.RouteCode == 1))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0103" && p.RouteCode == 3))));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_No_Standards_Are_Imported_Current_Standards_Are_Retained(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var importedStandards = new List<Standard>();

            var currentRoutes = new List<Domain.Entities.Route>
            {
                new Domain.Entities.Route { Id = 1, Name = "Route 1", Active = true},
                new Domain.Entities.Route { Id = 2, Name = "Route 2", Active = true}
            };

            var currentStandards = new List<Domain.Entities.Standard>
            {
                GetValidStandard(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
                GetValidStandard(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, 1, "Option 2"),
                GetValidStandard(101, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, 2, "Option 1")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(currentRoutes);

            standardRepository
                .Setup(s => s.GetStandards())
                .ReturnsAsync(currentStandards);

            service.Setup(x => x.GetStandards()).ReturnsAsync(importedStandards);

            //Act
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            routeImportRepository.Verify(x => x.InsertMany(It.Is<List<RouteImport>>(s => s.Exists(p => p.Id == 1 && p.Active))));
            routeImportRepository.Verify(x => x.InsertMany(It.Is<List<RouteImport>>(s => s.Exists(p => p.Id == 2 && p.Active))));

            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0101" && p.RouteCode == 1))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0102" && p.RouteCode == 1))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0103" && p.RouteCode == 2))));
        }

        [Test]
        [MoqInlineAutoData("Ofqual is the intended EQA provider")]
        [MoqInlineAutoData("ofqual is the intended eqa provider")]
        [MoqInlineAutoData("OFQUAL IS THE INTENDED EQA PROVIDER")]
        public async Task Then_EqaProviderName_Is_Updated_If_Ofqual_Is_Intended(
            string eqaProviderName,
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                GetValidImportedStandard(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
            };

            importedStandards[0].EqaProvider = new Domain.ImportTypes.EqaProvider 
            { 
                ProviderName = eqaProviderName, 
                ContactName = new Settable<string>(null), 
                ContactAddress = new Settable<string>(null), 
                ContactEmail = new Settable<string>(null), 
                WebLink = new Settable<string>(null)
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards())
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service.Setup(x => x.GetStandards()).ReturnsAsync(importedStandards);

            //Act
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0101" && p.EqaProviderName == "Ofqual"))));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Standards_Are_Assigned_StandardUId(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var importedStandards = new List<Standard>
            {
                GetValidImportedStandard(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                GetValidImportedStandard(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                GetValidImportedStandard(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards())
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service.Setup(x => x.GetStandards()).ReturnsAsync(importedStandards);

            //Act
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            standardImportRepository.Verify(x =>
                x.InsertMany(It.Is<List<StandardImport>>(c =>
                    c.TrueForAll(s => s.StandardUId.Equals(s.IfateReferenceNumber.ToStandardUId(s.Version))))), Times.Once);
        }
    }
}
