using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using ApprenticeshipType = SFA.DAS.Courses.Domain.Entities.ApprenticeshipType;
using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenGettingStandardByAnyId
    {
        [Test]
        [MoqInlineAutoData("ST0001_1.0", "123")]
        [MoqInlineAutoData("FA0001_1.0", "456")]
        [MoqInlineAutoData("AU0001_1.0", "ZSC00123")]
        public async Task When_Id_Is_LarsCode_Then_Uses_GetLatestActiveStandard(
            string standardUid,
            string id,
            [Frozen] Mock<IStandardRepository> standardsRepository,
            StandardsService _sut)
        {
            // Arrange
            standardsRepository
                .Setup(r => r.GetLatestActiveStandard(id, CourseType.Apprenticeship))
                .ReturnsAsync((Standard)null);

            // Act
            await _sut.GetStandardByAnyId(id);

            // Assert
            standardsRepository.Verify(r => r.GetLatestActiveStandard(id, CourseType.Apprenticeship), Times.Once);
            standardsRepository.Verify(r => r.GetLatestActiveStandardByIfateReferenceNumber(It.IsAny<string>(), It.IsAny<CourseType?>()), Times.Never);
            standardsRepository.Verify(r => r.Get(It.IsAny<string>(), It.IsAny<CourseType?>()), Times.Never);
        }

        [Test]
        [MoqInlineAutoData("ST0001_1.0", "ST0001")]
        [MoqInlineAutoData("FA0001_1.0", "FA0001")]
        [MoqInlineAutoData("AU0001_1.0", "AU0001")]
        public async Task When_Id_Is_ReferenceNumber_Then_Uses_GetLatestActiveStandardByIfateReferenceNumber(
            string standardUid,
            string id,
            [Frozen] Mock<IStandardRepository> standardsRepository,
            StandardsService _sut)
        {
            // Arrange
            standardsRepository
                .Setup(r => r.GetLatestActiveStandardByIfateReferenceNumber(id, CourseType.Apprenticeship))
                .ReturnsAsync((Standard)null);

            // Act
            await _sut.GetStandardByAnyId(id);

            // Assert
            standardsRepository.Verify(r => r.GetLatestActiveStandardByIfateReferenceNumber(id, CourseType.Apprenticeship), Times.Once);
            standardsRepository.Verify(r => r.GetLatestActiveStandard(It.IsAny<string>(), It.IsAny<CourseType?>()), Times.Never);
            standardsRepository.Verify(r => r.Get(It.IsAny<string>(), It.IsAny<CourseType?>()), Times.Never);
        }

        [Test]
        [MoqInlineAutoData("ST0001_1.0")]
        [MoqInlineAutoData("FA0001_1.0")]
        [MoqInlineAutoData("AU0001_1.0")]
        public async Task When_Id_Is_StandardUId_Then_Uses_Get(
            string standardUId,
            [Frozen] Mock<IStandardRepository> standardsRepository,
            StandardsService _sut)
        {
            // Arrange
            standardsRepository
                .Setup(r => r.Get(standardUId, CourseType.Apprenticeship))
                .ReturnsAsync((Standard)null);

            // Act
            await _sut.GetStandardByAnyId(standardUId);

            // Assert
            standardsRepository.Verify(r => r.Get(standardUId, CourseType.Apprenticeship), Times.Once);
            standardsRepository.Verify(r => r.GetLatestActiveStandard(It.IsAny<string>(), It.IsAny<CourseType?>()), Times.Never);
            standardsRepository.Verify(r => r.GetLatestActiveStandardByIfateReferenceNumber(It.IsAny<string>(), It.IsAny<CourseType?>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task When_FoundationCourse_Then_Populates_RelatedOccupations(
            [Frozen] Mock<IStandardRepository> standardsRepository,
            StandardsService _sut)
        {
            // Arrange
            var id = "FA0001_1.0";

            var standardFromRepo = new Standard
            {
                StandardUId = id,
                Route = new Route { Name = "Route", Id = 1 },
                CourseType = CourseType.Apprenticeship,
                ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship,
                RelatedOccupations = new List<string> { "FA0002", "FA0003" }
            };

            var relatedStandardsFromRepo = new List<Standard>
            {
                new Standard
                {
                    StandardUId = "FA0002_1.0",
                    IfateReferenceNumber = "FA0002",
                    Route = new Route { Name = "Route", Id = 1 },
                    CourseType = CourseType.Apprenticeship,
                    ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship,
                },
                new Standard
                {
                    StandardUId = "FA0003_1.0",
                    IfateReferenceNumber = "FA0003",
                    Route = new Route { Name = "Route", Id = 1 },
                    CourseType = CourseType.Apprenticeship,
                    ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship,
                }
            };

            standardsRepository
                .Setup(r => r.Get(id, CourseType.Apprenticeship))
                .ReturnsAsync(standardFromRepo);

            standardsRepository
                .Setup(r => r.GetActiveStandardsByIfateReferenceNumbers(
                    It.Is<List<string>>(x => x.Count == 2 && x[0] == "FA0002" && x[1] == "FA0003"),
                    It.IsAny<CourseType?>()))
                .ReturnsAsync(relatedStandardsFromRepo);

            // Act
            var result = await _sut.GetStandardByAnyId(id);

            // Assert
            result.Should().NotBeNull();
            result.RelatedOccupations.Should().NotBeNull();

            standardsRepository.Verify(r => r.Get(id, CourseType.Apprenticeship), Times.Once);
            standardsRepository.Verify(r => r.GetActiveStandardsByIfateReferenceNumbers(It.IsAny<List<string>>(), It.IsAny<CourseType?>()), Times.Once);
        }
    }
}
