using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Services;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Services
{
    public class WhenGettingAStandardByStandardUId
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_ApprenticeshipStandard_Filtered_From_The_Repository(
            string standardUId,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            // Arrange
            mockStandardsRepository
                .Setup(repository => repository.Get(standardUId, CourseType.Apprenticeship))
                .ReturnsAsync(standardFromRepo);

            // Act
            var standard = await service.GetStandard(standardUId, CourseType.Apprenticeship);

            // Assert
            standard.Should().BeEquivalentTo((Domain.Courses.Standard)standardFromRepo);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_A_Standard_Unfiltered_From_The_Repository(
            string standardUId,
            Standard standardFromRepo,
            [Frozen] Mock<IStandardRepository> mockStandardsRepository,
            StandardsService service)
        {
            // Arrange
            mockStandardsRepository
                .Setup(repository => repository.Get(standardUId, null))
                .ReturnsAsync(standardFromRepo);

            // Act
            var standard = await service.GetStandard(standardUId, null);

            // Assert
            standard.Should().BeEquivalentTo((Domain.Courses.Standard)standardFromRepo);
        }
    }
}
