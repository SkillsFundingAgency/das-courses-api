using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
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
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(larsCode))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetLatestActiveStandard(larsCode);

            standard.Should().BeEquivalentTo(standardFromRepo, StandardEquivalencyAssertionOptions.ExcludingFields);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_Standard_From_The_Repository_By_IFateReferenceNumber(
            string iFateReferenceNumber,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(iFateReferenceNumber))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetLatestActiveStandard(iFateReferenceNumber);

            standard.Should().BeEquivalentTo(standardFromRepo, StandardEquivalencyAssertionOptions.ExcludingFields);
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
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(iFateReferenceNumber))
                .ReturnsAsync(standardFromRepo);

            mockStandardsRepository
                .Setup(repository => repository.GetActiveStandardsByIfateReferenceNumber(standardFromRepo.RelatedOccupations))
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
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(iFateReferenceNumber))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetLatestActiveStandard(iFateReferenceNumber);

            standard.RelatedOccupations.Should().BeEmpty();
            mockStandardsRepository
                .Verify(repository => repository.GetActiveStandardsByIfateReferenceNumber(It.IsAny<List<string>>()), Times.Never);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_With_RelatedOccupations_From_The_Repository_By_LarsCode(
            int larsCode,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship.ToString();
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(larsCode))
                .ReturnsAsync(standardFromRepo);

            mockStandardsRepository
                .Setup(repository => repository.GetActiveStandardsByIfateReferenceNumber(standardFromRepo.RelatedOccupations))
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
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.Apprenticeship.ToString();
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(larsCode))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetLatestActiveStandard(larsCode);

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
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.Get(standardUId))
                .ReturnsAsync(standardFromRepo);

            mockStandardsRepository
                .Setup(repository => repository.GetActiveStandardsByIfateReferenceNumber(standardFromRepo.RelatedOccupations))
                .ReturnsAsync(relatedOccupations);

            var standard = await service.GetStandard(standardUId);

            standard.RelatedOccupations.Should().HaveCount(relatedOccupations.Count);
            standard.RelatedOccupations.Should().BeEquivalentTo(relatedOccupations.Select(ro => new Domain.Courses.RelatedOccupation(ro.Title, ro.Level)));
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
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.Get(standardUId))
                .ReturnsAsync(standardFromRepo);

            var standard = await service.GetStandard(standardUId);

            standard.RelatedOccupations.Should().BeEmpty();
            mockStandardsRepository
                .Verify(repository => repository.GetActiveStandardsByIfateReferenceNumber(It.IsAny<List<string>>()), Times.Never);
        }
    }
}
