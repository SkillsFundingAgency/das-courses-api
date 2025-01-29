using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.CoursesImport.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Extensions;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using Standard = SFA.DAS.Courses.Domain.ImportTypes.Standard;

namespace SFA.DAS.Courses.Application.UnitTests.CoursesImport.Services
{
    public class WhenImportingStandards
    {
        private const string ProviderName = "Training Provider";
        private const string EPAOName = "EPAO";

        [Test, RecursiveMoqAutoData]
        public async Task Then_The_Distinct_Routes_Are_Loaded_Into_The_Import_Table_That_Are_Approved_For_Delivery(
            Standard notImportedStandard,
            [Frozen] Mock<IRouteImportRepository> routeImportRepository,
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> service,
            List<Standard> standardsImport,
            StandardsImportService standardsImportService)
        {
            //Arrange
            SetStandardsToStatus(standardsImport);
            notImportedStandard.Status = "In development";
            standardsImport.Add(notImportedStandard);
            var routes = standardsImport.Select(s => s.Route).Distinct().ToList();
            UpdateImportWithRegulationDetail(standardsImport);
            service.Setup(x => x.GetStandards()).ReturnsAsync(standardsImport.Where(c => c.Status == "Approved for Delivery"));

            //Act
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            routeImportRepository.Verify(x => x.InsertMany(It.Is<List<RouteImport>>(c => c.Count.Equals(routes.Count - 1))), Times.Once);
            importRepository.Verify(x => x.InsertMany(It.Is<List<StandardImport>>(std => std.TrueForAll(c => c.RouteCode != 0))));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_All_The_Standards_Are_Loaded_Into_The_Staging_Table_From_The_Api_And_Import_Data_Deleted_On_Load_And_Only_Routes_With_Approved_For_Delivery_Loaded(
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> ifateService,
            [Frozen] Mock<IRouteImportRepository> importRouteRepository,
            List<Standard> standardsImport,
            StandardsImportService standardsImportService)
        {
            //Arrange
            SetStandardsToStatus(standardsImport, "In Development");
            UpdateImportWithRegulationDetail(standardsImport);

            ifateService.Setup(x => x.GetStandards()).ReturnsAsync(standardsImport);

            //Act
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            ifateService.Verify(x => x.GetStandards(), Times.Once);
            importRepository.Verify(x =>
                x.InsertMany(It.Is<List<StandardImport>>(c =>
                    c.Count.Equals(standardsImport.Count()))), Times.Once);
            importRouteRepository.Verify(x => x.InsertMany(It.Is<List<RouteImport>>(c => c.Count == 0)));
            importRepository.Verify(x => x.DeleteAll(), Times.Once);
            importRouteRepository.Verify(x => x.DeleteAll(), Times.Once);
        }

        [Test]
        [MoqInlineAutoData("Ofqual is the intended EQA provider")]
        [MoqInlineAutoData("ofqual is the intended eqa provider")]
        [MoqInlineAutoData("OFQUAL IS THE INTENDED EQA PROVIDER")]
        public async Task Then_EqaProviderName_Is_Updated_If_Ofqual_Is_Intended(
            string eqaProviderName,
            [Frozen] Mock<IStandardImportRepository> standardImportRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> ifateService,
            List<Standard> standardsImport,
            Standard standard,
            StandardsImportService standardsImportService)
        {
            //Arrange
            standard.EqaProvider.ProviderName = eqaProviderName;
            standardsImport.Add(standard);
            SetStandardsToStatus(standardsImport);
            UpdateImportWithRegulationDetail(standardsImport);
            ifateService.Setup(x => x.GetStandards()).ReturnsAsync(standardsImport);

            //Act
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            standardImportRepository.Verify(x =>
                x.InsertMany(It.Is<List<StandardImport>>(c => c.Count(s => s.EqaProviderName == "Ofqual") == 1)), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_All_The_Standards_Are_Assigned_StandardUId_And_Loaded_Into_The_Staging_Table(
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> ifateService,
            List<Standard> standardsImport,
            StandardsImportService standardsImportService)
        {
            //Arrange
            SetStandardsToStatus(standardsImport);
            UpdateImportWithRegulationDetail(standardsImport);
            ifateService.Setup(x => x.GetStandards()).ReturnsAsync(standardsImport);

            //Act
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            importRepository.Verify(x =>
                x.InsertMany(It.Is<List<StandardImport>>(c =>
                    c.TrueForAll(s => s.StandardUId.Equals(s.IfateReferenceNumber.ToStandardUId(s.Version))))), Times.Once);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Duplicate_Standards_Are_Removed_And_LatestStandard_Is_Kept(
            [Frozen] Mock<IStandardImportRepository> importRepository,
            [Frozen] Mock<IInstituteOfApprenticeshipService> ifateService,
            List<Standard> standardsImport,
            Standard standard,
            Standard standardDuplicate,
            StandardsImportService standardsImportService)
        {
            //Arrange
            standardDuplicate.ReferenceNumber = standard.ReferenceNumber;
            standardDuplicate.Version = standard.Version;
            standard.CreatedDate = DateTime.Now;
            standardDuplicate.CreatedDate = DateTime.Now.AddHours(-1);
            standardsImport.Add(standard);
            standardsImport.Add(standardDuplicate);
            SetStandardsToStatus(standardsImport);
            UpdateImportWithRegulationDetail(standardsImport);
            ifateService.Setup(x => x.GetStandards()).ReturnsAsync(standardsImport);

            //Act
            await standardsImportService.ImportDataIntoStaging();

            //Assert
            importRepository.Verify(x =>
                x.InsertMany(It.Is<List<StandardImport>>(c =>
                    c.TrueForAll(s => s.StandardUId.Equals(s.IfateReferenceNumber.ToStandardUId(s.Version))))), Times.Once);
        }

        private static void SetStandardsToStatus(List<Standard> standardsImport, string status = "Approved for Delivery")
        {
            standardsImport.ForEach(c =>
            {
                c.Status = status;
            });
        }

        private static void UpdateImportWithRegulationDetail(List<Standard> standardImport)
        {
            foreach (var standard in standardImport)
            {
                standard.RegulationDetail[0].Name = ProviderName;
                standard.RegulationDetail[1].Name = EPAOName;
            }
        }
    }
}
