using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Services;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Extensions;
using SFA.DAS.Courses.Domain.ImportTypes.Settable;
using SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using Standard = SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland.Standard;

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
            [Frozen] Mock<ISkillsEnglandService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3"),
                CreateValidImportedApprenticeship(104, "ST0104", "0.1", "Title 4", Status.InDevelopment, "Route 3", "Option 4") // Route not loaded, standard loaded but route mapped to 0
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

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
        public async Task Then_In_Development_Standards_Above_2_0_Are_Not_Loaded(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            StandardsImportService _sut)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3"),
                CreateValidImportedApprenticeship(104, "ST0104", "2.0", "Title 4", Status.InDevelopment, "Route 3", "Option 4"),  // route not imported for in-development standard
                CreateValidImportedApprenticeship(105, "ST0105", "2.1", "Title 5", Status.InDevelopment, "Route 4", "Option 5")   // standard not imported for in-development above 2.0
            };

            IEnumerable<RouteImport> actualRouteImports = null;
            IEnumerable<StandardImport> actualStandardImports = null;

            routeRepository
                .Setup(x => x.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(x => x.GetStandards(null))
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            routeImportRepository
                .Setup(x => x.InsertMany(It.IsAny<IEnumerable<RouteImport>>()))
                .Callback<IEnumerable<RouteImport>>(x => actualRouteImports = x.ToList())
                .ReturnsAsync((IEnumerable<RouteImport> x) => x.Count());

            standardImportRepository
                .Setup(x => x.InsertMany(It.IsAny<IEnumerable<StandardImport>>()))
                .Callback<IEnumerable<StandardImport>>(x => actualStandardImports = x.ToList())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.ImportDataIntoStaging();

            // Assert
            actualRouteImports.Should().NotBeNull();
            actualRouteImports.Should().HaveCount(2);
            actualRouteImports.Select(x => x.Name).Should().BeEquivalentTo("Route 1", "Route 2");

            actualStandardImports.Should().NotBeNull();
            actualStandardImports.Select(x => x.IfateReferenceNumber).Should().BeEquivalentTo(
                "ST0101",
                "ST0102",
                "ST0103",
                "ST0104");

            actualStandardImports.Should().Contain(x =>
                x.IfateReferenceNumber == "ST0104" &&
                x.Version == "2.0");

            actualStandardImports.Should().NotContain(x =>
                x.IfateReferenceNumber == "ST0105");

            routeImportRepository.Verify(x => x.InsertMany(It.IsAny<List<RouteImport>>()), Times.Once);
            standardImportRepository.Verify(x => x.InsertMany(It.IsAny<List<StandardImport>>()), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Duplicate_Standards_Are_Removed_And_LatestStandard_Is_Kept(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(103, "ST0103", "1.0", "Title 3", Status.Retired, "Route 2", "Option 3"),
                CreateValidImportedApprenticeship(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3")
            };

            apprenticeships[2].CreatedDate = DateTime.Now; // this duplicate will be used as it was created last
            apprenticeships[3].CreatedDate = DateTime.Now.AddDays(-1); 

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
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
            [Frozen] Mock<ISkillsEnglandService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 3", "Option 3")
            };

            var currentRoutes = new List<Domain.Entities.Route>
            {
                new Domain.Entities.Route { Id = 1, Name = "Route 1", Active = true},
                new Domain.Entities.Route { Id = 2, Name = "Route 2", Active = true}
            };

            var currentStandards = new List<Domain.Entities.Standard>
            {
                GetValidStandard("101", "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
                GetValidStandard("102", "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, 1, "Option 2"),
                GetValidStandard("101", "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, 2, "Option 1")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(currentRoutes);

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(currentStandards);

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
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
            [Frozen] Mock<ISkillsEnglandService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>();

            var currentRoutes = new List<Domain.Entities.Route>
            {
                new Domain.Entities.Route { Id = 1, Name = "Route 1", Active = true},
                new Domain.Entities.Route { Id = 2, Name = "Route 2", Active = true}
            };

            var currentStandards = new List<Domain.Entities.Standard>
            {
                GetValidStandard("101", "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
                GetValidStandard("102", "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, 1, "Option 2"),
                GetValidStandard("103", "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, 2, "Option 1")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(currentRoutes);

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(currentStandards);

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
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
            [Frozen] Mock<ISkillsEnglandService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
            };

            apprenticeships[0].EqaProvider = new Apprenticeship.ApprenticeshipEqaProvider
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
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0101" && p.EqaProviderName == "Ofqual"))));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Standards_Are_Assigned_StandardUId(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            standardImportRepository.Verify(x =>
                x.InsertMany(It.Is<List<StandardImport>>(c =>
                    c.TrueForAll(s => s.StandardUId.Equals(s.IfateReferenceNumber.ToStandardUId(s.Version))))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Missing_Fields_Prevent_Standard_Import(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3")
            };

            apprenticeships[0].LarsCode = new Settable<int>(); // the lars code is missing from the import

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            routeImportRepository.Verify(x => x.InsertMany(It.IsAny<List<RouteImport>>()), Times.Never);
            standardImportRepository.Verify(x => x.InsertMany(It.IsAny<List<StandardImport>>()), Times.Never);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_BadFormat_For_ReferenceNumber_Prevent_Standard_Import(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"), // bad format for standard reference
                CreateValidImportedApprenticeship(102, "102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(103, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            routeImportRepository.Verify(x => x.InsertMany(It.IsAny<List<RouteImport>>()), Times.Never);
            standardImportRepository.Verify(x => x.InsertMany(It.IsAny<List<StandardImport>>()), Times.Never);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_BadFormat_For_Version_Prevents_Standard_Import(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"), // bad format for vesion
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(103, "ST0103", "1+0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            routeImportRepository.Verify(x => x.InsertMany(It.IsAny<List<RouteImport>>()), Times.Never);
            standardImportRepository.Verify(x => x.InsertMany(It.IsAny<List<StandardImport>>()), Times.Never);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Slack_Notification_Sent_With_All_Errors_When_Standard_Import_Prevented_Due_To_Errors(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            [Frozen] Mock<ISlackNotificationService> slackNotificationService,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                CreateValidImportedApprenticeship(102, "102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"), // bad referenceNumber format
                CreateValidImportedApprenticeship(103, "ST0103", "1+0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3") // bad version format
            };

            apprenticeships[0].LarsCode = new Settable<int>(); // the lars code is missing from the import

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(new List<Domain.Entities.Route>());

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(new List<Domain.Entities.Standard>());

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            slackNotificationService
                .Verify(s => s.UploadFile(
                    It.Is<List<string>>(p => p.Exists(s => s.Contains("E1001")) && p.Exists(s => s.Contains("E1002")) && p.Exists(s => s.Contains("E1003"))),
                    It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_Standard_Has_Standard_Error_Current_Standard_Is_Retained_And_Slack_Notification_Is_Sent(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            [Frozen] Mock<ISlackNotificationService> slackNotificationService,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(103, "ST0103", "1.0", "", Status.ApprovedForDelivery, "Route 2", "Option 3") // blank title is invalid
            };

            var currentRoutes = new List<Domain.Entities.Route>
            {
                new Domain.Entities.Route { Id = 1, Name = "Route 1", Active = true},
                new Domain.Entities.Route { Id = 2, Name = "Route 2", Active = true}
            };

            var currentStandards = new List<Domain.Entities.Standard>
            {
                GetValidStandard("101", "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
                GetValidStandard("102", "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, 1, "Option 2"),
                GetValidStandard("103", "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, 2, "Option 1")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(currentRoutes);

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(currentStandards);

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0103" && p.Title == "Title 3")))); // previous title retained

            slackNotificationService
                .Verify(s => s.UploadFile(
                    It.Is<List<string>>(p => p.Exists(s => s.Contains("S1008"))),
                    It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_Standard_Has_Warning_Then_Standard_Is_Imported_And_Slack_Notification_Is_Sent(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            [Frozen] Mock<ISlackNotificationService> slackNotificationService,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(0, "ST0103", "1.0", "Title New 3", Status.ApprovedForDelivery, "Route 2", "Option 3") // blank title is invalid
            };

            apprenticeships[2].PublishDate = DateTime.Now.AddDays(-14);

            var currentRoutes = new List<Domain.Entities.Route>
            {
                new Domain.Entities.Route { Id = 1, Name = "Route 1", Active = true},
                new Domain.Entities.Route { Id = 2, Name = "Route 2", Active = true}
            };

            var currentStandards = new List<Domain.Entities.Standard>
            {
                GetValidStandard("101", "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
                GetValidStandard("102", "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, 1, "Option 2"),
                GetValidStandard("0", "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, 2, "Option 1")
            };

            currentStandards[2].PublishDate = DateTime.Now.AddDays(-14);

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(currentRoutes);

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(currentStandards);

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0103" && p.Title == "Title New 3")))); // new title imported

            slackNotificationService
                .Verify(s => s.UploadFile(
                    It.Is<List<string>>(p => p.Exists(s => s.Contains("W1001"))),
                    It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_Standard_Has_Duplicated_LarsCode_Then_No_Standards_Imported_And_Slack_Notification_Is_Sent(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            [Frozen] Mock<ISlackNotificationService> slackNotificationService,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"),
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(101, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3") // duplicated lars code
            };

            var currentRoutes = new List<Domain.Entities.Route>
            {
                new Domain.Entities.Route { Id = 1, Name = "Route 1", Active = true},
                new Domain.Entities.Route { Id = 2, Name = "Route 2", Active = true}
            };

            var currentStandards = new List<Domain.Entities.Standard>
            {
                GetValidStandard("101", "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
                GetValidStandard("102", "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, 1, "Option 2"),
                GetValidStandard("103", "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, 2, "Option 1")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(currentRoutes);

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(currentStandards);

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            routeImportRepository.Verify(x => x.InsertMany(It.IsAny<List<RouteImport>>()), Times.Never);
            standardImportRepository.Verify(x => x.InsertMany(It.IsAny<List<StandardImport>>()), Times.Never);

            slackNotificationService
                .Verify(s => s.UploadFile(
                    It.Is<List<string>>(p => p.Exists(s => s.Contains("E1004"))),
                    It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_Standard_Has_Duplicated_LarsCode_Due_To_Other_Failure_Then_No_Standards_Imported_And_Slack_Notification_Is_Sent(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            [Frozen] Mock<ISlackNotificationService> slackNotificationService,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(103, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"), // lars code swapped with ST0103
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(101, "ST0103", "1.0", "", Status.ApprovedForDelivery, "Route 2", "Option 3") // but swap rejected because title is invalid
            };

            var currentRoutes = new List<Domain.Entities.Route>
            {
                new Domain.Entities.Route { Id = 1, Name = "Route 1", Active = true},
                new Domain.Entities.Route { Id = 2, Name = "Route 2", Active = true}
            };

            var currentStandards = new List<Domain.Entities.Standard>
            {
                GetValidStandard("101", "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
                GetValidStandard("102", "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, 1, "Option 2"),
                GetValidStandard("103", "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, 2, "Option 1")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(currentRoutes);

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(currentStandards);

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            routeImportRepository.Verify(x => x.InsertMany(It.IsAny<List<RouteImport>>()), Times.Never);
            standardImportRepository.Verify(x => x.InsertMany(It.IsAny<List<StandardImport>>()), Times.Never);

            slackNotificationService
                .Verify(s => s.UploadFile(
                    It.Is<List<string>>(p => p.Exists(s => s.Contains("E1004") && p.Exists(s => s.Contains("S1008")))),
                    It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_If_Standard_Has_Swapped_LarsCode_Then_Standards_Imported_And_No_Slack_Notification_Is_Sent(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            [Frozen] Mock<ISlackNotificationService> slackNotificationService,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(103, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1"), // lars code swapped with ST0103
                CreateValidImportedApprenticeship(102, "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, "Route 1", "Option 2"),
                CreateValidImportedApprenticeship(101, "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, "Route 2", "Option 3") // lars code swapped with ST0101
            };

            var currentRoutes = new List<Domain.Entities.Route>
            {
                new Domain.Entities.Route { Id = 1, Name = "Route 1", Active = true},
                new Domain.Entities.Route { Id = 2, Name = "Route 2", Active = true}
            };

            var currentStandards = new List<Domain.Entities.Standard>
            {
                GetValidStandard("101", "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, 1, "Option 1"),
                GetValidStandard("102", "ST0102", "1.0", "Title 2", Status.ApprovedForDelivery, 1, "Option 2"),
                GetValidStandard("103", "ST0103", "1.0", "Title 3", Status.ApprovedForDelivery, 2, "Option 1")
            };

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(currentRoutes);

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(currentStandards);

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = new List<FoundationApprenticeship>(),
                    ApprenticeshipUnits = new List<ApprenticeshipUnit>()
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0101" && p.LarsCode == "103"))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0102" && p.LarsCode == "102"))));
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(s => s.Exists(p => p.IfateReferenceNumber == "ST0103" && p.LarsCode == "101"))));

            slackNotificationService
                .Verify(s => s.UploadFile(
                    It.IsAny<List<string>>(),
                    It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Standards_Have_Correct_ApprenticeshipType_And_CourseType_And_DurationUnits(
            [Frozen] Mock<IRouteRepository> routeRepository,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardRepository> standardRepository,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<ISkillsEnglandService> service,
            [Frozen] Mock<ISlackNotificationService> slackNotificationService,
            StandardsImportService standardsImportService)
        {
            // Arrange
            var apprenticeships = new List<Apprenticeship>
            {
                CreateValidImportedApprenticeship(101, "ST0101", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1", "Option 1")
            };

            var foundationApprenticeships = new List<FoundationApprenticeship>
            {
                CreateValidImportedFoundationApprenticeship(102, "FA0102", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1")
            };

            var apprenticeshipUnits = new List<ApprenticeshipUnit>
            {
                CreateValidImportedApprenticeshipUnit("ZSC00103", "AU0103", "1.0", "Title 1", Status.ApprovedForDelivery, "Route 1")
            };

            var currentRoutes = new List<Domain.Entities.Route>
            {
                new Domain.Entities.Route { Id = 1, Name = "Route 1", Active = true}
            };

            var currentStandards = new List<Domain.Entities.Standard>();

            routeRepository
                .Setup(s => s.GetAll())
                .ReturnsAsync(currentRoutes);

            standardRepository
                .Setup(s => s.GetStandards(null))
                .ReturnsAsync(currentStandards);

            service
                .Setup(x => x.GetCourseImports())
                .ReturnsAsync(new SkillsEnglandStandardsResult
                {
                    Apprenticeships = apprenticeships,
                    FoundationApprenticeships = foundationApprenticeships,
                    ApprenticeshipUnits = apprenticeshipUnits
                });

            // Act
            await standardsImportService.ImportDataIntoStaging();

            // Assert
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(
                s => s.Exists(p => p.IfateReferenceNumber == "ST0101" 
                    && p.LarsCode == "101" 
                    && p.ApprenticeshipType == ApprenticeshipType.Apprenticeship
                    && p.CourseType == CourseType.Apprenticeship
                    && p.DurationUnits == DurationUnits.Months))));
            
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(
                s => s.Exists(p => p.IfateReferenceNumber == "FA0102" 
                    && p.LarsCode == "102" 
                    && p.ApprenticeshipType == ApprenticeshipType.FoundationApprenticeship
                    && p.CourseType == CourseType.Apprenticeship
                    && p.DurationUnits == DurationUnits.Months))));
            
            standardImportRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(
                s => s.Exists(p => p.IfateReferenceNumber == "AU0103" 
                    && p.LarsCode == "ZSC00103" 
                    && p.ApprenticeshipType == ApprenticeshipType.ApprenticeshipUnit
                    && p.CourseType == CourseType.ShortCourse
                    && p.DurationUnits == DurationUnits.Hours))));
        }
    }
}
