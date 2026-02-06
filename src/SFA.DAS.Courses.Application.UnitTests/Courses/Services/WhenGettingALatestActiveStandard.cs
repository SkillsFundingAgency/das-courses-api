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
        public async Task Then_Gets_A_Standard_Filtered_From_The_Repository_By_LarsCode(
            string larsCode,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            // Arrange
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(larsCode, CourseType.Apprenticeship))
                .ReturnsAsync(standardFromRepo);

            // Act
            var standard = await service.GetLatestActiveStandard(larsCode, CourseType.Apprenticeship);

            // Assert
            standard.Should().BeEquivalentTo((Domain.Courses.Standard)standardFromRepo);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_Standard_Filterd_From_The_Repository_By_IFateReferenceNumber(
            string iFateReferenceNumber,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            // Arrange
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandardByIfateReferenceNumber(iFateReferenceNumber, CourseType.Apprenticeship))
                .ReturnsAsync(standardFromRepo);

            // Act
            var standard = await service.GetLatestActiveStandardByIfateReferenceNumber(iFateReferenceNumber, CourseType.Apprenticeship);

            // Assert
            standard.Should().BeEquivalentTo((Domain.Courses.Standard)standardFromRepo);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_Filtered_With_RelatedOccupations_From_The_Repository_By_IFateReferenceNumber(
            string iFateReferenceNumber,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            // Arrange
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship;
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandardByIfateReferenceNumber(iFateReferenceNumber, CourseType.Apprenticeship))
                .ReturnsAsync(standardFromRepo);

            mockStandardsRepository
                .Setup(repository => repository.GetActiveStandardsByIfateReferenceNumbers(standardFromRepo.RelatedOccupations, CourseType.Apprenticeship))
                .ReturnsAsync(relatedOccupations);

            // Act
            var standard = await service.GetLatestActiveStandardByIfateReferenceNumber(iFateReferenceNumber, CourseType.Apprenticeship);

            // Assert
            standard.RelatedOccupations.Should().HaveCount(relatedOccupations.Count);
            standard.RelatedOccupations.Should().BeEquivalentTo(relatedOccupations.Select(ro => new Domain.Courses.RelatedOccupation(ro.Title, ro.Level)));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_Filtered_Without_RelatedOccupations_From_The_Repository_By_IFateReferenceNumber(
            string iFateReferenceNumber,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            // Arrange
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.Apprenticeship;
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandardByIfateReferenceNumber(iFateReferenceNumber, CourseType.Apprenticeship))
                .ReturnsAsync(standardFromRepo);

            // Act
            var standard = await service.GetLatestActiveStandardByIfateReferenceNumber(iFateReferenceNumber, CourseType.Apprenticeship);

            // Assert
            standard.RelatedOccupations.Should().BeEmpty();
            mockStandardsRepository
                .Verify(repository => repository.GetActiveStandardsByIfateReferenceNumbers(It.IsAny<List<string>>(), CourseType.Apprenticeship), Times.Never);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_Filtered_With_RelatedOccupations_From_The_Repository_By_LarsCode(
            string larsCode,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            // Arrange
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship;
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(larsCode, CourseType.Apprenticeship))
                .ReturnsAsync(standardFromRepo);

            mockStandardsRepository
                .Setup(repository => repository.GetActiveStandardsByIfateReferenceNumbers(standardFromRepo.RelatedOccupations, CourseType.Apprenticeship))
                .ReturnsAsync(relatedOccupations);

            // Act
            var standard = await service.GetLatestActiveStandard(larsCode, CourseType.Apprenticeship);

            // Assert
            standard.RelatedOccupations.Should().HaveCount(relatedOccupations.Count);
            standard.RelatedOccupations.Should().BeEquivalentTo(relatedOccupations.Select(ro => new Domain.Courses.RelatedOccupation(ro.Title, ro.Level)));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_Filtered_Without_RelatedOccupations_From_The_Repository_By_LarsCode(
            string larsCode,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            // Arrange
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.Apprenticeship;
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.GetLatestActiveStandard(larsCode, CourseType.Apprenticeship))
                .ReturnsAsync(standardFromRepo);

            // Act
            var standard = await service.GetLatestActiveStandard(larsCode, CourseType.Apprenticeship);

            // Assert
            standard.RelatedOccupations.Should().BeEmpty();
            mockStandardsRepository
                .Verify(repository => repository.GetActiveStandardsByIfateReferenceNumbers(It.IsAny<List<string>>(), CourseType.Apprenticeship), Times.Never);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_Filtered_With_RelatedOccupations_From_The_Repository_By_StandardUId(
            string standardUId,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            // Arrange
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship;
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.Get(standardUId, CourseType.Apprenticeship))
                .ReturnsAsync(standardFromRepo);

            mockStandardsRepository
                .Setup(repository => repository.GetActiveStandardsByIfateReferenceNumbers(standardFromRepo.RelatedOccupations, CourseType.Apprenticeship))
                .ReturnsAsync(relatedOccupations);

            // Act
            var standard = await service.GetStandard(standardUId, CourseType.Apprenticeship);

            // Assert
            standard.RelatedOccupations.Should().HaveCount(relatedOccupations.Count);
            standard.RelatedOccupations.Should().BeEquivalentTo(relatedOccupations.Select(ro => new Domain.Courses.RelatedOccupation(ro.Title, ro.Level)));
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Standard_Filtered_Without_RelatedOccupations_From_The_Repository_By_StandardUId(
            string standardUId,
            Standard standardFromRepo,
            List<Standard> relatedOccupations,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            // Arrange
            standardFromRepo.ApprenticeshipType = ApprenticeshipType.Apprenticeship;
            standardFromRepo.RelatedOccupations = ["ST1001", "ST1002"];
            mockStandardsRepository
                .Setup(repository => repository.Get(standardUId, CourseType.Apprenticeship))
                .ReturnsAsync(standardFromRepo);

            // Act
            var standard = await service.GetStandard(standardUId, CourseType.Apprenticeship);


            // Assert
            standard.RelatedOccupations.Should().BeEmpty();
            mockStandardsRepository
                .Verify(repository => repository.GetActiveStandardsByIfateReferenceNumbers(It.IsAny<List<string>>(), CourseType.Apprenticeship), Times.Never);
        }
    }
}
