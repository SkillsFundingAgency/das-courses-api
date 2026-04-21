using System;
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

    public class WhenGettingCourseByAnyId
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
            var standardFromRepo = new Standard
            {
                StandardUId = standardUid,
                LarsCode = id,
                Route = new Route { Name = "Route", Id = 1 },
            };

            standardsRepository
                .Setup(r => r.GetLatestActiveStandard(id, null))
                .ReturnsAsync(standardFromRepo);

            // Act
            var result = await _sut.GetCourseByAnyId(id);

            // Assert
            result.Should().NotBeNull();
            standardsRepository.Verify(r => r.Get(It.IsAny<string>(), It.IsAny<CourseType?>()), Times.Never);
            standardsRepository.Verify(r => r.GetLatestActiveStandard(id, null), Times.Once);
            standardsRepository.Verify(r => r.GetLatestActiveStandardByIfateReferenceNumber(It.IsAny<string>(), It.IsAny<CourseType?>()), Times.Never);
        }

        [Test]
        [MoqInlineAutoData("ST0001_1.0", "ST0001", "123")]
        [MoqInlineAutoData("FA0002_1.0", "FA0002", "456")]
        [MoqInlineAutoData("AU0003_1.0", "AU0003", "ZSC00003")]
        public async Task When_Id_Is_ReferenceNumber_Then_Uses_GetLatestActiveStandardByIfateReferenceNumber(
            string standardUid,
            string id,
            string larsCode,
            [Frozen] Mock<IStandardRepository> standardsRepository,
            StandardsService _sut)
        {
            // Arrange
            var standardFromRepo = new Standard
            {
                StandardUId = standardUid,
                LarsCode = larsCode,
                IfateReferenceNumber = id,
                Route = new Route { Name = "Route", Id = 1 },
            };

            standardsRepository
                .Setup(r => r.GetLatestActiveStandardByIfateReferenceNumber(id, null))
                .ReturnsAsync(standardFromRepo);

            // Act
            var result = await _sut.GetCourseByAnyId(id);

            // Assert
            result.Should().NotBeNull();
            
            standardsRepository.Verify(r => r.Get(It.IsAny<string>(), It.IsAny<CourseType?>()), Times.Never);
            standardsRepository.Verify(r => r.GetLatestActiveStandard(It.IsAny<string>(), It.IsAny<CourseType?>()), Times.Never);
            standardsRepository.Verify(r => r.GetLatestActiveStandardByIfateReferenceNumber(id, null), Times.Once);
        }

        [Test]
        [MoqInlineAutoData("ST0001_1.0", "123", CourseType.Apprenticeship)]
        [MoqInlineAutoData("FA0002_1.0", "456", CourseType.Apprenticeship)]
        [MoqInlineAutoData("AU0003_1.0", "ZSC00003", CourseType.ShortCourse)]
        public async Task When_Id_Is_StandardUId_Then_Uses_Get(
            string standardUId,
            string larsCode,
            CourseType courseType,
            [Frozen] Mock<IStandardRepository> standardsRepository,
            StandardsService _sut)
        {
            // Arrange
            var standardFromRepo = new Standard
            {
                StandardUId = standardUId,
                LarsCode = larsCode,
                CourseType = courseType,
                Route = new Route { Name = "Route", Id = 1 },
            };

            standardsRepository
                .Setup(r => r.Get(standardUId, null))
                .ReturnsAsync(standardFromRepo);

            // Act
            var result = await _sut.GetCourseByAnyId(standardUId);

            // Assert
            result.Should().NotBeNull();
            standardsRepository.Verify(r => r.Get(standardUId, null), Times.Once);
            standardsRepository.Verify(r => r.GetLatestActiveStandard(standardUId, null), Times.Never);
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
                LarsCode = "123",
                Route = new Route { Name = "Route", Id = 1 },
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
                    ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship,
                },
                new Standard
                {
                    StandardUId = "FA0003_1.0",
                    IfateReferenceNumber = "FA0003",
                    Route = new Route { Name = "Route", Id = 1 },
                    ApprenticeshipType = ApprenticeshipType.FoundationApprenticeship,
                }
            };

            standardsRepository
                .Setup(r => r.GetLatestActiveStandard(standardFromRepo.LarsCode, null))
                .ReturnsAsync(standardFromRepo);

            standardsRepository
                .Setup(r => r.Get(id, null))
                .ReturnsAsync(standardFromRepo);

            standardsRepository
                .Setup(r => r.GetActiveStandardsByIfateReferenceNumbers(
                    It.Is<List<string>>(x => x.Count == 2 && x[0] == "FA0002" && x[1] == "FA0003"),
                    It.IsAny<CourseType?>()))
                .ReturnsAsync(relatedStandardsFromRepo);

            // Act
            var result = await _sut.GetCourseByAnyId(id);

            // Assert
            result.Should().NotBeNull();
            result.RelatedOccupations.Should().NotBeNull();

            standardsRepository.Verify(r => r.Get(id, null), Times.Once);
            standardsRepository.Verify(r => r.GetActiveStandardsByIfateReferenceNumbers(It.IsAny<List<string>>(), It.IsAny<CourseType?>()), Times.Once);
        }

        [Test]
        [MoqInlineAutoData("ZSC00123")]
        public async Task When_ShortCourse_And_CourseDates_Null_Then_Populates_CourseDates(
            string larsCode,
            [Frozen] Mock<IStandardRepository> standardsRepository,
            StandardsService _sut)
        {
            // Arrange
            var earliestApproved = new DateTime(2020, 1, 1, 0, 0, 0,DateTimeKind.Utc);
            var latestStart = new DateTime(2030, 6, 30, 0, 0, 0, DateTimeKind.Utc);

            var latestActive = new Standard
            {
                LarsCode = larsCode,
                CourseType = CourseType.ShortCourse,
                ShortCourseDates = new ShortCourseDates
                {
                    EffectiveFrom = earliestApproved,
                    EffectiveTo = latestStart,
                    LastDateStarts = latestStart
                },
                VersionLatestStartDate = latestStart,
                ApprovedForDelivery = earliestApproved,
                Route = new Route { Name = "Route", Id = 1 }
            };

            standardsRepository
                .Setup(r => r.GetLatestActiveStandard(larsCode, null))
                .ReturnsAsync(latestActive);

            // Act
            var result = await _sut.GetCourseByAnyId(larsCode);

            // Assert
            result.Should().NotBeNull();
            result.CourseDates.Should().NotBeNull();
            result.CourseDates.EffectiveFrom.Should().Be(earliestApproved);
            result.CourseDates.EffectiveTo.Should().Be(latestStart);
            result.CourseDates.LastDateStarts.Should().Be(latestStart);

            standardsRepository.Verify(r => r.GetLatestActiveStandard(larsCode, null), Times.Once);
        }
    }
}
