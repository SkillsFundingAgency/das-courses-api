using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenGettingALatestActiveStandard
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_Standard_From_The_Repository_By_LarsCode(
            int larsCode,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            InitializeStandardProperties(standardFromRepo);
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(larsCode))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetLatestActiveStandard(larsCode);

            standard.Should().BeEquivalentTo(standardFromRepo, StandardEquivalencyAssertionOptions.ExcludingFields);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Returns_Null_When_No_Standard_Found_In_Repository_By_LarsCode(
            int larsCode,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(larsCode))
                .ReturnsAsync((Standard)null);

            var standard = await service.GetLatestActiveStandard(larsCode);

            standard.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_With_RelatedOccupations_From_The_Repository_By_LarsCode(
            int larsCode,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            InitializeStandardProperties(standardFromRepo);
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship.ToString();
            var ifateReferenceNumbers = new List<string>{"ST1001", "ST1002"};
            standardFromRepo.RelatedOccupations = JsonConvert.SerializeObject(ifateReferenceNumbers);
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(larsCode))
                .ReturnsAsync(standardFromRepo);

            mockStandardsRepository
                .Setup(repository => repository.GetActiveStandardsByIfateReferenceNumber(ifateReferenceNumbers))
                .ReturnsAsync(relatedOccupations);

            var standard = await service.GetLatestActiveStandard(larsCode);

            standard.RelatedOccupations.Should().HaveCount(relatedOccupations.Count);
            standard.RelatedOccupations.Should().BeEquivalentTo(relatedOccupations.Select(ro => new Domain.Courses.RelatedOccupation(ro.Title, ro.Level)));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_Without_RelatedOccupations_From_The_Repository_By_LarsCode(
            int larsCode,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            var ifateReferenceNumbers = new List<string>{"ST1001", "ST1002"};
            InitializeStandardProperties(standardFromRepo);
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.Apprenticeship.ToString();
            standardFromRepo.RelatedOccupations = JsonConvert.SerializeObject(ifateReferenceNumbers);
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(larsCode))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetLatestActiveStandard(larsCode);

            standard.RelatedOccupations.Should().BeEmpty();
            mockStandardsRepository
                .Verify(repository => repository.GetActiveStandardsByIfateReferenceNumber(It.IsAny<List<string>>()), Times.Never);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_Standard_From_The_Repository_By_IFateReferenceNumber(
            string iFateReferenceNumber,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            InitializeStandardProperties(standardFromRepo);
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(iFateReferenceNumber))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetLatestActiveStandard(iFateReferenceNumber);

            standard.Should().BeEquivalentTo(standardFromRepo, StandardEquivalencyAssertionOptions.ExcludingFields);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Returns_Null_When_No_Standard_Found_In_Repository_By_IFateReferenceNumber(
            string iFateReferenceNumber,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(iFateReferenceNumber))
                .ReturnsAsync((Standard)null);

            var standard = await service.GetLatestActiveStandard(iFateReferenceNumber);

            standard.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_With_RelatedOccupations_From_The_Repository_By_IFateReferenceNumber(
            string iFateReferenceNumber,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship.ToString();
            InitializeStandardProperties(standardFromRepo);
            var ifateReferenceNumbers = new List<string>{"ST1001", "ST1002"};
            standardFromRepo.RelatedOccupations = JsonConvert.SerializeObject(ifateReferenceNumbers);
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(iFateReferenceNumber))
                .ReturnsAsync(standardFromRepo);

            mockStandardsRepository
                .Setup(repository => repository.GetActiveStandardsByIfateReferenceNumber(ifateReferenceNumbers))
                .ReturnsAsync(relatedOccupations);

            var standard = await service.GetLatestActiveStandard(iFateReferenceNumber);

            standard.RelatedOccupations.Should().HaveCount(relatedOccupations.Count);
            standard.RelatedOccupations.Should().BeEquivalentTo(relatedOccupations.Select(ro => new Domain.Courses.RelatedOccupation(ro.Title, ro.Level)));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_Without_RelatedOccupations_From_The_Repository_By_IFateReferenceNumber(
            string iFateReferenceNumber,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.Apprenticeship.ToString();
            var ifateReferenceNumbers = new List<string>{"ST1001", "ST1002"};
            InitializeStandardProperties(standardFromRepo);
            standardFromRepo.RelatedOccupations = JsonConvert.SerializeObject(ifateReferenceNumbers);
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(iFateReferenceNumber))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetLatestActiveStandard(iFateReferenceNumber);

            standard.RelatedOccupations.Should().BeEmpty();
            mockStandardsRepository
                .Verify(repository => repository.GetActiveStandardsByIfateReferenceNumber(It.IsAny<List<string>>()), Times.Never);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_With_RelatedOccupations_From_The_Repository_By_StandardUId(
            string standardUId,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship.ToString();
            var ifateReferenceNumbers = new List<string>{"ST1001", "ST1002"};
            InitializeStandardProperties(standardFromRepo);
            standardFromRepo.RelatedOccupations = JsonConvert.SerializeObject(ifateReferenceNumbers);
            mockStandardsRepository
                .Setup(repository => repository.Get(standardUId))
                .ReturnsAsync(standardFromRepo);

            mockStandardsRepository
                .Setup(repository => repository.GetActiveStandardsByIfateReferenceNumber(ifateReferenceNumbers))
                .ReturnsAsync(relatedOccupations);

            var standard = await service.GetStandard(standardUId);

            standard.RelatedOccupations.Should().HaveCount(relatedOccupations.Count);
            standard.RelatedOccupations.Should().BeEquivalentTo(relatedOccupations.Select(ro => new Domain.Courses.RelatedOccupation(ro.Title, ro.Level)));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_Standard_From_The_Repository_By_StandardUId(
            string standardUId,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            InitializeStandardProperties(standardFromRepo);
            mockStandardsRepository
                .Setup(repository => repository.Get(standardUId))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetStandard(standardUId);

            standard.Should().BeEquivalentTo(standardFromRepo, StandardEquivalencyAssertionOptions.ExcludingFields);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Returns_Null_When_No_Standard_Found_In_Repository_By_StandardUId(
            string standardUId,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.Get(standardUId))
                .ReturnsAsync((Standard)null);

            var standard = await service.GetStandard(standardUId);

            standard.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_Without_RelatedOccupations_From_The_Repository_By_StandardUId(
            string standardUId,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.Apprenticeship.ToString();
            InitializeStandardProperties(standardFromRepo);
            var ifateReferenceNumbers = new List<string>{"ST1001", "ST1002"};
            standardFromRepo.RelatedOccupations = JsonConvert.SerializeObject(ifateReferenceNumbers);
            mockStandardsRepository
                .Setup(repository => repository.Get(standardUId))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetStandard(standardUId);

            standard.RelatedOccupations.Should().BeEmpty();
            mockStandardsRepository
                .Verify(repository => repository.GetActiveStandardsByIfateReferenceNumber(It.IsAny<List<string>>()), Times.Never);
        }

        private static void InitializeStandardProperties(Standard standardFromRepo)
        {
            standardFromRepo.Options = "[]";
            standardFromRepo.CoreDuties = "[]";
            standardFromRepo.Duties = "[]";
            standardFromRepo.RelatedOccupations = "[]";
        }
    }
}
